using JumboJumps.EFTB.State;
using JumboJumps.EFTB.State.Player;

namespace JumboJump.Assets.Scripts.EFTB.State.Player
{
    public class PlayerSwitchLaneState : BaseState
    {
        private PlayerStateController playerStateController;
        public PlayerSwitchLaneState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;

            StateTransitionMap.Add(typeof(PlayerIdleState), null);
            StateTransitionMap.Add(typeof(PlayerMovingState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }
    }
}
