using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerIdleState : BaseState
    {
        private PlayerStateController playerStateController;
        public PlayerIdleState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;
            StateTransitionMap.Add(typeof(PlayerWalkingState), null);
        }
        public override void UpdateLogic(float deltaTime)
        {
            float xInput = playerStateController.input2DManager.xInput;
            if (xInput > 0 || xInput < 0)
            {
                StateController.ChangeState(typeof(PlayerWalkingState));
            }
            playerStateController.visualizer.Idle();
        }

    }
}
