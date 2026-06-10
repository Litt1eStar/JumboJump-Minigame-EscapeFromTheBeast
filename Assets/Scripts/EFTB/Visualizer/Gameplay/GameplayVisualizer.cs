using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.UI.Gameplay.PauseMenu;
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

            HidePanel();
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

        public void HidePanel()
        {
            uiGameplayCanvas?.HidePauseMenu();
            uiGameplayCanvas?.HideFinishLevelPanel();
        }

        public void OnFinishLevel(GameStatus gameStatus)
        {
            SetFinishLevelTextLabel(gameStatus);
            ShowFinishLevelPanel();
        }
        public void ShowFinishLevelPanel()
        {
            uiGameplayCanvas?.ShowFinishLevelPanel();
        }

        public void HideFinishLevelPanel()
        {
            uiGameplayCanvas?.HideFinishLevelPanel();
        }

        public void SetFinishLevelTextLabel(GameStatus gameStatus)
        {
            uiGameplayCanvas?.SetFinishLevelTextLabel(gameStatus);
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
