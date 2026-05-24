using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EFTB.State.Player
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
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
            {
                StateController.ChangeState(typeof(PlayerWalkingState));
            }
            playerStateController.visualizer.Idle();
        }

    }
}
