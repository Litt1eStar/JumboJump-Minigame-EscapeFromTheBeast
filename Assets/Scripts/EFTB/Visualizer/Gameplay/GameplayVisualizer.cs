using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Sound;
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
        public event Action EventFinishMainMenuButtonClicked;
        public event Action EventSFXToggleClicked;
        public event Action EventBGMToggleClicked;

        private ScoreManager scoreManager;
        private SoundManager soundManager;
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
            uiGameplayCanvas?.Initialize();

            this.gameplayController = gameplayController;
            scoreManager = GameContext.Instance.Get<ScoreManager>();
            soundManager = GameContext.Instance.Get<SoundManager>();

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
            soundManager = null;
        }

        public void Subscribe()
        {
            if (uiGameplayPanel != null) uiGameplayPanel.EventPauseUIButtonClicked += OnPauseButtonClicked;

            if (uiPauseMenuPanel != null)
            {
                uiPauseMenuPanel.EventResumeUIButtonClicked += OnResumeButtonClicked;
                uiPauseMenuPanel.EventSFXUIButtonClicked += OnSFXButtonClicked;
                uiPauseMenuPanel.EventBGMUIButtonClicked += OnBGMButtonClicked;
            }

            if (uiFinishLevelPanel != null)
            {
                uiFinishLevelPanel.EventMainMenuUIButtonClicked += OnFinishMainMenuButtonClicked;
                uiFinishLevelPanel.EventReplayUIButtonClicked += OnFinishMainMenuButtonClicked;
            }

            if (scoreManager != null) scoreManager.EventScoreChanged += SetScoreLabel;
            if (gameplayController != null) gameplayController.EventFinishLevel += OnLevelFinished;

            if (soundManager != null)
            {
                soundManager.EventSFXStateChanged += OnSFXStateChanged;
                soundManager.EventBGMStateChanged += OnBGMStateChanged;
            }
        }

        public void UnSubscribe()
        {
            if (uiGameplayPanel != null) uiGameplayPanel.EventPauseUIButtonClicked -= OnPauseButtonClicked;
            if (uiPauseMenuPanel != null)
            {
                uiPauseMenuPanel.EventResumeUIButtonClicked -= OnResumeButtonClicked;
                uiPauseMenuPanel.EventSFXUIButtonClicked -= OnSFXButtonClicked;
                uiPauseMenuPanel.EventBGMUIButtonClicked -= OnBGMButtonClicked;
            }
            if (uiFinishLevelPanel != null)
            {
                uiFinishLevelPanel.EventMainMenuUIButtonClicked -= OnFinishMainMenuButtonClicked;
                uiFinishLevelPanel.EventReplayUIButtonClicked -= OnFinishMainMenuButtonClicked;
            }
            
            if (scoreManager != null) scoreManager.EventScoreChanged -= SetScoreLabel;
            if (gameplayController != null) gameplayController.EventFinishLevel -= OnLevelFinished;

            if (soundManager != null)
            {
                soundManager.EventSFXStateChanged -= OnSFXStateChanged;
                soundManager.EventBGMStateChanged -= OnBGMStateChanged;
            }
        }

        public void OnPauseButtonClicked()
        {
            EFTBSound.PlayUIClick();
            EventPauseUIButtonClicked?.Invoke();
        }

        public void OnResumeButtonClicked()
        {
            EFTBSound.PlayUIClick();
            EventResumeUIButtonClicked?.Invoke();
        }

        public void OnSFXButtonClicked()
        {
            EFTBSound.ToggleSFX();
            EFTBSound.PlayUIClick();
            EventSFXToggleClicked?.Invoke();
        }

        public void OnBGMButtonClicked()
        {
            EFTBSound.ToggleBGM();
            EFTBSound.PlayUIClick();
            EventBGMToggleClicked?.Invoke();
        }

        public void OnFinishMainMenuButtonClicked()
        {
            EFTBSound.PlayUIClick();
            HidePanel();
            EventFinishMainMenuButtonClicked?.Invoke();
        }

        public void OnLevelFinished(GameStatus gameStatus)
        {
            if (scoreManager != null)
            {
                uiGameplayCanvas?.SetFinishLevelScore(scoreManager.CurrentScoreData.TotalScore);
            }
            EFTBSound.PlayGameOver();
            ShowFinishLevelPanel();
        }

        public void SetScoreLabel(ScoreData scoreData)
        {
            uiGameplayCanvas?.SetScoreLabel(scoreData);
        }

        public void ShowPauseMenu()
        {
            RefreshPauseMenuSoundVisuals();
            uiPauseMenuPanel?.Show();
        }

        public void RefreshPauseMenuSoundVisuals()
        {
            if (uiPauseMenuPanel == null) return;
            uiPauseMenuPanel.SetSFXVisualState(EFTBSound.IsSFXOn);
            uiPauseMenuPanel.SetBGMVisualState(EFTBSound.IsBGMOn);
        }

        private void OnSFXStateChanged(bool isOn)
        {
            uiPauseMenuPanel?.SetSFXVisualState(isOn);
        }

        private void OnBGMStateChanged(bool isOn)
        {
            uiPauseMenuPanel?.SetBGMVisualState(isOn);
        }

        public void HidePanel()
        {
            uiPauseMenuPanel?.Hide();
            uiFinishLevelPanel?.Hide();
            HideGameplayCanvas();
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
