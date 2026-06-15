using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class FinishGameState : BaseState
    {
        private GameplayStateController stateController;
        private GameplayController gameplayController;

        public FinishGameState(BaseStateController stateController, GameplayController gameplayController) : base(stateController)
        {
            this.stateController = (GameplayStateController)stateController;
            this.gameplayController = gameplayController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            stateController.GameplayVisualizer.EventFinishMainMenuButtonClicked += OnFinishMainMenuButtonClicked;
        }

        public void OnFinishMainMenuButtonClicked()
        {
            gameplayController.ReturnToMainMenu();
        }
    }
}
