using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.State.Cat.SleepyCat;
using JumboJumps.EFTB.UI;
using UnityEngine;

namespace JumboJumps.EFTB.GameData.Cat
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
        public override ICatStateController BuildStateController(GICat giCat, Transform transform, UICatStateLabel label)
        {
            SleepyCatStateController controller = new SleepyCatStateController(this, giCat, label, transform);
            return controller;
        }
    }
}
