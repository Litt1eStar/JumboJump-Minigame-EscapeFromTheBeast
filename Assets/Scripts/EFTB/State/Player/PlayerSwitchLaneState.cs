using JumboJumps.EFTB.Constant.Gameplay;
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
                targetLane = Mathf.Min(playerStateController.LaneXPositions.Length - 1, targetLane + 1);
            }

            Vector3 startPos = playerStateController.Visualizer.PlayerPosition;
            float stepY = playerStateController.IsStepUpRequested ? ConstGameplay.Obstacle.Furniture.Cell_Height : 0f;
            float targetY = startPos.y + stepY;

            if (playerStateController.IsTargetCellBlocked(targetLane, targetY))
            {
                StateController.ChangeState(typeof(PlayerIdleState));
                return;
            }

            float targetX = playerStateController.LaneXPositions[targetLane];
            playerStateController.CurrentLaneIndex = targetLane;
            Vector3 targetPos = new Vector3(targetX, targetY, startPos.z);

            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            switchLaneCoroutine = coroutineHelper.Restart(switchLaneCoroutine, SmoothStepLane(startPos, targetPos));
        }

        private IEnumerator SmoothStepLane(Vector3 startPos, Vector3 targetPos)
        {
            float elapsed = 0f;
            float duration = ConstGameplay.Player.Step_Duration;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
                playerStateController.Visualizer.SetPosition(currentPos);
                yield return null;
            }

            playerStateController.Visualizer.SetPosition(targetPos);
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

        private void OnFinishSwitchingLane()
        {
            StateController.ChangeState(typeof(PlayerIdleState));
        }
    }
}
