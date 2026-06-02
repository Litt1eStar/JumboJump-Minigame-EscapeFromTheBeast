using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.MainMenu;

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

            StateTransitionMap.Add(typeof(MainMenuState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            gameplayController = new GameplayController();
            gameplayController.Initialize();
            gameplayController.EventReturnBackToMainMenu += ReturnBackToMainMenu;

            gameplayStateManager = new GameplayStateManager();
            gameplayStateManager.Initialize(gameplayController);
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

            gameplayStateManager?.UpdateLogic(deltaTime);
        }

        public void ReturnBackToMainMenu()
        {
            stateController.ChangeState(typeof(MainMenuState));
        }
    }
}
