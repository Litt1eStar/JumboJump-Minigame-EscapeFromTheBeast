using JumboJumps.EFTB.Utilities;
using JumboJumps.SpinningCat.Plugins.WebBridge;

using System;
using static JumboJumps.SpinningCat.Plugins.WebBridge.MiniHubBridge;

namespace JumboJumps.SpinningCat.Manager
{
    public class MiniHubManager
    {
        private MiniHubBridge miniHubBridge;

        /// <summary>
        /// Aggregated callbacks for GetParentAuthInfo.
        /// Parameter: Whether the parent auth info was loaded successfully.
        /// </summary>
        private Action<bool> pendingGetParentAuthCallbacks;

        /// <summary>
        /// Aggregated callbacks for GetProfile.
        /// Parameter: Whether the profile was loaded successfully.
        /// </summary>
        private Action<bool> pendingGetProfileCallbacks;

        private bool isGetParentAuthInProgress;
        private bool isGetProfileInProgress;

        public ParentAuthInfoResponseModel CachedParentAuthInfo { get; private set; }
        public ProfileResponseModel CachedProfile { get; private set; }
        public bool IsReady { get; private set; }

        public string ParentAuthToken => CachedParentAuthInfo?.Token;

        public void Initialize(MiniHubBridge miniHubBridge)
        {
            this.miniHubBridge = miniHubBridge;
            IsReady = false;
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);

            pendingGetParentAuthCallbacks = null;
            pendingGetProfileCallbacks = null;
            isGetParentAuthInProgress = false;
            isGetProfileInProgress = false;
            CachedParentAuthInfo = null;
            CachedProfile = null;
            IsReady = false;
            miniHubBridge = null;
        }

        /// <summary>
        /// Fetch and cache the parent auth info (JWT token + API base URL).
        /// </summary>
        /// <param name="callback">Invoked with true if parent auth info was loaded successfully, false otherwise.</param>
        public void GetParentAuthInfo(Action<bool> callback)
        {
            if (CachedParentAuthInfo != null)
            {
                callback?.Invoke(true);
                return;
            }

            if (miniHubBridge == null)
            {
                LogError($"{nameof(GetParentAuthInfo)}| {nameof(MiniHubBridge)} is not initialized.");
                callback?.Invoke(false);
                return;
            }

            if (isGetParentAuthInProgress)
            {
                pendingGetParentAuthCallbacks += callback;
                return;
            }

            isGetParentAuthInProgress = true;
            pendingGetParentAuthCallbacks = callback;
            miniHubBridge.GetParentAuthInfo(OnGetParentAuthInfoCompleted);
        }

        /// <summary>
        /// Fetch and cache the player profile. Sets IsReady to true on success.
        /// </summary>
        /// <param name="callback">Invoked with true if profile was loaded successfully, false otherwise.</param>
        public void GetProfile(Action<bool> callback)
        {
            if (CachedProfile != null)
            {
                callback?.Invoke(true);
                return;
            }

            if (miniHubBridge == null)
            {
                LogError($"{nameof(GetProfile)}| {nameof(MiniHubBridge)} is not initialized.");
                callback?.Invoke(false);
                return;
            }

            if (isGetProfileInProgress)
            {
                pendingGetProfileCallbacks += callback;
                return;
            }

            isGetProfileInProgress = true;
            pendingGetProfileCallbacks = callback;
            miniHubBridge.GetProfile(OnGetProfileCompleted);
        }

        /// <summary>
        /// Start a game session via the platform SDK.
        /// </summary>
        /// <param name="callback">Invoked with success flag, session response data, and error message if failed.</param>
        public void StartGameSession(Action<bool, StartSessionResponseModel, string> callback)
        {
            if (miniHubBridge == null)
            {
                LogError($"{nameof(StartGameSession)}| {nameof(MiniHubBridge)} is not initialized.");
                callback?.Invoke(false, null, $"{nameof(MiniHubBridge)} is not initialized.");
                return;
            }

            miniHubBridge.StartGameSession(callback);
        }

        /// <summary>
        /// End the current game session and submit the final score.
        /// </summary>
        /// <param name="score">The player's final score.</param>
        /// <param name="callback">Invoked with success flag, session response data, and error message if failed.</param>
        public void EndGameSession(int score, Action<bool, EndSessionResponseModel, string> callback)
        {
            if (miniHubBridge == null)
            {
                LogError($"{nameof(EndGameSession)}| {nameof(MiniHubBridge)} is not initialized.");
                callback?.Invoke(false, null, $"{nameof(MiniHubBridge)} is not initialized.");
                return;
            }

            miniHubBridge.EndGameSession(score, callback);
        }

        /// <summary>
        /// Close the game and notify the platform parent frame.
        /// </summary>
        public void CloseGame()
        {
            if (miniHubBridge == null)
            {
                LogError($"{nameof(CloseGame)}| {nameof(MiniHubBridge)} is not initialized.");
                return;
            }

            miniHubBridge.CloseGame();
        }

        private void OnGetParentAuthInfoCompleted(bool isSuccess, ParentAuthInfoResponseModel response, string error)
        {
            if (!isSuccess)
            {
                LogError($"{nameof(OnGetParentAuthInfoCompleted)}| Failed to load parent auth info. Error: {error}");
                CompleteGetParentAuthInfo(false);
                return;
            }

            if (response == null || string.IsNullOrWhiteSpace(response.Token))
            {
                LogError($"{nameof(OnGetParentAuthInfoCompleted)}| MiniHub parent auth info is missing token.");
                CompleteGetParentAuthInfo(false);
                return;
            }

            CachedParentAuthInfo = response;
            CompleteGetParentAuthInfo(true);
        }

        private void OnGetProfileCompleted(bool isSuccess, ProfileResponseModel response, string error)
        {
            if (!isSuccess)
            {
                LogError($"{nameof(OnGetProfileCompleted)}| Failed to load profile. Error: {error}");
                CompleteGetProfile(false);
                return;
            }

            CachedProfile = response ?? new ProfileResponseModel(null, null);
            IsReady = true;
            CompleteGetProfile(true);
        }

        private void CompleteGetParentAuthInfo(bool isSuccess)
        {
            if (!isSuccess)
            {
                LogError($"{nameof(CompleteGetParentAuthInfo)}| Failed to load parent auth info.");
            }

            var callbacks = pendingGetParentAuthCallbacks;
            pendingGetParentAuthCallbacks = null;
            isGetParentAuthInProgress = false;
            callbacks?.Invoke(isSuccess);
        }

        private void CompleteGetProfile(bool isSuccess)
        {
            if (!isSuccess)
            {
                LogError($"{nameof(CompleteGetProfile)}| Failed to load profile.");
            }

            var callbacks = pendingGetProfileCallbacks;
            pendingGetProfileCallbacks = null;
            isGetProfileInProgress = false;
            callbacks?.Invoke(isSuccess);
        }

        private void LogError(string message)
        {
            DebugLogHelper.LogError($"[{GetType().Name}] {message}");
        }
    }
}
