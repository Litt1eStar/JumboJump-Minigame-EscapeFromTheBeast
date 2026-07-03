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
            StateTransitionMap.Add(typeof(AggressiveCatDissappearState), null);
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            countdownTimer = stateController.Config.TimeToAwake;
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

            bool isPlayerInSight = stateController.visualizer.IsTargetInSght();

            if (isPlayerInSight && countdownTimer > 0)
            {
                StateController.ChangeState(typeof(AggressiveCatAlertState));
                return;
            }

            if (countdownTimer <= 0)
            {
                StateController.ChangeState(typeof(AggressiveCatDissappearState));
                return;
            }
        }
    }
}
