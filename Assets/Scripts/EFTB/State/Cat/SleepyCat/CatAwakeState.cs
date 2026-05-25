namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class CatAwakeState : BaseState
    {
        private readonly float TIME_TO_CATCH = 5f;
        private float countdownTimer = 0f;
        public CatAwakeState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(CatSleepState), null);
            StateTransitionMap.Add(typeof(CatCatchState), null);
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
            
            bool isPlayerInSight = true; // Replace with actual logic to check if player is in sight
            
            if(isPlayerInSight && countdownTimer < TIME_TO_CATCH)
            {
                // Transition to catch state
                StateController.ChangeState(typeof(CatCatchState));
            }

            if(countdownTimer > TIME_TO_CATCH)
            {
                //Transition to sleep state
                StateController.ChangeState(typeof(CatSleepState));
            }
        }
    }
}
