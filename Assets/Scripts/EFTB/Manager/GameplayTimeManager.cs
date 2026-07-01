using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Manager
{
    public class GameplayTimeManager
    {
        public float CurrentTimer { get; private set; } = 0f;
        private float limitPlayTime;
        public void Initialize()
        {
            limitPlayTime = ConstGameplay.LimitPlayTime;
            CurrentTimer = limitPlayTime;

            GameContext.Instance.Add(this);
        }

        public void UpdateLogic(float deltaTime)
        {

        }

        public void Dispose()
        {
            CurrentTimer = 0f;

            GameContext.Instance.Remove(this);
        }
    }
}
