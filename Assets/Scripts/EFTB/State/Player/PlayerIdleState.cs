using JumboJump.EFTB.State.Player;
using JumboJump.EFTB.Model;
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
            StateController.ChangeState(typeof(PlayerMovingState));
        }

        public void OnSwipe(SwipeDirectionEnum swipeDirection)
        {
            StateController.ChangeState(typeof(PlayerSwitchLaneState));
        }
    }
}
