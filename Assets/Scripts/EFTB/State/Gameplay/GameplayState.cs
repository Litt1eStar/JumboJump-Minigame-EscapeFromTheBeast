using JumboJump.Assets.Scripts.EFTB.State.Gameplay;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayState : BaseState
    {
        private GameplayStateManager gameplayStateManager;
        private GameplayController gameplayController;
        private GameStateController stateController;
        public GameplayState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (GameStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            DebugLogHelper.Log("GameplayState: Entered Gameplay State");

            gameplayController = new GameplayController();
            gameplayController.Initialize();

            gameplayController.EventReturnBackToMainMenu += ReturnBackToMainMenu;

            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            

            gameplayStateManager?.Dispose();
            gameplayStateManager = null;

            if(gameplayController != null)
            {
                gameplayController.EventReturnBackToMainMenu -= ReturnBackToMainMenu;
                gameplayController.Dispose();
                gameplayController = null;
            }
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            gameplayStateManager.UpdateLogic(deltaTime);
        }

        public void ReturnBackToMainMenu()
        {
            stateController.ChangeState(typeof(MainMenuState));
        }
    }
}
