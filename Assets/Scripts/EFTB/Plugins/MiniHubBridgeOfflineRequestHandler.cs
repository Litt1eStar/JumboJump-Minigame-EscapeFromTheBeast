using System;
using UnityEngine;

namespace JumboJumps.EFTB.Plugins
{
    public class MiniHubBridgeOfflineRequestHandler
    {
        private const int MOCK_WALLET_CHIPS = 1000;
        private const int MOCK_WALLET_STARS = 100000;
        private const int MOCK_STAMINA = 5;
        private const int MOCK_LEVEL = 1;
        private const string MOCK_DISPLAY_NAME = "Offline Player";
        private const string MOCK_AVATAR_ID = "offline-avatar";
        private const string MOCK_STATUS = "offline";
        private const string MOCK_PLATFORM_LANGUAGE = "TH";

        public MiniHubBridge.ParentAuthInfoResponseModel HandleGetParentAuthInfoRequest(string apiBase)
        {
            return new MiniHubBridge.ParentAuthInfoResponseModel("mock_offline_token_12345", apiBase);
        }

        public MiniHubBridge.ProfileResponseModel HandleGetProfileRequest()
        {
            var now = DateTime.UtcNow;

            return new MiniHubBridge.ProfileResponseModel
            (
                new MiniHubBridge.ProfileDataResponseModel
                (
                    "offline-player-id-001",
                    MOCK_DISPLAY_NAME,
                    new MiniHubBridge.AvatarResponseModel(MOCK_AVATAR_ID, string.Empty),
                    MOCK_LEVEL,
                    MOCK_STATUS,
                    MOCK_PLATFORM_LANGUAGE
                ),
                new MiniHubBridge.TokenResponseModel
                (
                    "mock_offline_token_12345",
                    86400,
                    now.AddDays(1).ToString("O"),
                    true
                )
            );
        }

        public MiniHubBridge.WalletResponseModel HandleGetWalletRequest()
        {
            return new MiniHubBridge.WalletResponseModel(MOCK_WALLET_CHIPS, MOCK_WALLET_STARS);
        }

        public MiniHubBridge.PurchaseProductResponseModel HandlePurchaseProductRequest(string productId, string clientRequestId = null)
        {
            return new MiniHubBridge.PurchaseProductResponseModel(Guid.NewGuid().ToString(), productId, clientRequestId);
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
                "offline-player-id-001",
                new MiniHubBridge.StaminaResponseModel(now, MOCK_STAMINA),
                MOCK_WALLET_CHIPS
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
                MOCK_WALLET_CHIPS,
                MOCK_WALLET_STARS,
                score
            );
        }
    }
}
