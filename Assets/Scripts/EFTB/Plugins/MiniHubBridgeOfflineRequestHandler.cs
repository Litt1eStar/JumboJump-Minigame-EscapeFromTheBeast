using JumboJumps.EFTB.Constant.Network;
using JumboJumps.EFTB.Utilities;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;

namespace JumboJumps.EFTB.Plugins
{
    public class MiniHubBridgeOfflineRequestHandler
    {
        private const int MOCK_STAMINA = 5;
        private const int MOCK_LEVEL = 1;
        private const string MOCK_DISPLAY_NAME = "Offline Player";
        private const string MOCK_AVATAR_ID = "offline-avatar";
        private const string MOCK_STATUS = "offline";
        private const string MOCK_PLATFORM_LANGUAGE = "TH";
        private const string MOCK_ISSUER = "Jumbo Jumps Co., Ltd.";
        private const string MOCK_AUDIENCE = "MiniGamePlatform";
        private const string MOCK_ROLE = "PLAYER";
        private const string MOCK_JWT_ALGORITHM = "HS256";
        private const string MOCK_JWT_TYPE = "JWT";
        private const string MOCK_JWT_SECRET_KEY = "THIS_IS_A_VERY_SECURE_SECRET_KEY_CHANGE_ME";
        private const string NAME_IDENTIFIER_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string ROLE_CLAIM = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public MiniHubBridge.ParentAuthInfoResponseModel HandleGetParentAuthInfoRequest(string apiBase)
        {
            return new MiniHubBridge.ParentAuthInfoResponseModel(GetOrCreateOfflineParentAuthToken(), apiBase);
        }

        public MiniHubBridge.ProfileResponseModel HandleGetProfileRequest()
        {
            var now = DateTime.UtcNow;

            return new MiniHubBridge.ProfileResponseModel
            (
                new MiniHubBridge.ProfileDataResponseModel
                (
                    GetOfflinePlayerId(),
                    MOCK_DISPLAY_NAME,
                    new MiniHubBridge.AvatarResponseModel(MOCK_AVATAR_ID, string.Empty),
                    MOCK_LEVEL,
                    MOCK_STATUS,
                    MOCK_PLATFORM_LANGUAGE
                ),
                new MiniHubBridge.TokenResponseModel
                (
                    GetOrCreateOfflineParentAuthToken(),
                    86400,
                    now.AddDays(1).ToString("O"),
                    true
                )
            );
        }

        public MiniHubBridge.WalletResponseModel HandleGetWalletRequest()
        {
            return new MiniHubBridge.WalletResponseModel(100, 10);
        }

        public MiniHubBridge.PurchaseProductResponseModel HandlePurchaseProductRequest(string productId, string clientRequestId)
        {
            return new MiniHubBridge.PurchaseProductResponseModel($"offline_receipt_{Guid.NewGuid():N}", productId, clientRequestId);
        }

        public MiniHubBridge.StartSessionResponseModel HandleStartGameSessionRequest()
        {
            var now = DateTime.UtcNow.ToString("O");

            return new MiniHubBridge.StartSessionResponseModel
            (
                true,
                Guid.NewGuid().ToString(),
                MOCK_STATUS,
                now,
                GetOfflinePlayerId(),
                new MiniHubBridge.StaminaResponseModel(now, MOCK_STAMINA),
                0
            );
        }

        public MiniHubBridge.EndSessionResponseModel HandleEndGameSessionRequest(int score)
        {
            var now = DateTime.UtcNow.ToString("O");

            return new MiniHubBridge.EndSessionResponseModel
            (
                true,
                Guid.NewGuid().ToString(),
                MOCK_STATUS,
                now,
                new MiniHubBridge.StaminaResponseModel(now, MOCK_STAMINA),
                0,
                0,
                score
            );
        }

        private string GetOrCreateOfflineParentAuthToken()
        {
            var token = PlayerPrefs.GetString(ConstNetwork.OfflineMode.PLAYER_PREFS_MINIHUB_PARENT_AUTH_TOKEN, string.Empty);
            if (IsValidOfflineJwtToken(token))
            {
                DebugLogHelper.Log($"[{GetType().Name}] Reusing offline MiniHub JWT: {token}");
                return token;
            }

            token = CreateOfflineJwtToken();
            PlayerPrefs.SetString(ConstNetwork.OfflineMode.PLAYER_PREFS_MINIHUB_PARENT_AUTH_TOKEN, token);
            PlayerPrefs.Save();
            DebugLogHelper.Log($"[{GetType().Name}] Generated offline MiniHub JWT: {token}");
            return token;
        }

