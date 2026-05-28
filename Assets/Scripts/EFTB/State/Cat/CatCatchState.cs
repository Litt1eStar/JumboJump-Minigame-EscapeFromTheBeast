using JumboJumps.EFTB.State.Cat.SleepyCat;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Cat
{
    public class CatCatchState : BaseState
    {
        private SleepyCatStateController stateController;
        public CatCatchState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (SleepyCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            DebugLogHelper.Log("PLAYER GOT CATCH");
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        } 
    }
}
