namespace JumboJumps.EFTB.State.Player
{
    public class PlayerWalkingState : BaseState
    {
        private PlayerStateController playerStateController;

        public PlayerWalkingState(BaseStateController stateController) : base(stateController)
        {
            playerStateController = (PlayerStateController)stateController;
            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void UpdateLogic(float deltaTime)
        {
            float xInput = playerStateController.Input2DManager.XInput; 
            if(xInput == 0)
            {
                StateController.ChangeState(typeof(PlayerIdleState));
            }
            playerStateController.Visualizer.Move(xInput);
        }
    }
}
