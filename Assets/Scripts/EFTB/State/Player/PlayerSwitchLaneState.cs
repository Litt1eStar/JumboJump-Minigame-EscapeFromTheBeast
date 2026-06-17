using JumboJump.EFTB.Model;
using JumboJumps.EFTB.State;
using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Utilities;

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

            DebugLogHelper.Log("Enter PlayerSwitchLaneState");
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }
    }
}
