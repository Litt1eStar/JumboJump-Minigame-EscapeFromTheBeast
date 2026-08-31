using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Model;
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

        private ScoreManager scoreManager;
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
            scoreManager = GameContext.Instance.Get<ScoreManager>();

            if (scoreManager != null)
            {
                SetScoreLabel(scoreManager.CurrentScoreData);
            }

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
            if (uiGameplayPanel != null) uiGameplayPanel.EventPauseUIButtonClicked += OnPauseButtonClicked;

            if (uiPauseMenuPanel != null)
            {
                uiPauseMenuPanel.EventResumeUIButtonClicked += OnResumeButtonClicked;
                uiPauseMenuPanel.EventMainMenuUIButtonClicked += OnMainMenuButtonClicked;
            }

            if (uiFinishLevelPanel != null)
            {
                uiFinishLevelPanel.EventMainMenuUIButtonClicked += OnFinishMainMenuButtonClicked;
                uiFinishLevelPanel.EventReplayUIButtonClicked += OnFinishMainMenuButtonClicked;
            }

            if (scoreManager != null) scoreManager.EventScoreChanged += SetScoreLabel;
            
            if (gameplayController != null) gameplayController.EventFinishLevel += OnLevelFinished;
        }

        public void UnSubscribe()
        {
            if (uiGameplayPanel != null) uiGameplayPanel.EventPauseUIButtonClicked -= OnPauseButtonClicked;
            if (uiPauseMenuPanel != null)
            {
                uiPauseMenuPanel.EventResumeUIButtonClicked -= OnResumeButtonClicked;
                uiPauseMenuPanel.EventMainMenuUIButtonClicked -= OnMainMenuButtonClicked;
            }
            if (uiFinishLevelPanel != null)
            {
                uiFinishLevelPanel.EventMainMenuUIButtonClicked -= OnFinishMainMenuButtonClicked;
                uiFinishLevelPanel.EventReplayUIButtonClicked -= OnFinishMainMenuButtonClicked;
            }
            
            if (scoreManager != null) scoreManager.EventScoreChanged -= SetScoreLabel;
            if (gameplayController != null) gameplayController.EventFinishLevel -= OnLevelFinished;
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
            HidePanel();
            EventFinishMainMenuButtonClicked?.Invoke();
        }

        public void OnLevelFinished(GameStatus gameStatus)
        {
            if (scoreManager != null)
            {
                uiGameplayCanvas?.SetFinishLevelScore(scoreManager.CurrentScoreData.TotalScore);
            }
            ShowFinishLevelPanel();
        }

        public void SetScoreLabel(ScoreData scoreData)
        {
            uiGameplayCanvas?.SetScoreLabel(scoreData.TotalScore);
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
