using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Constant.Gameplay;
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

        /// <summary>
        /// Invoke when gameplay timer <= 0
        /// </summary>
        public event Action EventGameplayTimerFinished;

        private GameplayTimeVisualizer visualizer;
        private GameplayDifficultyEnum currentDifficulty;
        public GameplayDifficultyEnum CurrentDifficulty => currentDifficulty;
        public float CurrentTimer { get; private set; } = 0f;
        private float limitPlayTime;

        public void Initialize()
        {
            visualizer = new GameplayTimeVisualizer();
            visualizer.Initialize(this);

            currentDifficulty = GameplayDifficultyEnum.Easy;
            limitPlayTime = ConstGameplay.LimitPlayTime;
            CurrentTimer = limitPlayTime;

            GameContext.Instance.Add(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if(CurrentTimer <= 0)
            {
                EventGameplayTimerFinished?.Invoke();
            }

            CurrentTimer -= deltaTime;
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
            float normalRemainingTime = limitPlayTime * (1 - ConstGameplay.LevelGenerator.MediumDifficultyTimePercentage);
            float hardRemainingTime = limitPlayTime * (1 - ConstGameplay.LevelGenerator.HardDifficultyTimePercentage);

            if (CurrentTimer <= normalRemainingTime && CurrentTimer > hardRemainingTime)
            {
                currentDifficulty = GameplayDifficultyEnum.Normal;
                DebugLogHelper.Log($"{currentDifficulty.ToString()}");
            }
            else if (CurrentTimer <= hardRemainingTime)
            {
                currentDifficulty = GameplayDifficultyEnum.Hard;
                DebugLogHelper.Log($"{currentDifficulty.ToString()}");
            }
        }
    }
}
