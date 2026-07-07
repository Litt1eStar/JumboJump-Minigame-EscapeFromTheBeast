using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatAwakeState : BaseState
    {
        private AggressiveCatStateController stateController;
        private float countdownTimer;

        public AggressiveCatAwakeState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(AggressiveCatAlertState), null);
            StateTransitionMap.Add(typeof(AggressiveCatDisappearState), null);
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = stateController.Config.TimeToAwake;
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

            bool isPlayerInSight = stateController.Visualizer.IsTargetInSight();

            if (isPlayerInSight && countdownTimer > 0)
            {
                StateController.ChangeState(typeof(AggressiveCatAlertState));
                return;
            }

            if (countdownTimer <= 0)
            {
                StateController.ChangeState(typeof(AggressiveCatDisappearState));
                return;
            }
        }
    }
}
