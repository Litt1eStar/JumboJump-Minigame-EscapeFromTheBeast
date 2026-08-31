(() => {
  "use strict";

  let sdkInstance = null;

  function createMiniHubSdk({ playerId, gameId }) {
    const apiState = {
      accessToken: null,
      profile: null,
      sessionId: null,
      sessionStartMs: null,
    };

    let parentJwtCache = null;
    let parentApiBaseCache = null;
    let parentJwtPending = null;

    function canAccessParentWindow() {
      if (window.parent === window) return false;
      try {
        void window.parent.location.href;
        return true;
      } catch (ex) {
        return false;
      }
    }

    async function getParentAuthInfo() {
      if (parentJwtCache) {
        return { token: parentJwtCache, apiBase: parentApiBaseCache };
      }
      if (window.parent === window) return null;
      if (parentJwtPending) return parentJwtPending;

      parentJwtPending = (async () => {
        try {
          if (canAccessParentWindow() && typeof window.parent.getJwtToken === "function") {
            const result = await Promise.resolve(window.parent.getJwtToken());
            if (typeof result === "string" || !result) {
              parentJwtCache = result || null;
              return { token: parentJwtCache, apiBase: null };
            }

            parentJwtCache = result.token || null;
            parentApiBaseCache = result.apiBase || null;
            return { token: parentJwtCache, apiBase: parentApiBaseCache };
          }
        } catch (ex) {
          // Ignore direct access failures and fall back to postMessage.
        }

        return await new Promise((resolve) => {
          const channel = `mwgp_jwt_${Date.now()}_${Math.random().toString(16).slice(2)}`;
          const timeoutId = setTimeout(() => {
            cleanup();
            resolve(null);
          }, 1200);

          function onMessage(event) {
            if (event.source !== window.parent) return;
            const data = event.data;
            if (!data || data.type !== "MWGP_JWT_RESPONSE" || data.channel !== channel) return;
            cleanup();
            parentApiBaseCache = data.apiBase || null;
            resolve({
              token: data.token || null,
              apiBase: parentApiBaseCache,
            });
          }

          function cleanup() {
            clearTimeout(timeoutId);
            window.removeEventListener("message", onMessage);
          }

          window.addEventListener("message", onMessage);
          window.parent.postMessage({ type: "MWGP_JWT_REQUEST", channel }, "*");
        });
      })();

      const info = await parentJwtPending;
      parentJwtCache = info?.token || null;
      parentApiBaseCache = info?.apiBase || null;
      parentJwtPending = null;

      return info || null;
    }

    async function apiPost(path, body, token = null) {
      const parentAuth = await getParentAuthInfo();

      const authToken = parentAuth?.token || token;
      const baseUrl = parentAuth?.apiBase;
      if (!baseUrl) {
        throw new Error("Missing API base URL from parent.");
      }
      const res = await fetch(`${baseUrl}${path}`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          ...(authToken ? { Authorization: `Bearer ${authToken}` } : {}),
        },
        body: JSON.stringify(body),
      });
      const data = await res.json().catch(() => null);
      if (!res.ok || !data || data.isSuccess === false) {
        const err = data?.error || `HTTP ${res.status}`;
        throw new Error(err);
      }
      return data.data;
    }

    async function sha256Hex(input) {
      if (!crypto?.subtle) return "";
      const bytes = new TextEncoder().encode(input);
      const hash = await crypto.subtle.digest("SHA-256", bytes);
      return [...new Uint8Array(hash)].map((b) => b.toString(16).padStart(2, "0")).join("");
    }

    async function ensureProfile() {
      if (apiState.accessToken && apiState.profile) return apiState.profile;
      const profile = await apiPost("/auth/profile", { playerId });
      apiState.profile = profile || null;
      apiState.accessToken = profile?.token?.accessToken || null;
      return apiState.profile;
    }

    async function getProfile() {
      const profile = await ensureProfile();
      if (!profile) {
        throw new Error("Missing profile.");
      }
      return profile;
    }

    async function startGameSession() {
      await ensureProfile();
      if (!apiState.accessToken) throw new Error("Missing access token.");

      const start = await apiPost("/game/start", { gameId }, apiState.accessToken);
      apiState.sessionId = start.sessionId || null;
      apiState.sessionStartMs = Date.now();
      if (!apiState.sessionId) throw new Error("Missing sessionId.");
    }

    async function endGameSession(score) {
      if (!apiState.accessToken || !apiState.sessionId) return;
      const clientTime = Math.max(0, Math.round((Date.now() - apiState.sessionStartMs) / 1000));
      const clientHash = await sha256Hex(`${apiState.sessionId}|${score}|${clientTime}`);
      await apiPost("/game/end", {
        sessionId: apiState.sessionId,
        score,
        clientTime,
        clientHash,
      }, apiState.accessToken);
    }

    function notifyContinue(result = {}) {
      const payload = {
        gameId,
        playerId,
        score: Number.isFinite(result.score) ? result.score : null,
        sessionId: apiState.sessionId,
        clientTime: Number.isFinite(result.clientTime) ? result.clientTime : null,
        source: "minigame-spinning-cat",
      };

      try {
        if (canAccessParentWindow() && typeof window.parent.openGameResultModal === "function") {
          window.parent.openGameResultModal(payload);
          return true;
        }
      } catch (ex) {
        // Ignore direct parent access errors; postMessage fallback below.
      }

      if (window.parent && window.parent !== window) {
        window.parent.postMessage({ type: "MWGP_GAME_RESULT_CONTINUE", payload }, "*");
        return true;
      }

      return false;
    }

    function postCloseGameMessage(targetWindow, payload) {
      if (!targetWindow || targetWindow === window) return false;

      targetWindow.postMessage({ type: "MWGP_CLOSE_GAME", payload }, "*");
      return true;
    }

    function closeGame() {
      const payload = {
        gameId,
        playerId,
        source: "minigame-spinning-cat",
      };

      try {
        if (canAccessParentWindow() && typeof window.parent.closeGame === "function") {
          window.parent.closeGame(payload);
          return true;
        }
      } catch (ex) {
        // Ignore direct parent access errors; postMessage fallback below.
      }

      if (postCloseGameMessage(window.parent, payload)) {
        return true;
      }

      if (postCloseGameMessage(window.top, payload)) {
        return true;
      }

      return false;
    }

    const sdk = {
      getParentAuthInfo,
      getProfile,
      ensureProfile,
      startGameSession,
      endGameSession,
      notifyContinue,
      closeGame,
      apiPost,
    };

    sdkInstance = sdk;
    return sdk;
  }

  async function getParentAuthInfo() {
    if (!sdkInstance || typeof sdkInstance.getParentAuthInfo !== "function") {
      throw new Error("MiniHubSDK has not been created yet.");
    }
    return sdkInstance.getParentAuthInfo();
  }

  async function startGameSession() {
    if (!sdkInstance || typeof sdkInstance.startGameSession !== "function") {
      throw new Error("MiniHubSDK has not been created yet.");
    }
    return sdkInstance.startGameSession();
  }

  async function endGameSession(score) {
    if (!sdkInstance || typeof sdkInstance.endGameSession !== "function") {
      throw new Error("MiniHubSDK has not been created yet.");
    }
    return sdkInstance.endGameSession(score);
  }

  async function getProfile() {
    if (!sdkInstance || typeof sdkInstance.getProfile !== "function") {
      throw new Error("MiniHubSDK has not been created yet.");
    }
    return sdkInstance.getProfile();
  }

  function closeGame() {
    if (!sdkInstance || typeof sdkInstance.closeGame !== "function") {
      throw new Error("MiniHubSDK has not been created yet.");
    }
    return sdkInstance.closeGame();
  }

  window.MiniHubSDK = {
    create: createMiniHubSdk,
    getParentAuthInfo,
    getProfile,
    startGameSession,
    endGameSession,
    closeGame,
  };
})();
