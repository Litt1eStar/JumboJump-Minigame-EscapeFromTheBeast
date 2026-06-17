using JumboJump.Assets.Scripts.EFTB.State.Player;
using JumboJump.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerMovingState : BaseState
    {
        private PlayerStateController playerStateController;
        private bool isMovingForward;
        public PlayerMovingState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;

            StateTransitionMap.Add(typeof(PlayerIdleState), null);
            StateTransitionMap.Add(typeof(PlayerSwitchLaneState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            DebugLogHelper.Log("Enter PlayerMovingState");

            isMovingForward = true;
        }

        public override void OnExitState()
        {

            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            if (isMovingForward == false) return;

            DebugLogHelper.Log("Moving Forward");
        }
    }
}
