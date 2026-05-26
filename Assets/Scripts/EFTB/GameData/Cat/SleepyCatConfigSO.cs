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
        public float TIME_TILL_AWAKE;
        public float TIME_TO_ALERT;
        public float TIME_TO_CATCH;
        public override ICatStateController BuildStateController(GICat sight, Transform transform, UICatStateLabel label)
        {
            SleepyCatStateController controller = new SleepyCatStateController(this, sight, label, transform);
            return controller;
        }
    }
}
