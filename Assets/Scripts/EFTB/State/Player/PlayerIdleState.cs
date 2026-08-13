using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;
using JumboJumps.EFTB.Visualizer;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerIdleState : BaseState
    {
        private PlayerStateController playerStateController => (PlayerStateController)StateController;
        private PlayerVisualizer playerVisualizer => playerStateController.Visualizer;
        private float idleTimer;

        public PlayerIdleState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerMovingState), null);            
            StateTransitionMap.Add(typeof(PlayerSwitchLaneState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            playerVisualizer?.SetMovingAnimation(false);
            idleTimer = playerStateController.IdleTimer;
            Subscribe();
        }

        public override void OnExitState()
        {
            Unsubscribe();

            playerStateController.IdleTimer = idleTimer;

            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            IdleTimerTracking(deltaTime);
        }

        private void IdleTimerTracking(float deltaTime)
        {
            idleTimer += deltaTime;
            playerStateController.IdleTimer = idleTimer;

            if (idleTimer >= ConstGameplay.Cat.AggressiveCat.IDLE_LIMIT)
            {
                idleTimer = 0f;
                playerStateController.ResetIdleTimer();
                playerStateController.InvokeIdleLimitExceeded();
            }
        }

        public void Subscribe()
        {
            if (playerStateController.Input2DManager == null) return;

            playerStateController.Input2DManager.EventTap += OnTap;
            playerStateController.Input2DManager.EventSwipe += OnSwipe;
        }

        public void Unsubscribe() 
        {
            if (playerStateController.Input2DManager == null) return;
            
            playerStateController.Input2DManager.EventTap -= OnTap;
            playerStateController.Input2DManager.EventSwipe -= OnSwipe;
        }

        public void OnTap()
        {
            float targetY = playerStateController.Visualizer.PlayerPosition.y + ConstGameplay.Player.STEP_DISTANCE_Y;

            if (playerStateController.IsTargetCellBlocked(playerStateController.CurrentLaneIndex, targetY))
            {
                // Furniture in next cell of player so moving forward cannot execute
                return;
            }

            playerStateController.IsStepUpRequested = true;

            StateController.ChangeState(typeof(PlayerMovingState));
        }

        public void OnSwipe(SwipeDirectionEnum swipeDirection)
        {
            int targetLane = playerStateController.CurrentLaneIndex;

            if (swipeDirection == SwipeDirectionEnum.Left)
            {
                targetLane = Mathf.Max(0, targetLane - 1);
            }
            else if (swipeDirection == SwipeDirectionEnum.Right)
            {
                targetLane = Mathf.Min(playerStateController.LaneXPositions.Length - 1, targetLane + 1);
            }

            float currentY = playerStateController.Visualizer.PlayerPosition.y;

            if (playerStateController.IsTargetCellBlocked(targetLane, currentY))
            {
                // Target lane cell is blocked by furniture -> lane switch cannot execute!
                return;
            }

            playerStateController.IsStepUpRequested = false;
            playerStateController.LastSwipeDirection = swipeDirection;
            
            StateController.ChangeState(typeof(PlayerSwitchLaneState));
        }
    }
}
