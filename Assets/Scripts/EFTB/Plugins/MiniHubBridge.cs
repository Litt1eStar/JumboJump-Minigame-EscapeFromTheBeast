using JumboJumps.EFTB.Utilities;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace JumboJumps.EFTB.Plugins
{
    public class MiniHubBridge : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
        [DllImport("__Internal")]
        private static extern void MiniHubGetParentAuthInfo(string gameObjectName, string successCallback, string errorCallback);

        [DllImport("__Internal")]
        private static extern void MiniHubGetProfile(string gameObjectName, string successCallback, string errorCallback);

        [DllImport("__Internal")]
        private static extern void MiniHubGetWallet(string gameObjectName, string successCallback, string errorCallback);

        [DllImport("__Internal")]
        private static extern void MiniHubPurchaseProduct(string productId, string clientRequestId, string gameObjectName, string successCallback, string errorCallback);

        [DllImport("__Internal")]
        private static extern void MiniHubStartGameSession(string gameObjectName, string successCallback, string errorCallback);

        [DllImport("__Internal")]
        private static extern void MiniHubEndGameSession(int score, string gameObjectName, string successCallback, string errorCallback);

        [DllImport("__Internal")]
        private static extern void MiniHubCloseGame();
#endif

        [SerializeField]
        private string editorToken = string.Empty;

        [SerializeField]
        private string editorApiBase = string.Empty;

        private Action<bool, ParentAuthInfoResponseModel, string> pendingGetParentAuthInfoCallback;
        private Action<bool, ProfileResponseModel, string> pendingGetProfileCallback;
        private Action<bool, WalletResponseModel, string> pendingGetWalletCallback;
        private Action<bool, PurchaseProductResponseModel, string> pendingPurchaseProductCallback;
        private Action<bool, StartSessionResponseModel, string> pendingStartGameSessionCallback;
        private Action<bool, EndSessionResponseModel, string> pendingEndGameSessionCallback;

        private readonly MiniHubBridgeOfflineRequestHandler offlineRequestHandler = new MiniHubBridgeOfflineRequestHandler();

        public void GetParentAuthInfo(Action<bool, ParentAuthInfoResponseModel, string> callback)
        {
            if (!TryBeginRequest(ref pendingGetParentAuthInfoCallback, callback, nameof(GetParentAuthInfo)))
            {
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubGetParentAuthInfo(gameObject.name, nameof(OnGetParentAuthInfoSuccess), nameof(OnGetParentAuthInfoError));
#else
            CompleteGetParentAuthInfoSuccess(offlineRequestHandler.HandleGetParentAuthInfoRequest(editorApiBase));
#endif
        }

        public void GetProfile(Action<bool, ProfileResponseModel, string> callback)
        {
            if (!TryBeginRequest(ref pendingGetProfileCallback, callback, nameof(GetProfile)))
            {
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubGetProfile(gameObject.name, nameof(OnGetProfileSuccess), nameof(OnGetProfileError));
#else
            CompleteGetProfileSuccess(offlineRequestHandler.HandleGetProfileRequest());
#endif
        }

        public void GetWallet(Action<bool, WalletResponseModel, string> callback)
        {
            if (!TryBeginRequest(ref pendingGetWalletCallback, callback, nameof(GetWallet)))
            {
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubGetWallet(gameObject.name, nameof(OnGetWalletSuccess), nameof(OnGetWalletError));
#else
            CompleteGetWalletSuccess(offlineRequestHandler.HandleGetWalletRequest());
#endif
        }

        public void PurchaseProduct(string productId, Action<bool, PurchaseProductResponseModel, string> callback, string clientRequestId = null)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                callback?.Invoke(false, null, "ProductId is required.");
                return;
            }

            if (!TryBeginRequest(ref pendingPurchaseProductCallback, callback, nameof(PurchaseProduct)))
            {
                return;
            }

            clientRequestId = ResolveClientRequestId(clientRequestId);

#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubPurchaseProduct(productId, clientRequestId, gameObject.name, nameof(OnPurchaseProductSuccess), nameof(OnPurchaseProductError));
#else
            CompletePurchaseProductSuccess(offlineRequestHandler.HandlePurchaseProductRequest(productId, clientRequestId));
#endif
        }

        public void StartGameSession(Action<bool, StartSessionResponseModel, string> callback)
        {
            DebugLogHelper.Log($"[{GetType().Name}] StartGameSession requested.");
            if (!TryBeginRequest(ref pendingStartGameSessionCallback, callback, nameof(StartGameSession)))
            {
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubStartGameSession(gameObject.name, nameof(OnStartGameSessionSuccess), nameof(OnStartGameSessionError));
#else
            CompleteStartGameSessionSuccess(offlineRequestHandler.HandleStartGameSessionRequest());
#endif
        }

        public void EndGameSession(int score, Action<bool, EndSessionResponseModel, string> callback)
        {
            DebugLogHelper.Log($"[{GetType().Name}] EndGameSession requested for Score: {score}.");
            if (!TryBeginRequest(ref pendingEndGameSessionCallback, callback, nameof(EndGameSession)))
            {
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubEndGameSession(score, gameObject.name, nameof(OnEndGameSessionSuccess), nameof(OnEndGameSessionError));
#else
            CompleteEndGameSessionSuccess(offlineRequestHandler.HandleEndGameSessionRequest(score));
#endif
        }

        public void CloseGame()
        {
            DebugLogHelper.Log($"[{GetType().Name}] CloseGame requested.");
#if UNITY_WEBGL && !UNITY_EDITOR && !OFFLINE_MODE && !OFFLINE_MINIHUB
            MiniHubCloseGame();
#else
            DebugLogHelper.Log($"[{GetType().Name}] CloseGame called in Editor / Offline mode.");
            Application.Quit();
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

            CompleteGetProfileSuccess(response);
        }

        public void OnGetProfileError(string payload)
        {
            CompleteGetProfileError(ReadErrorMessage(payload, nameof(OnGetProfileError)));
        }

        public void OnGetWalletSuccess(string payload)
        {
            var response = DeserializePayload<WalletResponseModel>(payload, nameof(OnGetWalletSuccess));
            CompleteGetWalletSuccess(response ?? new WalletResponseModel(0, 0));
        }

        public void OnGetWalletError(string payload)
        {
            CompleteGetWalletError(ReadErrorMessage(payload, nameof(OnGetWalletError)));
        }

        public void OnPurchaseProductSuccess(string payload)
        {
            var response = DeserializePayload<PurchaseProductResponseModel>(payload, nameof(OnPurchaseProductSuccess));

            if (response == null || string.IsNullOrWhiteSpace(response.ReceiptId))
            {
                CompletePurchaseProductError("Purchase response is missing receiptId.");
                return;
            }

            CompletePurchaseProductSuccess(response);
        }

        public void OnPurchaseProductError(string payload)
        {
            CompletePurchaseProductError(ReadErrorMessage(payload, nameof(OnPurchaseProductError)));
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

            CompleteEndGameSessionSuccess(response.Data ?? new EndSessionResponseModel(false));
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
                DebugLogHelper.LogError($"[{GetType().Name}] {operationName} was called while another request was still pending.");
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
                DebugLogHelper.LogException(ex);
                DebugLogHelper.LogError($"[{GetType().Name}] {callbackName} failed to deserialize payload: {payload}");
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

        private string ResolveClientRequestId(string clientRequestId)
        {
            return string.IsNullOrWhiteSpace(clientRequestId)
                ? $"unity_{Guid.NewGuid():N}"
                : clientRequestId;
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

        private void CompleteGetWalletSuccess(WalletResponseModel response)
        {
            var callback = pendingGetWalletCallback;
            pendingGetWalletCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompleteGetWalletError(string error)
        {
            var callback = pendingGetWalletCallback;
            pendingGetWalletCallback = null;
            callback?.Invoke(false, null, error);
        }

        private void CompletePurchaseProductSuccess(PurchaseProductResponseModel response)
        {
            var callback = pendingPurchaseProductCallback;
            pendingPurchaseProductCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompletePurchaseProductError(string error)
        {
            var callback = pendingPurchaseProductCallback;
            pendingPurchaseProductCallback = null;
            callback?.Invoke(false, null, error);
        }

        private void CompleteStartGameSessionSuccess(StartSessionResponseModel response)
        {
            DebugLogHelper.Log($"[{GetType().Name}] StartGameSession Succeeded. SessionId: {response?.SessionId}");
            var callback = pendingStartGameSessionCallback;
            pendingStartGameSessionCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompleteStartGameSessionError(string error)
        {
            DebugLogHelper.LogError($"[{GetType().Name}] StartGameSession Failed: {error}");
            var callback = pendingStartGameSessionCallback;
            pendingStartGameSessionCallback = null;
            callback?.Invoke(false, null, error);
        }

        private void CompleteEndGameSessionSuccess(EndSessionResponseModel response)
        {
            DebugLogHelper.Log($"[{GetType().Name}] EndGameSession Succeeded. Score: {response?.Score}, SessionId: {response?.SessionId}");
            var callback = pendingEndGameSessionCallback;
            pendingEndGameSessionCallback = null;
            callback?.Invoke(true, response, null);
        }

        private void CompleteEndGameSessionError(string error)
        {
            DebugLogHelper.LogError($"[{GetType().Name}] EndGameSession Failed: {error}");
            var callback = pendingEndGameSessionCallback;
            pendingEndGameSessionCallback = null;
            callback?.Invoke(false, null, error);
        }

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

        public class WalletResponseModel
        {
            [JsonProperty("Chips")]
            public decimal Chips { get; private set; }

            [JsonProperty("Stars")]
            public decimal Stars { get; private set; }

            public WalletResponseModel(decimal chips, decimal stars)
            {
                Chips = chips;
                Stars = stars;
            }
        }

        public class PurchaseProductResponseModel
        {
            [JsonProperty("receiptId")]
            public string ReceiptId { get; private set; }

            [JsonProperty("productId")]
            public string ProductId { get; private set; }

            [JsonProperty("clientRequestId")]
            public string ClientRequestId { get; private set; }

            public PurchaseProductResponseModel(string receiptId, string productId, string clientRequestId = null)
            {
                ReceiptId = receiptId;
                ProductId = productId;
                ClientRequestId = clientRequestId;
            }
        }

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

        public class AvatarResponseModel
        {
            [JsonProperty("id")]
            public string Id { get; private set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; private set; }

            public AvatarResponseModel(string id, string imageUrl)
            {
                Id = id;
                ImageUrl = imageUrl;
            }
        }

        public class StaminaResponseModel
        {
            [JsonProperty("lastUpdatedAt")]
            public string LastUpdatedAt { get; private set; }

            [JsonProperty("current")]
            public int Current { get; private set; }

            public StaminaResponseModel(string lastUpdatedAt, int current)
            {
                LastUpdatedAt = lastUpdatedAt;
                Current = current;
            }
        }

        public class TokenResponseModel
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; private set; }

            [JsonProperty("expiresIn")]
            public int ExpiresIn { get; private set; }

            [JsonProperty("expiresAt")]
            public string ExpiresAt { get; private set; }

            [JsonProperty("isAuthenticated")]
            public bool IsAuthenticated { get; private set; }

            public TokenResponseModel(string accessToken, int expiresIn, string expiresAt, bool isAuthenticated)
            {
                AccessToken = accessToken;
                ExpiresIn = expiresIn;
                ExpiresAt = expiresAt;
                IsAuthenticated = isAuthenticated;
            }
        }

        private class MiniHubErrorResponseModel
        {
            [JsonProperty("error")]
            public string Error { get; private set; }

            public MiniHubErrorResponseModel(string error)
            {
                Error = error;
            }
        }

        private class MiniHubApiResponseModel<T>
        {
            [JsonProperty("isSuccess")]
            public bool IsSuccess { get; private set; }

            [JsonProperty("errorCode")]
            public string ErrorCode { get; private set; }

            [JsonProperty("error")]
            public string Error { get; private set; }

            [JsonProperty("data")]
            public T Data { get; private set; }

            public MiniHubApiResponseModel(bool isSuccess, string errorCode, string error, T data)
            {
                IsSuccess = isSuccess;
                ErrorCode = errorCode;
                Error = error;
                Data = data;
            }
        }
    }
}
