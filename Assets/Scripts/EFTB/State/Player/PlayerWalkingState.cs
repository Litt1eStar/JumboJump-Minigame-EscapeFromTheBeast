using Assets.Scripts.EFTB.Manager;
using Assets.Scripts.EFTB.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EFTB.State.Player
{
    public class PlayerWalkingState : BaseState
    {
        private PlayerStateController playerStateController;
        public PlayerWalkingState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;
            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void UpdateLogic(float deltaTime)
        {
            float xInput = playerStateController.input2DManager.xInput; 
            if(xInput == 0)
            {
                StateController.ChangeState(typeof(PlayerIdleState));
            }
            playerStateController.visualizer.Move(xInput);
        }
    }
}
