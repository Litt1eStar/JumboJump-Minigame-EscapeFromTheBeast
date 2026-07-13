using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using System.Collections;
using UnityEngine;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerSwitchLaneState : BaseState
    {
        private PlayerStateController playerStateController => (PlayerStateController)StateController;

        private CoroutineHelper coroutineHelper;
        private Coroutine switchLaneCoroutine;

        public PlayerSwitchLaneState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            int targetLane = playerStateController.CurrentLaneIndex;
            if (playerStateController.LastSwipeDirection == SwipeDirectionEnum.Left)
            {
                targetLane = Mathf.Max(0, targetLane - 1);
            }
            else if (playerStateController.LastSwipeDirection == SwipeDirectionEnum.Right)
            {
                targetLane = Mathf.Min(playerStateController.LANE_X_POSITIONS.Length - 1, targetLane + 1);
            }

            if (targetLane == playerStateController.CurrentLaneIndex)
            {
                OnFinishSwitchingLane();
                return;
            }

            float targetX = playerStateController.LANE_X_POSITIONS[targetLane];
            playerStateController.CurrentLaneIndex = targetLane;

            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            switchLaneCoroutine = coroutineHelper.Restart(switchLaneCoroutine, SmoothSwitchLane(targetX));
        }

        private IEnumerator SmoothSwitchLane(float target)
        {
            float startX = playerStateController.Visualizer.PlayerPosition.x;
            float elapsed = 0f;
            float duration = 0.2f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float currentX = Mathf.Lerp(startX, target, t);
                playerStateController.Visualizer.SetXPosition(currentX);
                yield return null;
            }

            playerStateController.Visualizer.SetXPosition(target);
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
            StateController.ChangeState(typeof(PlayerIdleState));
        }
    }
}
