using Assets.Scripts.EFTB.GameData.Cat;
using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using Assets.Scripts.EFTB.UI;
using Assets.Scripts.EFTB.Visualizer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
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
