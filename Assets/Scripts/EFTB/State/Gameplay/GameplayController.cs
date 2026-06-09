using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using JumboJumps.EFTB.Visualizer.Gameplay;
using System;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayController
    {
        public event Action EventReturnBackToMainMenu;

        private CollectibleManager collectibleManager;
        private CollectibleVisualizer collectibleVisualizer;
        private GameplayVisualizer gameplayVisualizer;

        private GameplayStateController stateController;
        public void Initialize(GameplayStateController stateController)
        {
            collectibleManager = GameContext.Instance.Get<CollectibleManager>();
            if (collectibleManager == null)
            {
                DebugLogHelper.LogError("GameplayController: CollectibleManager not found in GameContext.");
                return;
            }

            this.stateController = stateController;

            collectibleVisualizer = new CollectibleVisualizer();
            collectibleVisualizer.Initialize();

            gameplayVisualizer = new GameplayVisualizer();
            gameplayVisualizer.Initialize();
            gameplayVisualizer.Subscribe(
                OnClickPauseButton,
                OnClickResumeButton,
                OnClickMainMenuButton
                );

            collectibleManager.EventTotalCoinValueChanged += OnCoinCollected;
        }

        public void Dispose()
        {
            if (collectibleManager != null)
            {
                collectibleManager.EventTotalCoinValueChanged -= OnCoinCollected;
                collectibleManager = null;
            }

            collectibleVisualizer?.Dispose();
            collectibleVisualizer = null;

            gameplayVisualizer?.Dispose();
            gameplayVisualizer = null;
        }

        public void OnCoinCollected(int totalCoinValue)
        {
            gameplayVisualizer.SetCoinCounterLabel(totalCoinValue);
        }

        public void OnClickPauseButton()
        {
            gameplayVisualizer.ShowPauseMenu();
            stateController.ChangeState(typeof(PauseMenuState));
        }

        public void OnClickResumeButton()
        {
            gameplayVisualizer.HidePauseMenu();
            stateController.ChangeState(typeof(InGameState));
        }
        public void OnClickMainMenuButton()
        {
            ReturnToMainMenu();
        }
        public void ReturnToMainMenu()
        {
            EventReturnBackToMainMenu?.Invoke();
        }
    }
}
