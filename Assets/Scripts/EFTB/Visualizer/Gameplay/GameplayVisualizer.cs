using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Visualizer.Gameplay
{
    public class GameplayVisualizer
    {
        private UIGameplayCanvas uiGameplayCanvas;

        public void Initialize()
        {
            uiGameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();

            if (uiGameplayCanvas == null)
            {
                DebugLogHelper.LogError("Failed to initialize GameplayVisualizer: UIGameplayCanvas not found in scene.");
            }

            HidePauseMenu();
        }

        public void Dispose()
        {
            uiGameplayCanvas = null;
        }

        public void Subscribe(
            Action pauseBtnCallback,
            Action resumeBtnCallback,
            Action mainMenuCallback
            )
        {
            uiGameplayCanvas?.Subscribe(pauseBtnCallback, resumeBtnCallback, mainMenuCallback);
        }

        public void SetCoinCounterLabel(int value)
        {
            uiGameplayCanvas?.SetCoinCounterLabel(value);
        }

        public void ShowPauseMenu()
        {
            uiGameplayCanvas?.ShowPauseMenu();
        }

        public void HidePauseMenu()
        {
            uiGameplayCanvas?.HidePauseMenu();
        }

        public void Show()
        {
            uiGameplayCanvas?.Show();
        }

        public void Hide()
        {
            uiGameplayCanvas?.Hide();
        }
    }
}
