mergeInto(LibraryManager.library, {
  MiniHubGetParentAuthInfo: function (gameObjectNamePtr, successCallbackPtr, errorCallbackPtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr || 0);
    var successCallback = UTF8ToString(successCallbackPtr || 0);
    var errorCallback = UTF8ToString(errorCallbackPtr || 0);

    function sendUnityMessage(methodName, payload) {
      if (!gameObjectName || !methodName) return;

      if (typeof SendMessage === "function") {
        SendMessage(gameObjectName, methodName, payload);
        return;
      }

      if (typeof unityInstance !== "undefined" && unityInstance && typeof unityInstance.SendMessage === "function") {
        unityInstance.SendMessage(gameObjectName, methodName, payload);
      }
    }

    var authInvoker =
      window.MiniHubSDK && typeof window.MiniHubSDK.getParentAuthInfo === "function"
        ? window.MiniHubSDK.getParentAuthInfo
        : null;

    if (!authInvoker) {
      sendUnityMessage(
        errorCallback,
        JSON.stringify({
          error: "MiniHubSDK.getParentAuthInfo is not available.",
        })
      );
      return;
    }

    Promise.resolve(authInvoker())
      .then(function (result) {
        sendUnityMessage(
          successCallback,
          JSON.stringify({
            token: result && result.token ? String(result.token) : "",
            apiBase: result && result.apiBase ? String(result.apiBase) : "",
          })
        );
      })
      .catch(function (error) {
        sendUnityMessage(
          errorCallback,
          JSON.stringify({
            error: error && error.message ? error.message : String(error),
          })
        );
      });
  },

  MiniHubGetProfile: function (gameObjectNamePtr, successCallbackPtr, errorCallbackPtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr || 0);
    var successCallback = UTF8ToString(successCallbackPtr || 0);
    var errorCallback = UTF8ToString(errorCallbackPtr || 0);

    function sendUnityMessage(methodName, payload) {
      if (!gameObjectName || !methodName) return;

      if (typeof SendMessage === "function") {
        SendMessage(gameObjectName, methodName, payload);
        return;
      }

      if (typeof unityInstance !== "undefined" && unityInstance && typeof unityInstance.SendMessage === "function") {
        unityInstance.SendMessage(gameObjectName, methodName, payload);
      }
    }

    var profileInvoker =
      window.MiniHubSDK && typeof window.MiniHubSDK.getProfile === "function"
        ? window.MiniHubSDK.getProfile
        : null;

    if (!profileInvoker) {
      sendUnityMessage(
        errorCallback,
        JSON.stringify({
          error: "MiniHubSDK.getProfile is not available.",
        })
      );
      return;
    }

    Promise.resolve(profileInvoker())
      .then(function (result) {
        sendUnityMessage(successCallback, JSON.stringify(result || {}));
      })
      .catch(function (error) {
        sendUnityMessage(
          errorCallback,
          JSON.stringify({
            error: error && error.message ? error.message : String(error),
          })
        );
      });
  },

  MiniHubStartGameSession: function (gameObjectNamePtr, successCallbackPtr, errorCallbackPtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr || 0);
    var successCallback = UTF8ToString(successCallbackPtr || 0);
    var errorCallback = UTF8ToString(errorCallbackPtr || 0);

    function sendUnityMessage(methodName, payload) {
      if (!gameObjectName || !methodName) return;

      if (typeof SendMessage === "function") {
        SendMessage(gameObjectName, methodName, payload);
        return;
      }

      if (typeof unityInstance !== "undefined" && unityInstance && typeof unityInstance.SendMessage === "function") {
        unityInstance.SendMessage(gameObjectName, methodName, payload);
      }
    }

    var startInvoker =
      window.MiniHubSDK && typeof window.MiniHubSDK.startGameSession === "function"
        ? window.MiniHubSDK.startGameSession
        : null;

    if (!startInvoker) {
      sendUnityMessage(
        errorCallback,
        JSON.stringify({
          error: "MiniHubSDK.startGameSession is not available.",
        })
      );
      return;
    }

    Promise.resolve(startInvoker())
      .then(function () {
        sendUnityMessage(
          successCallback,
          JSON.stringify({
            isSuccess: true,
          })
        );
      })
      .catch(function (error) {
        sendUnityMessage(
          errorCallback,
          JSON.stringify({
            error: error && error.message ? error.message : String(error),
          })
        );
      });
  },

  MiniHubEndGameSession: function (score, gameObjectNamePtr, successCallbackPtr, errorCallbackPtr) {
    var gameObjectName = UTF8ToString(gameObjectNamePtr || 0);
    var successCallback = UTF8ToString(successCallbackPtr || 0);
    var errorCallback = UTF8ToString(errorCallbackPtr || 0);

    function sendUnityMessage(methodName, payload) {
      if (!gameObjectName || !methodName) return;

      if (typeof SendMessage === "function") {
        SendMessage(gameObjectName, methodName, payload);
        return;
      }

      if (typeof unityInstance !== "undefined" && unityInstance && typeof unityInstance.SendMessage === "function") {
        unityInstance.SendMessage(gameObjectName, methodName, payload);
      }
    }

    var endInvoker =
      window.MiniHubSDK && typeof window.MiniHubSDK.endGameSession === "function"
        ? window.MiniHubSDK.endGameSession
        : null;

    if (!endInvoker) {
      sendUnityMessage(
        errorCallback,
        JSON.stringify({
          score: score,
          error: "MiniHubSDK.endGameSession is not available.",
        })
      );
      return;
    }

    Promise.resolve(endInvoker(score))
      .then(function () {
        sendUnityMessage(
          successCallback,
          JSON.stringify({
            isSuccess: true,
            score: score,
          })
        );
      })
      .catch(function (error) {
        sendUnityMessage(
          errorCallback,
          JSON.stringify({
            score: score,
            error: error && error.message ? error.message : String(error),
          })
        );
      });
  },

  MiniHubCloseGame: function () {
    var closeInvoker =
      window.MiniHubSDK && typeof window.MiniHubSDK.closeGame === "function"
        ? window.MiniHubSDK.closeGame
        : null;

    if (closeInvoker) {
      closeInvoker();
    }
  },
});
