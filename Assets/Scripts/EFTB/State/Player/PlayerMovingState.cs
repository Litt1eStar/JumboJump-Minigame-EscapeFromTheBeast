using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerMovingState : BaseState
    {
        private PlayerStateController playerStateController => (PlayerStateController)StateController;

        private bool isMovingForward;

        public PlayerMovingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            DebugLogHelper.Log("Enter PlayerMovingState");

            isMovingForward = true;

            Subscribe();
        }

        public override void OnExitState()
        {
            Unsubscribe();
            base.OnExitState();
        }

        public void Subscribe()
        {
            playerStateController.Input2DManager.EventHoldEnded += OnHoldEnded;
        }

        public void Unsubscribe()
        {
            playerStateController.Input2DManager.EventHoldEnded -= OnHoldEnded;
        }

        public override void UpdateLogic(float deltaTime)
        {
            if (!isMovingForward) return;

            DebugLogHelper.Log("Moving Forward");
        }

        public void OnHoldEnded()
        {
            isMovingForward = false;
            StateController.ChangeState(typeof(PlayerIdleState));
        }
    }
}
