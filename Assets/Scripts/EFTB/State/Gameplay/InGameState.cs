using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class InGameState : BaseState
    {
        public InGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(FinishGameState), null);
            StateTransitionMap.Add(typeof(PauseMenuState), null);
        }
    }
}
