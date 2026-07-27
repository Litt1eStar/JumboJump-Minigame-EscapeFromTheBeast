using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.Gameplay;
using System;

namespace JumboJumps.EFTB.Manager
{
    public class GameplayTimeManager
    {
        /// <summary>
        /// parameter : timer
        /// Invoke when gameplay timer changed
        /// </summary>
        public event Action<float> EventGameplayTimerChanged;

        private LevelGeneratorManager levelGeneratorManager;
        private GameplayTimeVisualizer visualizer;
        private GameplayController gameplayController;
        private GameplayStateManager gameplayStateManager;
        private GameplayDifficultyEnum currentDifficulty;
        public GameplayDifficultyEnum CurrentDifficulty => currentDifficulty;
        public float CurrentTimer { get; private set; } = 0f;
        private float limitPlayTime;
        private float normalRemainingTime;
        private float hardRemainingTime;
        private bool isTimerFinished = false;

        public void Initialize()
        {
            visualizer = new GameplayTimeVisualizer();
            visualizer.Initialize(this);

            levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
            gameplayController = GameContext.Instance.Get<GameplayController>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            currentDifficulty = GameplayDifficultyEnum.Easy;
            limitPlayTime = ConstGameplay.Limit_Play_Time;
            CurrentTimer = limitPlayTime;
            isTimerFinished = false;

            normalRemainingTime = limitPlayTime * (1 - levelGeneratorManager.Config.MediumDifficultyTimePercentage);
            hardRemainingTime = limitPlayTime * (1 - levelGeneratorManager.Config.HardDifficultyTimePercentage);

            GameContext.Instance.Add(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if (isTimerFinished) return;

            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            CurrentTimer -= deltaTime;
            if (CurrentTimer <= 0)
            {
                CurrentTimer = 0f;
                isTimerFinished = true;
                gameplayController?.InvokeFinishLevel(GameStatus.Win);
            }

            HandleDifficultyAdjustment();
            EventGameplayTimerChanged?.Invoke(CurrentTimer);
        }

        public void Dispose()
        {
            CurrentTimer = 0f;
            
            visualizer?.Dispose();
            visualizer = null;

            GameContext.Instance.Remove(this);
        }

        private void HandleDifficultyAdjustment()
        {
            if (CurrentTimer <= normalRemainingTime && CurrentTimer > hardRemainingTime)
            {
                currentDifficulty = GameplayDifficultyEnum.Normal;
            }
            else if (CurrentTimer <= hardRemainingTime)
            {
                currentDifficulty = GameplayDifficultyEnum.Hard;
            }
        }
    }
}
