using Assets.Scripts.EFTB.Manager;
using Assets.Scripts.EFTB.Visualizer;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class SleepyCatStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(CatSleepState);
        public CatVisualizer visualizer { get; private set; }
        public SleepyCatStateController(SleepyCatBehaviourConfig config)
        {
            visualizer = new CatVisualizer();
            visualizer.Initialize();

            States = new Dictionary<Type, BaseState>()
            {
                {typeof(CatSleepState), new CatSleepState(this, config.TIME_TILL_AWAKE) },
                {typeof(CatAwakeState), new CatAwakeState(this, config.TIME_TO_ALERT) },
                {typeof(CatAlertState), new CatAlertState(this, config.TIME_TO_CATCH) },
                {typeof(CatCatchState), new CatCatchState(this) }
            };
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            visualizer.UpdateLogic(deltaTime);
        }
    }
}
