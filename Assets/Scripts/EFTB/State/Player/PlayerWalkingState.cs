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
        public PlayerWalkingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void UpdateLogic(float deltaTime)
        {
            if(Input.GetAxis("Horizontal") == 0)
            {
                StateController.ChangeState(typeof(PlayerIdleState));
            }
        }
    }
}
