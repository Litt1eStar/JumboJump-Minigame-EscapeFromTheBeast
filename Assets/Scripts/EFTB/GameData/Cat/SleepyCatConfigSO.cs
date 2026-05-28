using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using Assets.Scripts.EFTB.State.Cat.SleepyCat;
using Assets.Scripts.EFTB.UI;
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

        public float TimeTillAwake => timeTillAwake;
        public float TimeToAlert => timeToAlert;
        public float TimeToCatch => timeToCatch;

        public override ICatStateController BuildStateController(GICat sight, Transform transform, UICatStateLabel label)
        {
            SleepyCatStateController controller = new SleepyCatStateController(this, sight, label, transform);
            return controller;
        }
    }
}
