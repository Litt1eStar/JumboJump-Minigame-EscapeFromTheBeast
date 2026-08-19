using JumboJumps.EFTB.Constant.Sound;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Sound
{
    public static class EFTBSound
    {
        private static SoundManager SoundManager => GameContext.Instance.Get<SoundManager>();

        public static void PlaySFX(string soundKey)
        {
            var manager = SoundManager;
            if (manager != null)
            {
                manager.PlaySFX(soundKey);
            }
            else
            {
                DebugLogHelper.LogWarning($"[EFTBSound] SoundManager not found in GameContext.");
            }
        }

        public static void PlayBGM(string bgmKey)
        {
            var manager = SoundManager;
            if (manager != null)
            {
                manager.PlayBGM(bgmKey);
            }
            else
            {
                DebugLogHelper.LogWarning($"[EFTBSound] SoundManager not found in GameContext.");
            }
        }

        public static void StopBGM()
        {
            var manager = SoundManager;
            if (manager != null)
            {
                manager.StopBGM();
            }
        }

        public static void PlayCatMeowSad() => PlaySFX(ConstSound.Keys.Cat.MEOW_SAD);
        public static void PlayTreatCollected() => PlaySFX(ConstSound.Keys.Collectible.TREAT_COLLECT);
        public static void PlayUIClick() => PlaySFX(ConstSound.Keys.UI.BUTTON_CLICK);
        public static void PlayGameOver() => PlaySFX(ConstSound.Keys.UI.GAME_OVER);
        public static void PlayUIReady() => PlaySFX(ConstSound.Keys.UI.READY);
        public static void PlayUIGo() => PlaySFX(ConstSound.Keys.UI.GO);
        public static void PlayGameplayBGM() => PlayBGM(ConstSound.Keys.BGM.GAMEPLAY);
    }
}
