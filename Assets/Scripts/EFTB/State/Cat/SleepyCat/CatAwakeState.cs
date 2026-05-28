namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class CatAwakeState : BaseState
    {
        private readonly float TIME_TO_ALERT = 5f;
        private float countdownTimer = 0f;

        private SleepyCatStateController stateController;
        public CatAwakeState(
            BaseStateController stateController,
            float timeToAlert
            ) : base(stateController)
        {
            StateTransitionMap.Add(typeof(CatSleepState), null);
            StateTransitionMap.Add(typeof(CatAlertState), null);
            
            this.stateController = (SleepyCatStateController)stateController;
            TIME_TO_ALERT = timeToAlert;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = TIME_TO_ALERT;
            stateController.visualizer.Subscribe();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            stateController.visualizer.Unsubscribe();
        }

        public override void UpdateLogic(float deltaTime)
        {
            countdownTimer -= deltaTime;
            StateController.InvokeEventTimerChanged(countdownTimer);
            bool isPlayerInSight = stateController.visualizer.IsTargetInSght(); // Replace with actual logic to check if player is in sight
            
            if(isPlayerInSight && countdownTimer > 0)
            {
                // Transition to alert state
                StateController.ChangeState(typeof(CatAlertState));
                return;
            }

            if(countdownTimer <= 0)
            {
                //Transition to sleep state
                StateController.ChangeState(typeof(CatSleepState));
                return;
            }
        }
    }
}
