(() => {
  "use strict";

  const TARGET_GAMEOBJECT_NAME = "GameInitializer";
  let isSuspended = false;

  function sendLifecycleMessage(methodName) {
    const unityInstance = window.unityGameInstance || window.unityInstance;
    if (!unityInstance) return;

    if (typeof unityInstance.SendMessage === "function") {
      unityInstance.SendMessage(TARGET_GAMEOBJECT_NAME, methodName);
    }
  }

  function setAppSuspended(suspended) {
    if (isSuspended === suspended) return;
    isSuspended = suspended;

    if (isSuspended) {
      sendLifecycleMessage("OnWebAppSuspended");
    } else {
      sendLifecycleMessage("OnWebAppResumed");
    }
  }

  function updateVisibilityState() {
    const isHidden = document.hidden || document.visibilityState === "hidden";
    setAppSuspended(isHidden);
  }

  document.addEventListener("visibilitychange", updateVisibilityState, false);
  window.addEventListener("pagehide", () => setAppSuspended(true), false);
  window.addEventListener("pageshow", () => updateVisibilityState(), false);
  window.addEventListener("blur", () => setAppSuspended(true), false);
  window.addEventListener("focus", () => updateVisibilityState(), false);

  updateVisibilityState();
})();
