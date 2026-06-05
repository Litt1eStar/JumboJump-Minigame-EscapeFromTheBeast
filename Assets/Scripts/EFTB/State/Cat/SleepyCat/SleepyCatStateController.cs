using JumboJumps.EFTB.GameData.Cat;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.UI;
using JumboJumps.EFTB.Visualizer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.SleepyCat
{
    public class SleepyCatStateController : BaseStateController, ICatStateController
    {
        protected override Type DefaultTypeState => typeof(CatSleepState);
        public CatVisualizer visualizer { get; private set; }

        public SleepyCatStateController(
            SleepyCatConfigSO config,
            GICat giCat,
            UICatStateLabel label,
            Transform target)
        {
            visualizer = new CatVisualizer();
            visualizer.Initialize(giCat, label, this);

            States = new Dictionary<Type, BaseState>()
            {
                {typeof(CatSleepState), new CatSleepState(this, config.TimeTillAwake) },
                {typeof(CatAwakeState), new CatAwakeState(this, config.TimeToAlert) },
                {typeof(CatAlertState), new CatAlertState(this, config.TimeToCatch) },
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
