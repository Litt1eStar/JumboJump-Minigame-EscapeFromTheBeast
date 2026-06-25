using JumboJumps.EFTB.State;
using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Utilities;
using System.Collections;
using UnityEngine;

namespace JumboJump.EFTB.State.Player
{
    public class PlayerSwitchLaneState : BaseState
    {
        private CoroutineHelper coroutineHelper;
        private Coroutine switchLaneCoroutine;

        public PlayerSwitchLaneState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            switchLaneCoroutine = coroutineHelper.Restart(switchLaneCoroutine, SimulatedSwitchLane());
        }

        private IEnumerator SimulatedSwitchLane()
        {
            #if UNITY_EDITOR
                yield return new WaitForSeconds(3); //This is for testing purpose. I will replace it with real logic later
            #endif

            OnFinishSwitchingLane();
        }


        public override void OnExitState()
        {
            if (coroutineHelper != null) 
            {
                coroutineHelper.Stop(switchLaneCoroutine);
                coroutineHelper = null;
                switchLaneCoroutine = null;
            }

            base.OnExitState();
        }

        public void OnFinishSwitchingLane()
        {
            // Call this method when Finish Switch Lane routine
            StateController.ChangeState(typeof(PlayerIdleState));
        }
    }
}
