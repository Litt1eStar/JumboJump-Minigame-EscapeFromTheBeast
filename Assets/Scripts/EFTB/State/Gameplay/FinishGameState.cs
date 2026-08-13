using JumboJumps.EFTB.State.MainMenu;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class FinishGameState : BaseState
    {
        private GameplayStateController stateController;
        private GameplayController gameplayController;

        public FinishGameState(BaseStateController stateController, GameplayController gameplayController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(MainMenuState), null);

            this.stateController = (GameplayStateController)stateController;
            this.gameplayController = gameplayController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.EventFinishMainMenuButtonClicked += OnFinishMainMenuButtonClicked;
            }
        }

        public override void OnExitState()
        {
            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.HidePanel();
                stateController.GameplayVisualizer.EventFinishMainMenuButtonClicked -= OnFinishMainMenuButtonClicked;
            }

            base.OnExitState();
        }

        public void OnFinishMainMenuButtonClicked()
        {
            gameplayController?.ReturnToMainMenu();
        }
    }
}
