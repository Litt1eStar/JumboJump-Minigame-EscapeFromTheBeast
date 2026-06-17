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
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            DebugLogHelper.Log("Enter PlayerMovingState");

            isMovingForward = true;
            Subscribe();
        }

        public override void OnExitState()
        {
            Unsubscribe();
            base.OnExitState();
        }

        public void Subscribe()
        {
            playerStateController.Input2DManager.EventHoldEnded += OnStopMoving;
        }

        public void Unsubscribe()
        {
            playerStateController.Input2DManager.EventHoldEnded -= OnStopMoving;
        }

        public override void UpdateLogic(float deltaTime)
        {
            if (isMovingForward == false) return;

            DebugLogHelper.Log("Moving Forward");
        }

        public void OnStopMoving()
        {
            isMovingForward = false;
            playerStateController.ChangeState(typeof(PlayerIdleState));
        }
    }
}
