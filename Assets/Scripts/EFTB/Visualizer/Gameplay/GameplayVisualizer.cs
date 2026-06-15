using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.UI.Gameplay.PauseMenu;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Visualizer.Gameplay
{
    public class GameplayVisualizer
    {
        public event Action EventPauseUIButtonClicked;
        public event Action EventResumeUIButtonClicked;
        public event Action EventMainMenuUIButtonClicked;

        private CollectibleManager collectibleManager;

        private UIGameplayCanvas uiGameplayCanvas;
        private UIPauseMenuPanel uiPauseMenuPanel;
        private UIGameplayPanel uiGameplayPanel;

        public void Initialize()
        {
            uiGameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();

            if (uiGameplayCanvas == null)
            {
                DebugLogHelper.LogError("Failed to initialize GameplayVisualizer: UIGameplayCanvas not found in scene.");
            }
            uiGameplayCanvas.Initialize();

            collectibleManager = GameContext.Instance.Get<CollectibleManager>();

            SetCoinCounterLabel(collectibleManager.TotalCoinValue);

            uiPauseMenuPanel = uiGameplayCanvas?.UIPauseMenuPanel;
            uiGameplayPanel = uiGameplayCanvas?.UIGameplayPanel;

            HidePanel();
            Subscribe();
        }

        public void Dispose()
        {
            UnSubscribe();
            uiGameplayCanvas = null;
        }

        public void Subscribe()
        {
            uiGameplayPanel.EventPauseUIButtonClicked += OnPauseButtonClicked;

            uiPauseMenuPanel.EventResumeUIButtonClicked += OnResumeButtonClicked;
            uiPauseMenuPanel.EventMainMenuUIButtonClicked += OnMainMenuButtonClicked;

            collectibleManager.EventTotalCoinValueChanged += SetCoinCounterLabel;
        }

        public void UnSubscribe()
        {
            uiGameplayPanel.EventPauseUIButtonClicked -= OnPauseButtonClicked;
            
            uiPauseMenuPanel.EventResumeUIButtonClicked -= OnResumeButtonClicked;
            uiPauseMenuPanel.EventMainMenuUIButtonClicked -= OnMainMenuButtonClicked;

            collectibleManager.EventTotalCoinValueChanged -= SetCoinCounterLabel;
        }

        public void OnPauseButtonClicked()
        {
            EventPauseUIButtonClicked?.Invoke();
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

        public void SetFinishLevelTextLabel(GameStatus gameStatus)
        {
            uiGameplayCanvas?.SetFinishLevelTextLabel(gameStatus);
        }

        public void ShowGameplayCanvas()
        {
            uiGameplayCanvas?.Show();
        }

        public void HideGameplayCanvas()
        {
            uiGameplayCanvas?.Hide();
        }
    }
}
