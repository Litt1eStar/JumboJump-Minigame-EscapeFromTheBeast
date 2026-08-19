using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerIdleState : BaseState
    {
        private PlayerStateController playerStateController => (PlayerStateController)StateController;
        private float idleTimer;

        public PlayerIdleState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerMovingState), null);            
            StateTransitionMap.Add(typeof(PlayerSwitchLaneState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            idleTimer = 0f;
            Subscribe();
        }

        public override void OnExitState()
        {
            Unsubscribe();

            idleTimer = 0f;

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
            if (idleTimer >= ConstGameplay.Cat.AggressiveCat.IDLE_LIMIT)
            {
                idleTimer = 0f;
                playerStateController.InvokeIdleLimitExceeded();
            }
        }

        public void Subscribe()
        {
            if (playerStateController.Input2DManager == null) return;

            playerStateController.Input2DManager.EventTap += OnTap;
            playerStateController.Input2DManager.EventSwipe += OnSwipe;
            playerStateController.Input2DManager.EventCombinedStep += OnCombinedStep;
        }

        public void Unsubscribe() 
        {
            if (playerStateController.Input2DManager == null) return;
            
            playerStateController.Input2DManager.EventTap -= OnTap;
            playerStateController.Input2DManager.EventSwipe -= OnSwipe;
            playerStateController.Input2DManager.EventCombinedStep -= OnCombinedStep;
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
                targetLane = Mathf.Min(playerStateController.LANE_X_POSITIONS.Length - 1, targetLane + 1);
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

        public void OnCombinedStep(SwipeDirectionEnum swipeDirection)
        {
            int targetLane = playerStateController.CurrentLaneIndex;

            if (swipeDirection == SwipeDirectionEnum.Left)
            {
                targetLane = Mathf.Max(0, targetLane - 1);
            }
            else if (swipeDirection == SwipeDirectionEnum.Right)
            {
                targetLane = Mathf.Min(playerStateController.LANE_X_POSITIONS.Length - 1, targetLane + 1);
            }

            float targetY = playerStateController.Visualizer.PlayerPosition.y + ConstGameplay.Player.STEP_DISTANCE_Y;

            if (playerStateController.IsTargetCellBlocked(targetLane, targetY))
            {
                // Target cell is blocked by furniture so combined step cannot execute
                return;
            }

            playerStateController.IsStepUpRequested = true;
            playerStateController.LastSwipeDirection = swipeDirection;
            
            StateController.ChangeState(typeof(PlayerSwitchLaneState));
        }
    }
}
