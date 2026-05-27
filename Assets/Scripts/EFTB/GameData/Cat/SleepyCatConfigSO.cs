using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using Assets.Scripts.EFTB.State.Cat.SleepyCat;
using Assets.Scripts.EFTB.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.EFTB.GameData.Cat
{
    [CreateAssetMenu(fileName = "SleepyCatConfigSO", menuName = "Scriptable Objects/SleepyCatConfigSO")]
    public class SleepyCatConfigSO : BaseCatConfigSO
    {
        [SerializeField]
        private float timeTillAwake;
        [SerializeField]
        private float timeToAlert;
        [SerializeField]
        private float timeToCatch;

        public float TIME_TILL_AWAKE => timeTillAwake;
        public float TIME_TO_ALERT => timeToAlert;
        public float TIME_TO_CATCH => timeToCatch;

        public override ICatStateController BuildStateController(GICat sight, Transform transform, UICatStateLabel label)
        {
            SleepyCatStateController controller = new SleepyCatStateController(this, sight, label, transform);
            return controller;
        }
    }
}
