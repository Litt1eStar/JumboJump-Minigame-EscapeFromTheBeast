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

        public override void UpdateLogic(float deltaTime)
        {
            /*
            float xInput = playerStateController.Input2DManager.XInput;
            if (xInput > 0 || xInput < 0)
            {
                StateController.ChangeState(typeof(PlayerWalkingState));
            }
            playerStateController.Visualizer.Idle();
            */
        }

    }
}
