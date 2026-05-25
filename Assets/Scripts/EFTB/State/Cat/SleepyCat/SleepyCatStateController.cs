using Assets.Scripts.EFTB.Utilities;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class SleepyCatStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(CatSleepState);
        public SleepyCatStateController()
        {
            States = new Dictionary<Type, BaseState>()
            {
                {typeof(CatSleepState), new CatSleepState(this) },
                {typeof(CatAwakeState), new CatAwakeState(this) },
                {typeof(CatAlertState), new CatAlertState(this) },
            };
        }
    }
}
