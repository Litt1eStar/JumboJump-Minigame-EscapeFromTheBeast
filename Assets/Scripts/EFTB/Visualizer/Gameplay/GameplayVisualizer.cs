using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.UI.Gameplay.FinishLevel;
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
        public event Action EventFinishMainMenuButtonClicked;

        private CollectibleManager collectibleManager;
        private GameplayController gameplayController;

        private UIGameplayCanvas uiGameplayCanvas;
        private UIPauseMenuPanel uiPauseMenuPanel;
        private UIGameplayPanel uiGameplayPanel;
        private UIFinishLevelPanel uiFinishLevelPanel;

        public void Initialize(GameplayController gameplayController)
        {
            uiGameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();

            if (uiGameplayCanvas == null)
            {
                DebugLogHelper.LogError("Failed to initialize GameplayVisualizer: UIGameplayCanvas not found in scene.");
            }
            uiGameplayCanvas.Initialize();

            this.gameplayController = gameplayController;
            collectibleManager = GameContext.Instance.Get<CollectibleManager>();

            SetCoinCounterLabel(collectibleManager.TotalCoinValue);

            uiPauseMenuPanel = uiGameplayCanvas?.UIPauseMenuPanel;
            uiGameplayPanel = uiGameplayCanvas?.UIGameplayPanel;
            uiFinishLevelPanel = uiGameplayCanvas?.UIFinishLevelPanel;

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

            uiFinishLevelPanel.EventMainMenuUIButtonClicked += OnFinishMainMenuButtonClicked;
            collectibleManager.EventTotalCoinValueChanged += SetCoinCounterLabel;

            gameplayController.EventFinishLevel += OnLevelFinished;
        }

        public void UnSubscribe()
        {
            uiGameplayPanel.EventPauseUIButtonClicked -= OnPauseButtonClicked;

            uiPauseMenuPanel.EventResumeUIButtonClicked -= OnResumeButtonClicked;
            uiPauseMenuPanel.EventMainMenuUIButtonClicked -= OnMainMenuButtonClicked;

            uiFinishLevelPanel.EventMainMenuUIButtonClicked -= OnFinishMainMenuButtonClicked;
            collectibleManager.EventTotalCoinValueChanged -= SetCoinCounterLabel;

            if (gameplayController != null)
            {
                gameplayController.EventFinishLevel -= OnLevelFinished;
            }
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

        public void OnFinishMainMenuButtonClicked()
        {
            EventFinishMainMenuButtonClicked?.Invoke();
        }

        public void OnLevelFinished(GameStatus gameStatus)
        {
            SetFinishLevelTextLabel(gameStatus);
            ShowFinishLevelPanel();
        }

        public void SetCoinCounterLabel(int value)
        {
            uiGameplayCanvas?.SetCoinCounterLabel(value);
        }

        public void ShowPauseMenu()
        {
            uiPauseMenuPanel.Show();
        }

        public void HidePanel()
        {
            uiPauseMenuPanel?.Hide();
            uiFinishLevelPanel?.Hide();
        }

        public void ShowFinishLevelPanel()
        {
            uiFinishLevelPanel?.Show();
        }

        public void SetFinishLevelTextLabel(GameStatus gameStatus)
        {
            uiGameplayCanvas?.SetFinishLevelTextLabel(gameStatus);
        }

        public void ShowGameplayCanvas()
        {
            uiGameplayCanvas?.ShowGameplayPanel();
        }

        public void HideGameplayCanvas()
        {
            uiGameplayCanvas?.HideGameplayPanel();
        }
    }
}
