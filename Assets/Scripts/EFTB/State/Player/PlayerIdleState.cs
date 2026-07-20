using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerIdleState : BaseState
    {
        private PlayerStateController playerStateController => (PlayerStateController)StateController;

        public PlayerIdleState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerMovingState), null);            
            StateTransitionMap.Add(typeof(PlayerSwitchLaneState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            Subscribe();
        }

        public override void OnExitState()
        {
            Unsubscribe();
            
            base.OnExitState();
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
            playerStateController.IsStepUpRequested = true;

            StateController.ChangeState(typeof(PlayerMovingState));
        }

        public void OnSwipe(SwipeDirectionEnum swipeDirection)
        {
            playerStateController.IsStepUpRequested = false;
            playerStateController.LastSwipeDirection = swipeDirection;
            
            StateController.ChangeState(typeof(PlayerSwitchLaneState));
        }

        public void OnCombinedStep(SwipeDirectionEnum swipeDirection)
        {
            playerStateController.IsStepUpRequested = true;
            playerStateController.LastSwipeDirection = swipeDirection;
            
            StateController.ChangeState(typeof(PlayerSwitchLaneState));
        }
    }
}
