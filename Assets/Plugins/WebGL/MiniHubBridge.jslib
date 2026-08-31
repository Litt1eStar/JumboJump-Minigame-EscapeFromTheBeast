mergeInto(LibraryManager.library, {

    MiniHubGetParentAuthInfo: function (objectNamePtr, successCallbackPtr, errorCallbackPtr) {
        var objectName = UTF8ToString(objectNamePtr);
        var successCallback = UTF8ToString(successCallbackPtr);
        var errorCallback = UTF8ToString(errorCallbackPtr);

        if (window.MiniHubPlatform && typeof window.MiniHubPlatform.getParentAuthInfo === 'function') {
            window.MiniHubPlatform.getParentAuthInfo()
                .then(function (res) {
                    unityInstance.SendMessage(objectName, successCallback, JSON.stringify(res));
                })
                .catch(function (err) {
                    unityInstance.SendMessage(objectName, errorCallback, JSON.stringify(err));
                });
        } else {
            unityInstance.SendMessage(objectName, errorCallback, "MiniHubPlatform.getParentAuthInfo is not available.");
        }
    },

    MiniHubGetProfile: function (objectNamePtr, successCallbackPtr, errorCallbackPtr) {
        var objectName = UTF8ToString(objectNamePtr);
        var successCallback = UTF8ToString(successCallbackPtr);
        var errorCallback = UTF8ToString(errorCallbackPtr);

        if (window.MiniHubPlatform && typeof window.MiniHubPlatform.getProfile === 'function') {
            window.MiniHubPlatform.getProfile()
                .then(function (res) {
                    unityInstance.SendMessage(objectName, successCallback, JSON.stringify(res));
                })
                .catch(function (err) {
                    unityInstance.SendMessage(objectName, errorCallback, JSON.stringify(err));
                });
        } else {
            unityInstance.SendMessage(objectName, errorCallback, "MiniHubPlatform.getProfile is not available.");
        }
    },

    MiniHubGetWallet: function (objectNamePtr, successCallbackPtr, errorCallbackPtr) {
        var objectName = UTF8ToString(objectNamePtr);
        var successCallback = UTF8ToString(successCallbackPtr);
        var errorCallback = UTF8ToString(errorCallbackPtr);

        if (window.MiniHubPlatform && typeof window.MiniHubPlatform.getWallet === 'function') {
            window.MiniHubPlatform.getWallet()
                .then(function (res) {
                    unityInstance.SendMessage(objectName, successCallback, JSON.stringify(res));
                })
                .catch(function (err) {
                    unityInstance.SendMessage(objectName, errorCallback, JSON.stringify(err));
                });
        } else {
            unityInstance.SendMessage(objectName, errorCallback, "MiniHubPlatform.getWallet is not available.");
        }
    },

    MiniHubStartGameSession: function (objectNamePtr, successCallbackPtr, errorCallbackPtr) {
        var objectName = UTF8ToString(objectNamePtr);
        var successCallback = UTF8ToString(successCallbackPtr);
        var errorCallback = UTF8ToString(errorCallbackPtr);

        if (window.MiniHubPlatform && typeof window.MiniHubPlatform.startGameSession === 'function') {
            window.MiniHubPlatform.startGameSession()
                .then(function (res) {
                    unityInstance.SendMessage(objectName, successCallback, JSON.stringify(res));
                })
                .catch(function (err) {
                    unityInstance.SendMessage(objectName, errorCallback, JSON.stringify(err));
                });
        } else {
            unityInstance.SendMessage(objectName, errorCallback, "MiniHubPlatform.startGameSession is not available.");
        }
    },

    MiniHubEndGameSession: function (score, objectNamePtr, successCallbackPtr, errorCallbackPtr) {
        var objectName = UTF8ToString(objectNamePtr);
        var successCallback = UTF8ToString(successCallbackPtr);
        var errorCallback = UTF8ToString(errorCallbackPtr);

        if (window.MiniHubPlatform && typeof window.MiniHubPlatform.endGameSession === 'function') {
            window.MiniHubPlatform.endGameSession(score)
                .then(function (res) {
                    unityInstance.SendMessage(objectName, successCallback, JSON.stringify(res));
                })
                .catch(function (err) {
                    unityInstance.SendMessage(objectName, errorCallback, JSON.stringify(err));
                });
        } else {
            unityInstance.SendMessage(objectName, errorCallback, "MiniHubPlatform.endGameSession is not available.");
        }
    },

    MiniHubCloseGame: function () {
        if (window.MiniHubPlatform && typeof window.MiniHubPlatform.closeGame === 'function') {
            window.MiniHubPlatform.closeGame();
        } else {
            console.log("MiniHubPlatform.closeGame is not available.");
        }
    }
});
