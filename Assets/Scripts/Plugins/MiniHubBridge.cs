using JumboJumps.EFTB.Utilities;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace JumboJumps.SpinningCat.Plugins.WebBridge
{
    public class MiniHubBridge : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void MiniHubGetParentAuthInfo(string gameObjectName, string successCallback, string errorCallback);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void MiniHubGetProfile(string gameObjectName, string successCallback, string errorCallback);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void MiniHubStartGameSession(string gameObjectName, string successCallback, string errorCallback);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void MiniHubEndGameSession(int score, string gameObjectName, string successCallback, string errorCallback);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void MiniHubCloseGame();
#endif

        [SerializeField]
        private string editorToken = string.Empty;

        [SerializeField]
        private string editorApiBase = string.Empty;

        private Action<bool, ParentAuthInfoResponseModel, string> pendingGetParentAuthInfoCallback;
        private Action<bool, ProfileResponseModel, string> pendingGetProfileCallback;
        private Action<bool, StartSessionResponseModel, string> pendingStartGameSessionCallback;
        private Action<bool, EndSessionResponseModel, string> pendingEndGameSessionCallback;

#if OFFLINE_MODE || OFFLINE_MINIHUB
        private readonly MiniHubBridgeOfflineRequestHandler offlineRequestHandler = new MiniHubBridgeOfflineRequestHandler();
#endif

        public void GetParentAuthInfo(Action<bool, ParentAuthInfoResponseModel, string> callback)
        {
            if (!TryBeginRequest(ref pendingGetParentAuthInfoCallback, callback, nameof(GetParentAuthInfo)))
            {
                return;
            }

#if OFFLINE_MODE || OFFLINE_MINIHUB
            CompleteGetParentAuthInfoSuccess(offlineRequestHandler.HandleGetParentAuthInfoRequest(editorApiBase));
#elif UNITY_WEBGL && !UNITY_EDITOR
            MiniHubGetParentAuthInfo(gameObject.name, nameof(OnGetParentAuthInfoSuccess), nameof(OnGetParentAuthInfoError));
#else
            CompleteGetParentAuthInfoSuccess(new ParentAuthInfoResponseModel(editorToken, editorApiBase));
#endif
        }

        public void GetProfile(Action<bool, ProfileResponseModel, string> callback)
        {
            if (!TryBeginRequest(ref pendingGetProfileCallback, callback, nameof(GetProfile)))
            {
                return;
            }

#if OFFLINE_MODE || OFFLINE_MINIHUB
            CompleteGetProfileSuccess(offlineRequestHandler.HandleGetProfileRequest());
#elif UNITY_WEBGL && !UNITY_EDITOR
            MiniHubGetProfile(gameObject.name, nameof(OnGetProfileSuccess), nameof(OnGetProfileError));
#else
            CompleteGetProfileSuccess(new ProfileResponseModel(null, null));
#endif
        }

        public void StartGameSession(Action<bool, StartSessionResponseModel, string> callback)
        {
            if (!TryBeginRequest(ref pendingStartGameSessionCallback, callback, nameof(StartGameSession)))
            {
                return;
            }

#if OFFLINE_MODE || OFFLINE_MINIHUB
            CompleteStartGameSessionSuccess(offlineRequestHandler.HandleStartGameSessionRequest());
#elif UNITY_WEBGL && !UNITY_EDITOR
            MiniHubStartGameSession(gameObject.name, nameof(OnStartGameSessionSuccess), nameof(OnStartGameSessionError));
#else
            CompleteStartGameSessionSuccess(new StartSessionResponseModel(true));
#endif
        }

        public void EndGameSession(int score, Action<bool, EndSessionResponseModel, string> callback)
        {
            if (!TryBeginRequest(ref pendingEndGameSessionCallback, callback, nameof(EndGameSession)))
            {
                return;
            }

#if OFFLINE_MODE || OFFLINE_MINIHUB
            CompleteEndGameSessionSuccess(offlineRequestHandler.HandleEndGameSessionRequest(score));
#elif UNITY_WEBGL && !UNITY_EDITOR
            MiniHubEndGameSession(score, gameObject.name, nameof(OnEndGameSessionSuccess), nameof(OnEndGameSessionError));
#else
            CompleteEndGameSessionSuccess(new EndSessionResponseModel(true, score: score));
#endif
        }

        public void CloseGame()
        {
#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubCloseGame();
#endif
        }

        public void OnGetParentAuthInfoSuccess(string payload)
        {
            var response = DeserializePayload<ParentAuthInfoResponseModel>(payload, nameof(OnGetParentAuthInfoSuccess));
            CompleteGetParentAuthInfoSuccess(response ?? new ParentAuthInfoResponseModel(null, null));
        }

        public void OnGetParentAuthInfoError(string payload)
        {
            CompleteGetParentAuthInfoError(ReadErrorMessage(payload, nameof(OnGetParentAuthInfoError)));
        }

        public void OnGetProfileSuccess(string payload)
        {
            var response = DeserializePayload<ProfileResponseModel>(payload, nameof(OnGetProfileSuccess));

            if (response == null)
            {
                CompleteGetProfileError("Get profile response is invalid.");
                return;
            }

            if (response.Profile == null && response.Token == null)
            {
                CompleteGetProfileError("Get profile response is invalid.");
                return;
            }

            CompleteGetProfileSuccess(response);
        }

        public void OnGetProfileError(string payload)
        {
            CompleteGetProfileError(ReadErrorMessage(payload, nameof(OnGetProfileError)));
        }

        public void OnStartGameSessionSuccess(string payload)
        {
            var response = DeserializePayload<MiniHubApiResponseModel<StartSessionResponseModel>>(payload, nameof(OnStartGameSessionSuccess));

            if (response == null)
            {
                CompleteStartGameSessionError("Start game session response is invalid.");
                return;
            }

            if (!response.IsSuccess)
            {
                CompleteStartGameSessionError(string.IsNullOrWhiteSpace(response.Error) ? "Start game session failed." : response.Error);
                return;
            }

            CompleteStartGameSessionSuccess(response.Data ?? new StartSessionResponseModel(false));
        }

        public void OnStartGameSessionError(string payload)
        {
            CompleteStartGameSessionError(ReadErrorMessage(payload, nameof(OnStartGameSessionError)));
        }

        public void OnEndGameSessionSuccess(string payload)
        {
            var response = DeserializePayload<MiniHubApiResponseModel<EndSessionResponseModel>>(payload, nameof(OnEndGameSessionSuccess));

            if (response == null)
            {
                CompleteEndGameSessionError("End game session response is invalid.");
                return;
            }

            if (!response.IsSuccess)
            {
                CompleteEndGameSessionError(string.IsNullOrWhiteSpace(response.Error) ? "End game session failed." : response.Error);
                return;
            }

            var endSessionData = response.Data;

            if (endSessionData == null)
            {
                endSessionData = DeserializePayload<EndSessionResponseModel>(payload, nameof(OnEndGameSessionSuccess));
            }

            CompleteEndGameSessionSuccess(endSessionData ?? new EndSessionResponseModel(false));
        }

        public void OnEndGameSessionError(string payload)
        {
            CompleteEndGameSessionError(ReadErrorMessage(payload, nameof(OnEndGameSessionError)));
        }

        private bool TryBeginRequest<T>(ref Action<bool, T, string> pendingCallback, Action<bool, T, string> callback, string operationName)
        {
            if (pendingCallback != null)
            {
                callback?.Invoke(false, default, $"{operationName} is already in progress.");
                LogError($"{operationName} was called while another request was still pending.");
                return false;
            }

            pendingCallback = callback;
            return true;
        }

        private T DeserializePayload<T>(string payload, string callbackName) where T : class
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(payload);
            }
            catch (Exception ex)
            {
                LogException(ex);
                LogError($"{callbackName} failed to deserialize payload: {payload}");
                return null;
            }
        }

        private string ReadErrorMessage(string payload, string callbackName)
        {
            var response = DeserializePayload<MiniHubErrorResponseModel>(payload, callbackName);
            if (response != null && !string.IsNullOrWhiteSpace(response.Error))
            {
                return response.Error;
            }

            return string.IsNullOrWhiteSpace(payload) ? "Unknown MiniHub error." : payload;
        }

        private void CompleteGetParentAuthInfoSuccess(ParentAuthInfoResponseModel response)
        {
            var callback = pendingGetParentAuthInfoCallback;
            pendingGetParentAuthInfoCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompleteGetParentAuthInfoError(string error)
        {
            var callback = pendingGetParentAuthInfoCallback;
            pendingGetParentAuthInfoCallback = null;
            callback?.Invoke(false, null, error);
        }

        private void CompleteGetProfileSuccess(ProfileResponseModel response)
        {
            var callback = pendingGetProfileCallback;
            pendingGetProfileCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompleteGetProfileError(string error)
        {
            var callback = pendingGetProfileCallback;
            pendingGetProfileCallback = null;
            callback?.Invoke(false, null, error);
        }

        private void CompleteStartGameSessionSuccess(StartSessionResponseModel response)
        {
            var callback = pendingStartGameSessionCallback;
            pendingStartGameSessionCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompleteStartGameSessionError(string error)
        {
            var callback = pendingStartGameSessionCallback;
            pendingStartGameSessionCallback = null;
            callback?.Invoke(false, null, error);
        }

        private void CompleteEndGameSessionSuccess(EndSessionResponseModel response)
        {
            var callback = pendingEndGameSessionCallback;
            pendingEndGameSessionCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompleteEndGameSessionError(string error)
        {
            var callback = pendingEndGameSessionCallback;
            pendingEndGameSessionCallback = null;
            callback?.Invoke(false, null, error);
        }

        private void LogError(string message)
        {
            DebugLogHelper.LogError($"[{GetType().Name}] {message}");
        }

        private void LogException(Exception ex)
        {
            DebugLogHelper.LogException(ex);
        }

        [Preserve]
        public class ParentAuthInfoResponseModel
        {
            [JsonProperty("token")]
            public string Token { get; private set; }

            [JsonProperty("apiBase")]
            public string ApiBase { get; private set; }

            public ParentAuthInfoResponseModel(string token, string apiBase)
            {
                Token = token;
                ApiBase = apiBase;
            }
        }

        [Preserve]
        public class ProfileResponseModel
        {
            [JsonProperty("profile")]
            public ProfileDataResponseModel Profile { get; private set; }

            [JsonProperty("token")]
            public TokenResponseModel Token { get; private set; }

            public ProfileResponseModel(ProfileDataResponseModel profile, TokenResponseModel token)
            {
                Profile = profile;
                Token = token;
            }
        }

        [Preserve]
        public class ProfileDataResponseModel
        {
            [JsonProperty("playerId")]
            public string PlatformUserId { get; private set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; private set; }

            [JsonProperty("avatar")]
            public AvatarResponseModel Avatar { get; private set; }

            [JsonProperty("level")]
            public int Level { get; private set; }

            [JsonProperty("status")]
            public string Status { get; private set; }

            [JsonProperty("platformLanguage")]
            public string LanguageCode { get; private set; }

            public ProfileDataResponseModel(string platformUserId,
                                            string displayName,
                                            AvatarResponseModel avatar,
                                            int level,
                                            string status,
                                            string languageCode = null)
            {
                PlatformUserId = platformUserId;
                DisplayName = displayName;
                Avatar = avatar;
                Level = level;
                Status = status;
                LanguageCode = languageCode;
            }
        }

        [Preserve]
        public class AvatarResponseModel
        {
            [JsonProperty("avatarId")]
            public string AvatarId { get; private set; }

            [JsonProperty("avatarUrl")]
            public string AvatarUrl { get; private set; }

            public AvatarResponseModel(string avatarId, string avatarUrl)
            {
                AvatarId = avatarId;
                AvatarUrl = avatarUrl;
            }
        }

        [Preserve]
        public class TokenResponseModel
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; private set; }

            [JsonProperty("expiresIn")]
            public int ExpiresIn { get; private set; }

            [JsonProperty("expiresAt")]
            public string ExpiresAt { get; private set; }

            [JsonProperty("isTemporary")]
            public bool IsTemporary { get; private set; }

            public TokenResponseModel(string accessToken, int expiresIn, string expiresAt, bool isTemporary)
            {
                AccessToken = accessToken;
                ExpiresIn = expiresIn;
                ExpiresAt = expiresAt;
                IsTemporary = isTemporary;
            }
        }

        [Preserve]
        public class StartSessionResponseModel
        {
            [JsonProperty("isSuccess")]
            public bool IsSuccess { get; private set; }

            [JsonProperty("sessionId")]
            public string SessionId { get; private set; }

            [JsonProperty("status")]
            public string Status { get; private set; }

            [JsonProperty("startedAt")]
            public string StartedAt { get; private set; }

            [JsonProperty("consumedBy")]
            public string ConsumedBy { get; private set; }

            [JsonProperty("stamina")]
            public StaminaResponseModel Stamina { get; private set; }

            [JsonProperty("chipsBalance")]
            public int ChipsBalance { get; private set; }

            public StartSessionResponseModel(bool isSuccess,
                                             string sessionId = null,
                                             string status = null,
                                             string startedAt = null,
                                             string consumedBy = null,
                                             StaminaResponseModel stamina = null,
                                             int chipsBalance = 0)
            {
                IsSuccess = isSuccess;
                SessionId = sessionId;
                Status = status;
                StartedAt = startedAt;
                ConsumedBy = consumedBy;
                Stamina = stamina;
                ChipsBalance = chipsBalance;
            }
        }

        [Preserve]
        public class EndSessionResponseModel
        {
            [JsonProperty("isSuccess")]
            public bool IsSuccess { get; private set; }

            [JsonProperty("sessionId")]
            public string SessionId { get; private set; }

            [JsonProperty("status")]
            public string Status { get; private set; }

            [JsonProperty("endedAt")]
            public string EndedAt { get; private set; }

            [JsonProperty("stamina")]
            public StaminaResponseModel Stamina { get; private set; }

            [JsonProperty("chipsBalance")]
            public int ChipsBalance { get; private set; }

            [JsonProperty("stars")]
            public int Stars { get; private set; }

            [JsonProperty("score")]
            public int Score { get; private set; }

            public EndSessionResponseModel(bool isSuccess,
                                           string sessionId = null,
                                           string status = null,
                                           string endedAt = null,
                                           StaminaResponseModel stamina = null,
                                           int chipsBalance = 0,
                                           int stars = 0,
                                           int score = 0)
            {
                IsSuccess = isSuccess;
                SessionId = sessionId;
                Status = status;
                EndedAt = endedAt;
                Stamina = stamina;
                ChipsBalance = chipsBalance;
                Stars = stars;
                Score = score;
            }
        }

        [Preserve]
        public class StaminaResponseModel
        {
            [JsonProperty("lastRegeneratedAt")]
            public string LastRegeneratedAt { get; private set; }

            [JsonProperty("current")]
            public int Current { get; private set; }

            public StaminaResponseModel(string lastRegeneratedAt, int current)
            {
                LastRegeneratedAt = lastRegeneratedAt;
                Current = current;
            }
        }

        [Preserve]
        public class MiniHubErrorResponseModel
        {
            [JsonProperty("error")]
            public string Error { get; private set; }
        }

        [Preserve]
        public class MiniHubApiResponseModel<T>
        {
            [JsonProperty("isSuccess")]
            public bool IsSuccess { get; private set; }

            [JsonProperty("data")]
            public T Data { get; private set; }

            [JsonProperty("error")]
            public string Error { get; private set; }
        }
    }
}
