using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using JumboJumps.EFTB.Visualizer.Gameplay;
using System;

namespace JumboJumps.EFTB.State.Gameplay
{
    public enum GameStatus
    {
        None,
        Win,
        Lose
    }
    public class GameplayController
    {
        /// <summary>
        /// EventReturnBackToMainMenu will triggerd when player want to go back to main menu
        /// </summary>
        public event Action EventReturnBackToMainMenu;

        /// <summary>
        /// Parameter : GameStatus - Win or Lose
        /// </summary>
        public event Action<GameStatus> EventFinishLevel;

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

            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
            
            collectibleManager?.Dispose();
            collectibleManager = null;
            
            collectibleVisualizer?.Dispose();
            collectibleVisualizer = null;

            gameplayVisualizer?.Dispose();
            gameplayVisualizer = null;
        }

        public void Subscribe()
        {
            collectibleManager.EventTotalCoinValueChanged += OnCoinCollected;
            EventFinishLevel += OnFinishLevel;
        }

        public void Unsubscribe()
        {
            collectibleManager.EventTotalCoinValueChanged -= OnCoinCollected;
        }

        public void OnFinishLevel(GameStatus gameStatus)
        {
            gameplayVisualizer.OnFinishLevel(gameStatus);
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
            gameplayVisualizer.HidePanel();
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

        public void InvokeFinishLevel(GameStatus gameStatus)
        {
            EventFinishLevel?.Invoke(gameStatus);
        }
    }
}
