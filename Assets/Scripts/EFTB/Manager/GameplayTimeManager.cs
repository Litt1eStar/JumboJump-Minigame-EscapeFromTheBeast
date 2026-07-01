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
        public float CurrentTimer { get; private set; } = 0f;
        private float limitPlayTime;

        public void Initialize()
        {
            visualizer = new GameplayTimeVisualizer();
            visualizer.Initialize(this);

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
            EventGameplayTimerChanged?.Invoke(CurrentTimer);
        }

        public void Dispose()
        {
            CurrentTimer = 0f;

            GameContext.Instance.Remove(this);
        }
    }
}
