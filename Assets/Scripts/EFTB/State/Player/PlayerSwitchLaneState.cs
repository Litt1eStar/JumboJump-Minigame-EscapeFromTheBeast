using JumboJump.EFTB.Model;
using JumboJumps.EFTB.State;
using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Utilities;
using System.Collections;
using UnityEngine;

namespace JumboJump.Assets.Scripts.EFTB.State.Player
{
    public class PlayerSwitchLaneState : BaseState
    {
        private PlayerStateController playerStateController;

        public PlayerSwitchLaneState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;

            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            
            DebugLogHelper.Log("Enter PlayerSwitchLaneState");

            // Perform Switch Lane Logic then Switch back to Player Idle State
            GameContext.Instance.Get<CoroutineHelper>().Play(SimulatedSwitchLane());
        }

        private IEnumerator SimulatedSwitchLane()
        {
            yield return new WaitForSeconds(3);

            OnFinishSwitchingLane();
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public void OnFinishSwitchingLane()
        {
            // Call this method when Finish Switch Lane routine
            playerStateController.ChangeState(typeof(PlayerIdleState));
        }
    }
}
