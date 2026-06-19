using JumboJumps.EFTB.State;
using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Utilities;
using System.Collections;
using UnityEngine;

namespace JumboJump.EFTB.State.Player
{
    public class PlayerSwitchLaneState : BaseState
    {
        public PlayerSwitchLaneState(BaseStateController stateController) : base(stateController)
        {
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
            StateController.ChangeState(typeof(PlayerIdleState));
        }
    }
}
