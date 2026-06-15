using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.UI.Gameplay.PauseMenu;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Visualizer.Gameplay
{
    public class GameplayVisualizer
    {
        public event Action EventResumeUIButtonClicked;
        public event Action EventMainMenuUIButtonClicked;

        private UIGameplayCanvas uiGameplayCanvas;
        private UIPauseMenuPanel uiPauseMenuPanel;

        public void Initialize()
        {
            uiGameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();

            if (uiGameplayCanvas == null)
            {
                DebugLogHelper.LogError("Failed to initialize GameplayVisualizer: UIGameplayCanvas not found in scene.");
            }
            uiGameplayCanvas.Initialize();

            uiPauseMenuPanel = uiGameplayCanvas?.UIPauseMenuPanel;
            if(uiPauseMenuPanel == null)
            {
                DebugLogHelper.LogError("Failed to initialize GameplayVisualizer: UIPauseMenuPanel not found in UIGameplayCanvas.");
                return;
            }

            //HidePanel();
            Subscribe();
        }

        public void Dispose()
        {
            uiGameplayCanvas = null;
        }

        public void Subscribe()
        {
            uiPauseMenuPanel.EventResumeUIButtonClicked += OnResumeButtonClicked;
            uiPauseMenuPanel.EventMainMenuUIButtonClicked += OnMainMenuButtonClicked;
        }

        public void OnResumeButtonClicked()
        {
           EventResumeUIButtonClicked?.Invoke();
        }

        public void OnMainMenuButtonClicked()
        {
           EventMainMenuUIButtonClicked?.Invoke();
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
