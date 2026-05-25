namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class CatAwakeState : BaseState
    {
        private readonly float TIME_TO_ALERT = 5f;
        private float countdownTimer = 0f;

        private SleepyCatStateController stateController;
        public CatAwakeState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(CatSleepState), null);
            StateTransitionMap.Add(typeof(CatAlertState), null);
        
            this.stateController = (SleepyCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = 0f;
            stateController.visualizer.Subscribe();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            stateController.visualizer.Unsubscribe();
        }

        public override void UpdateLogic(float deltaTime)
        {
            countdownTimer += deltaTime;
            
            bool isPlayerInSight = stateController.visualizer.IsTargetInSght(); // Replace with actual logic to check if player is in sight
            
            if(isPlayerInSight && countdownTimer < TIME_TO_ALERT)
            {
                // Transition to alert state
                StateController.ChangeState(typeof(CatAlertState));
                return;
            }

            if(countdownTimer > TIME_TO_ALERT)
            {
                //Transition to sleep state
                StateController.ChangeState(typeof(CatSleepState));
                return;
            }
        }
    }
}
