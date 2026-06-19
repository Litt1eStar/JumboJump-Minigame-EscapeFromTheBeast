using JumboJump.EFTB.State.Player;
using JumboJump.EFTB.Model;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerIdleState : BaseState
    {
        private PlayerStateController playerStateController;

        public PlayerIdleState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;

            StateTransitionMap.Add(typeof(PlayerMovingState), null);            
            StateTransitionMap.Add(typeof(PlayerSwitchLaneState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            DebugLogHelper.Log("Enter PlayerIdleState");
            Subscribe();
        }

        public override void OnExitState()
        {
            Unsubscribe();
            
            base.OnExitState();
        }

        public void Subscribe()
        {
            playerStateController.Input2DManager.EventHoldStarted += OnHoldStarted;
            playerStateController.Input2DManager.EventSwipe += OnSwipe;
        }

        public void Unsubscribe() 
        {
            playerStateController.Input2DManager.EventHoldStarted -= OnHoldStarted;
            playerStateController.Input2DManager.EventSwipe -= OnSwipe;
        }

        public void OnHoldStarted()
        {
            playerStateController.ChangeState(typeof(PlayerMovingState));
        }

        public void OnSwipe(SwipeDirectionEnum swipeDirection)
        {
            playerStateController.ChangeState(typeof(PlayerSwitchLaneState));
        }
    }
}
