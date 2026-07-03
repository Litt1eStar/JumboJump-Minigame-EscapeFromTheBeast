using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.State.Cat.AggressiveCat;
using JumboJumps.EFTB.UI;
using UnityEngine;

namespace JumboJumps.EFTB.GameData.Cat
{
    [CreateAssetMenu(fileName = "AggressiveCatConfigSO", menuName = "Scriptable Objects/AggressiveCatConfigSO")]
    public class AggressiveCatConfigSO : BaseCatConfigSO
    {
        [SerializeField]
        private float timeToAppear = 1f;

        [SerializeField]
        private float timeToAwake = 5f;

        [SerializeField]
        private float timeToAlert = 3f;

        [SerializeField]
        private float timeToCatch = 0.5f;

        [SerializeField]
        private float timeToDissappear = 1f;

        [SerializeField]
        private float slideDistance = 5f;

        public float TimeToAppear => timeToAppear;
        public float TimeToAwake => timeToAwake;
        public float TimeToAlert => timeToAlert;
        public float TimeToCatch => timeToCatch;
        public float TimeToDissappear => timeToDissappear;
        public float SlideDistance => slideDistance;

        public override ICatStateController BuildStateController(GICat giCat, Transform transform, UICatStateLabel label)
        {
            return new AggressiveCatStateController(this, giCat, label, transform);
        }
    }
}
