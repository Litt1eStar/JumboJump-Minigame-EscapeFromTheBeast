using JumboJumps.EFTB.Constant.Network;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    public static class OfflinePlatformUserIdHelper
    {
        private const string DEFAULT_OFFLINE_PLATFORM_USER_ID = "offline-platform-user";
        private const string OFFLINE_MINIHUB_PLATFORM_USER_ID_PREFIX = "offline-minihub-";

        public static string GetOrCreate(bool shouldInvalidateMiniHubParentAuthToken = false, string logContext = null)
        {
#if OFFLINE_MINIHUB
            var key = ConstNetwork.OfflineMode.PLAYER_PREF_PLATFORM_USER_ID;
            var platformUserId = PlayerPrefs.GetString(key, string.Empty);

            if (string.IsNullOrWhiteSpace(platformUserId)
                || string.Equals(platformUserId, DEFAULT_OFFLINE_PLATFORM_USER_ID, StringComparison.Ordinal))
            {
                platformUserId = $"{OFFLINE_MINIHUB_PLATFORM_USER_ID_PREFIX}{Guid.NewGuid()}";
                PlayerPrefs.SetString(key, platformUserId);

                if (shouldInvalidateMiniHubParentAuthToken)
                {
                    PlayerPrefs.DeleteKey(ConstNetwork.OfflineMode.PLAYER_PREFS_MINIHUB_PARENT_AUTH_TOKEN);
                }

                PlayerPrefs.Save();

                if (!string.IsNullOrWhiteSpace(logContext))
                {
                    DebugLogHelper.Log($"[{logContext}] Generated offline MiniHub platform user id: {platformUserId}");
                }
            }

            return platformUserId;
#else
            return PlayerPrefs.GetString(ConstNetwork.OfflineMode.PLAYER_PREF_PLATFORM_USER_ID, DEFAULT_OFFLINE_PLATFORM_USER_ID);
#endif
        }
    }
}
