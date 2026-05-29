using JumboJumps.EFTB.State.Cat.SleepyCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumboJumps.EFTB.State.Cat
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
            countdownTimer = TIME_TO_CATCH;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            stateController.visualizer.Unsubscribe();
        }

        public override void UpdateLogic(float deltaTime)
        {
            if(countdownTimer > 0f)
            {
                countdownTimer -= deltaTime;
            }

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
