namespace JumboJumps.EFTB.State.Cat.SleepyCat
{
    public class CatSleepState : BaseState
    {
        private readonly float TIME_TILL_AWAKE = 5f;
        private float countdownTimer = 0f;
        public CatSleepState(
            BaseStateController stateController,
            float timeTillAwake
            ): base(stateController)
        {
            StateTransitionMap.Add(typeof(CatAwakeState), null);    
            TIME_TILL_AWAKE = timeTillAwake;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = TIME_TILL_AWAKE;
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            countdownTimer -= deltaTime;
            StateController.InvokeEventTimerChanged(countdownTimer);
            //DebugLogHelper.LogWarning($"[{GetType().Name}] Countdown Timer: {countdownTimer}");
            if (countdownTimer <= 0)
            {
                // Transition to awake state
                StateController.ChangeState(typeof(CatAwakeState));
                return;
            }
        }
    }
}
