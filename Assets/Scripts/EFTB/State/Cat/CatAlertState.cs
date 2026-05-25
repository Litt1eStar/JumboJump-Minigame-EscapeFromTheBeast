using Assets.Scripts.EFTB.State.Cat.SleepyCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EFTB.State.Cat
{
    public class CatAlertState : BaseState
    {
        private readonly float TIME_TO_CATCH = 5f;
        private float countdownTimer = 0f;
        private SleepyCatStateController stateController;
        public CatAlertState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(CatCatchState), null);
            StateTransitionMap.Add(typeof(CatSleepState), null);

            this.stateController = (SleepyCatStateController)stateController;
        }
        public override void OnEnterState()
        {
            base.OnEnterState();
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

            if (isPlayerInSight && countdownTimer < TIME_TO_CATCH)
            {
                // Transition to catch state
                StateController.ChangeState(typeof(CatCatchState));
                return;
            }

            if (countdownTimer > TIME_TO_CATCH)
            {
                //Transition to sleep state
                StateController.ChangeState(typeof(CatSleepState));
                return;
            }
        }
    }
}