        private string CreateOfflineJwtToken()
        {
            var platformUserId = GetOfflinePlatformUserId();
            var now = DateTimeOffset.UtcNow;
            var payload = new OfflineJwtPayload
            {
                Credential = platformUserId,
                Username = platformUserId,
                NameIdentifier = GetOfflineAuthorizationId(),
                Role = MOCK_ROLE,
                Exp = now.AddDays(365).ToUnixTimeSeconds(),
                Iss = MOCK_ISSUER,
                Aud = MOCK_AUDIENCE
            };

            var headerJson = JsonConvert.SerializeObject(new OfflineJwtHeader
            {
                Algorithm = MOCK_JWT_ALGORITHM,
                Type = MOCK_JWT_TYPE
            });

            var payloadJson = JsonConvert.SerializeObject(payload);
            var encodedHeader = ToBase64Url(headerJson);
            var encodedPayload = ToBase64Url(payloadJson);
            var signingInput = $"{encodedHeader}.{encodedPayload}";
            var signature = CreateJwtSignature(signingInput);
            return $"{signingInput}.{signature}";
        }

        private bool IsValidOfflineJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            try
            {
                var signingInput = $"{parts[0]}.{parts[1]}";
                var expectedSignature = CreateJwtSignature(signingInput);
                var currentPlatformUserId = GetOfflinePlatformUserId();
                var headerJson = FromBase64Url(parts[0]);
                var header = JsonConvert.DeserializeObject<OfflineJwtHeader>(headerJson);
                var payloadJson = FromBase64Url(parts[1]);
                var payload = JsonConvert.DeserializeObject<OfflineJwtPayload>(payloadJson);
                return header != null
                    && string.Equals(header.Algorithm, MOCK_JWT_ALGORITHM, StringComparison.Ordinal)
                    && string.Equals(header.Type, MOCK_JWT_TYPE, StringComparison.Ordinal)
                    && AreEqualConstantTime(parts[2], expectedSignature)
                    && payload != null
                    && string.Equals(payload.Credential, currentPlatformUserId, StringComparison.Ordinal)
                    && string.Equals(payload.Username, currentPlatformUserId, StringComparison.Ordinal)
                    && !string.IsNullOrWhiteSpace(payload.NameIdentifier)
                    && payload.Iss == MOCK_ISSUER
                    && payload.Aud == MOCK_AUDIENCE;
            }
            catch
            {
                return false;
            }
        }

        private string GetOfflinePlatformUserId()
        {
            return OfflinePlatformUserIdHelper.GetOrCreate(true, GetType().Name);
        }

        private string GetOfflinePlayerId()
        {
            return $"offline-player-{GetOfflinePlatformUserId()}";
        }

        private string GetOfflineAuthorizationId()
        {
            return GetOfflinePlatformUserId();
        }

        private string ToBase64Url(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value))
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private string CreateJwtSignature(string signingInput)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(MOCK_JWT_SECRET_KEY)))
            {
                var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signingInput));
                return ToBase64Url(signatureBytes);
            }
        }

        private string ToBase64Url(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private string FromBase64Url(string value)
        {
            var normalized = value.Replace('-', '+').Replace('_', '/');

            switch (normalized.Length % 4)
            {
                case 2:
                    normalized += "==";
                    break;
                case 3:
                    normalized += "=";
                    break;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(normalized));
        }

        private bool AreEqualConstantTime(string left, string right)
        {
            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            var diff = 0;
            for (int i = 0; i < left.Length; i++)
            {
                diff |= left[i] ^ right[i];
            }

            return diff == 0;
        }

        [Preserve]
        private class OfflineJwtPayload
        {
            [JsonProperty("Credential")]
            public string Credential { get; set; }

            [JsonProperty("Username")]
            public string Username { get; set; }

            [JsonProperty(NAME_IDENTIFIER_CLAIM)]
            public string NameIdentifier { get; set; }

            [JsonProperty(ROLE_CLAIM)]
            public string Role { get; set; }

            [JsonProperty("exp")]
            public long Exp { get; set; }

            [JsonProperty("iss")]
            public string Iss { get; set; }

            [JsonProperty("aud")]
            public string Aud { get; set; }
        }

        [Preserve]
        private class OfflineJwtHeader
        {
            [JsonProperty("alg")]
            public string Algorithm { get; set; }

            [JsonProperty("typ")]
            public string Type { get; set; }
        }
    }
}
