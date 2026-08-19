using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Manager
{
    public class SoundManager
    {
        public string CurrentBGMKey { get; private set; }

        public void Initialize()
        {
            GameContext.Instance.Add(this);
            DebugLogHelper.Log($"[{GetType().Name}] Initialized successfully.");
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
            CurrentBGMKey = null;
            DebugLogHelper.Log($"[{GetType().Name}] Disposed.");
        }

        /// <summary>
        /// Play a Sound Effect by sound key.
        /// Log stub when audio clip assets are not yet available.
        /// </summary>
        public void PlaySFX(string soundKey)
        {
            if (string.IsNullOrEmpty(soundKey)) return;

            DebugLogHelper.Log($"[{GetType().Name}] [SFX Stub] Playing sound: '{soundKey}'");
        }

        /// <summary>
        /// Play or change Background Music by BGM key.
        /// Log stub when audio clip assets are not yet available.
        /// </summary>
        public void PlayBGM(string bgmKey)
        {
            if (string.IsNullOrEmpty(bgmKey)) return;
            CurrentBGMKey = bgmKey;

            DebugLogHelper.Log($"[{GetType().Name}] [BGM Stub] Changing BGM to: '{bgmKey}'");
        }

        public void StopBGM()
        {
            CurrentBGMKey = null;
            DebugLogHelper.Log($"[{GetType().Name}] [BGM Stub] Stopped BGM.");
        }
    }
}
