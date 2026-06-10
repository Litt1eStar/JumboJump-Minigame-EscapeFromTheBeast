using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class InGameState : BaseState
    {
        private Input2DManager input2DManager;

        public InGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(FinishGameState), null);
            StateTransitionMap.Add(typeof(PauseMenuState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            input2DManager = GameContext.Instance.Get<Input2DManager>();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            input2DManager = null;
        }
    }
}
