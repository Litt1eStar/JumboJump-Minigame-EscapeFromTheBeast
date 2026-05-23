using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EFTB.State.Player
{
    public class PlayerIdleState : BaseState
    {
        public PlayerIdleState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerWalkingState), null);
        }
        public override void UpdateLogic(float deltaTime)
        {
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
            {
                StateController.ChangeState(typeof(PlayerWalkingState));
            }
        }

    }
}
