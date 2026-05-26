using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using System;
using UnityEngine;

namespace Assets.Scripts.EFTB.GameData.Cat
{
    public class SleepyCatConfigSO : BaseCatConfigSO
    {
        [SerializeField]
        private float TIME_TILL_AWAKE;
        [SerializeField]
        private float TIME_TILL_ALERT;
        [SerializeField]
        private float TIME_TILL_CATCH;
        public override ICatStateContrller BuildStateController(GICatSight sight, Transform transform)
        {
            throw new NotImplementedException();
        }
    }
}
