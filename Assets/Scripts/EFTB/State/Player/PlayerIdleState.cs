using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerIdleState : BaseState
    {
        private PlayerStateController playerStateController;

        public PlayerIdleState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;
            StateTransitionMap.Add(typeof(PlayerWalkingState), null);            
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            playerStateController.Input2DManager.EventTap += OnTapStart;
        }

        public override void OnExitState()
        {
            base.OnExitState();

            playerStateController.Input2DManager.EventTap -= OnTapStart;
        }

        public void OnTapStart()
        {
            playerStateController.ChangeState(typeof(PlayerWalkingState));
        }

    }
}
