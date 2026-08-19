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
        private GameplayController gameplayController;
        private GameplayStateManager gameplayStateManager;
        private GameplayDifficultyEnum currentDifficulty;
        public GameplayDifficultyEnum CurrentDifficulty => currentDifficulty;
        public float CurrentTimer { get; private set; } = 0f;
        private float limitPlayTime;
        private float mediumDifficultyThreshold;
        private float hardDifficultyThreshold;

        public void Initialize()
        {
            levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
            gameplayController = GameContext.Instance.Get<GameplayController>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            currentDifficulty = GameplayDifficultyEnum.Easy;
            limitPlayTime = ConstGameplay.LIMIT_PLAY_TIME;
            CurrentTimer = 0f;

            if (levelGeneratorManager != null && levelGeneratorManager.Config != null)
            {
                mediumDifficultyThreshold = limitPlayTime * levelGeneratorManager.Config.MediumDifficultyTimePercentage;
                hardDifficultyThreshold = limitPlayTime * levelGeneratorManager.Config.HardDifficultyTimePercentage;
            }

            GameContext.Instance.Add(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            CurrentTimer += deltaTime;
            HandleDifficultyAdjustment();
            EventGameplayTimerChanged?.Invoke(CurrentTimer);
        }

        public void Dispose()
        {
            CurrentTimer = 0f;

            GameContext.Instance.Remove(this);
        }

        private void HandleDifficultyAdjustment()
        {
            if (CurrentTimer >= hardDifficultyThreshold)
            {
                currentDifficulty = GameplayDifficultyEnum.Hard;
            }
            else if (CurrentTimer >= mediumDifficultyThreshold)
            {
                currentDifficulty = GameplayDifficultyEnum.Normal;
            }
            else
            {
                currentDifficulty = GameplayDifficultyEnum.Easy;
            }
        }
    }
}
