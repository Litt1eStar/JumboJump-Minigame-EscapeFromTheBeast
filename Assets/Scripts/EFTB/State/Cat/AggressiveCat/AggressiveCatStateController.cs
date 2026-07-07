using JumboJumps.EFTB.GameData.Cat;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.UI;
using JumboJumps.EFTB.Visualizer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatStateController : BaseStateController, ICatStateController
    {
        protected override Type DefaultTypeState => typeof(AggressiveCatAppearState);
        public CatVisualizer Visualizer { get; private set; }
        public AggressiveCatConfigSO Config { get; private set; }
        public GICat GiCat { get; private set; }
        public Transform Target { get; private set; }

        public AggressiveCatStateController(
            AggressiveCatConfigSO config,
            GICat giCat,
            UICatStateLabel label,
            Transform target)
        {
            Config = config;
            GiCat = giCat;
            Target = target;

            Visualizer = new CatVisualizer();
            Visualizer.Initialize(giCat, label, this);

            States = new Dictionary<Type, BaseState>()
            {
                { typeof(AggressiveCatAppearState), new AggressiveCatAppearState(this) },
                { typeof(AggressiveCatAwakeState), new AggressiveCatAwakeState(this) },
                { typeof(AggressiveCatAlertState), new AggressiveCatAlertState(this) },
                { typeof(AggressiveCatCatchState), new AggressiveCatCatchState(this) },
                { typeof(AggressiveCatDisappearState), new AggressiveCatDisappearState(this) }
            };
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            Visualizer.UpdateLogic(deltaTime);
        }
    }
}
