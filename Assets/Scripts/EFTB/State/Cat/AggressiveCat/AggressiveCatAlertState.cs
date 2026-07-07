using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatAlertState : BaseState
    {
        private AggressiveCatStateController stateController;
        private float countdownTimer;

        public AggressiveCatAlertState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(AggressiveCatCatchState), null);
            StateTransitionMap.Add(typeof(AggressiveCatDisappearState), null);
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = stateController.Config.TimeToAlert;
            stateController.Visualizer.Subscribe();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            stateController.Visualizer.Unsubscribe();
        }

        public override void UpdateLogic(float deltaTime)
        {
            countdownTimer -= deltaTime;
            StateController.InvokeEventTimerChanged(countdownTimer);

            if (countdownTimer <= 0f)
            {
                if (stateController.Visualizer.IsTargetInSight())
                {
                    stateController.ChangeState(typeof(AggressiveCatCatchState));
                }
                else
                {
                    stateController.ChangeState(typeof(AggressiveCatDisappearState));
                }
            }
        }
    }
}
