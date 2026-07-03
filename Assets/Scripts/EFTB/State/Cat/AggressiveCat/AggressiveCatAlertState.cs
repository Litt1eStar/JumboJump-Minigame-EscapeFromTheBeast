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
            StateTransitionMap.Add(typeof(AggressiveCatDissappearState), null);
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = stateController.Config.TimeToAlert;
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

            if (countdownTimer <= 0f)
            {
                if (stateController.visualizer.IsTargetInSght())
                {
                    stateController.ChangeState(typeof(AggressiveCatCatchState));
                }
                else
                {
                    stateController.ChangeState(typeof(AggressiveCatDissappearState));
                }
            }
        }
    }
}
