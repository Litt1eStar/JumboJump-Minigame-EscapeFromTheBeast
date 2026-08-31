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
        private float timeToDisappear = 1f;

        [SerializeField]
        private float slideDistance = 5f;

        [SerializeField]
        private float timeToSmash = 0.3f;

        [SerializeField]
        private float smashStayDuration = 3f;

        [SerializeField]
        private float smashThresholdY = 1.5f;

        [SerializeField]
        private AnimationCurve smashMovementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField]
        private AnimationCurve smashRotationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField]
        private float smashMoveInPercentage = 0.4f;

        [SerializeField]
        private float smashWaitPercentage = 0.7f;

        [SerializeField]
        private float smashCollisionProgressThreshold = 0.8f;

        [SerializeField]
        private float pounceWarningDuration = 1.5f;

        [SerializeField]
        private float pounceWarningShakeSpeed = 25f;

        [SerializeField]
        private float pounceWarningMaxZRotation = 12f;

        public float TimeToAppear => timeToAppear;
        public float TimeToAwake => timeToAwake;
        public float TimeToAlert => timeToAlert;
        public float TimeToCatch => timeToCatch;
        public float TimeToDisappear => timeToDisappear;
        public float SlideDistance => slideDistance;
        public float TimeToSmash => timeToSmash;
        public float SmashStayDuration => smashStayDuration;
        public float SmashThresholdY => smashThresholdY;
        public AnimationCurve SmashMovementCurve => smashMovementCurve;
        public AnimationCurve SmashRotationCurve => smashRotationCurve;
        public float SmashMoveInPercentage => smashMoveInPercentage;
        public float SmashWaitPercentage => smashWaitPercentage;
        public float SmashCollisionProgressThreshold => smashCollisionProgressThreshold;
        public float PounceWarningDuration => pounceWarningDuration;
        public float PounceWarningShakeSpeed => pounceWarningShakeSpeed;
        public float PounceWarningMaxZRotation => pounceWarningMaxZRotation;

        public override ICatStateController BuildStateController(GICat giCat, Transform transform, UICatStateLabel label)
        {
            return new AggressiveCatStateController(this, giCat, label, transform);
        }
    }
}
