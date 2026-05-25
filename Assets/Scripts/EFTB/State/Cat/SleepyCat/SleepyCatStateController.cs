using Assets.Scripts.EFTB.Visualizer;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class SleepyCatStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(CatSleepState);
        public CatVisualizer visualizer { get; private set; }
        public SleepyCatStateController()
        {
            visualizer = new CatVisualizer();
            visualizer.Initialize();

            States = new Dictionary<Type, BaseState>()
            {
                {typeof(CatSleepState), new CatSleepState(this) },
                {typeof(CatAwakeState), new CatAwakeState(this) },
                {typeof(CatAlertState), new CatAlertState(this) },
                {typeof(CatCatchState), new CatCatchState(this) }
            };
        }
    }
}
