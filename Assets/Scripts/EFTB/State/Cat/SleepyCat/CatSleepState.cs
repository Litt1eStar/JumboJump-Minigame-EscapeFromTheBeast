using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class CatSleepState : BaseState
    {
        private readonly float TIME_TILL_AWAKE = 5f;
        private float countdownTimer = 0f;
        private bool isAwake = false; 
        public CatSleepState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(CatAwakeState), null);    

            countdownTimer = 0f;
            isAwake = false;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            isAwake = true;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            isAwake = true;
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
            }
        }
    }
}
