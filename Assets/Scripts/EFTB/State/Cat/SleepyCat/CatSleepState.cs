using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class CatSleepState : BaseState
    {
        private readonly float TIME_TILL_AWAKE = 5f;
        private float countdownTimer = 0f;
        public CatSleepState(
            BaseStateController stateController,
            float TIME_TILL_AWAKE
            ): base(stateController)
        {
            StateTransitionMap.Add(typeof(CatAwakeState), null);    
            this.TIME_TILL_AWAKE = TIME_TILL_AWAKE;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = 0f;
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            countdownTimer += deltaTime;
            //DebugLogHelper.LogWarning($"[{GetType().Name}] Countdown Timer: {countdownTimer}");
            if (countdownTimer > TIME_TILL_AWAKE)
            {
                // Transition to awake state
                countdownTimer = 0f;
                StateController.ChangeState(typeof(CatAwakeState));
                return;
            }
        }
    }
}
