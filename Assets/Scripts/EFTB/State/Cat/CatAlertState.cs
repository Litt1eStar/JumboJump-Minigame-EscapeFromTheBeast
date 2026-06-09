using JumboJumps.EFTB.State.Cat.SleepyCat;

namespace JumboJumps.EFTB.State.Cat
{
    public class CatAlertState : BaseState
    {
        private readonly float TIME_TO_CATCH = 5f;
        private float countdownTimer = 0f;
        private SleepyCatStateController stateController;

        public CatAlertState(BaseStateController stateController,
                             float timeToCatch) : base(stateController)
        {
            StateTransitionMap.Add(typeof(CatCatchState), null);
            StateTransitionMap.Add(typeof(CatSleepState), null);

            this.stateController = (SleepyCatStateController)stateController;
            TIME_TO_CATCH = timeToCatch;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            stateController.visualizer.Subscribe();
            countdownTimer = TIME_TO_CATCH;
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

            if(countdownTimer <= 0f)
            {
                if (stateController.visualizer.IsTargetInSght())
                {
                    stateController.ChangeState(typeof(CatCatchState));
                }
                else
                {
                    stateController.ChangeState(typeof(CatSleepState));
                }
            }
        }
    }
}
