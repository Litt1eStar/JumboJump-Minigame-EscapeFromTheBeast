using UnityEngine;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class PauseMenuState : BaseState
    {
        private GameplayStateController stateController;
        private GameplayController gameplayController;
        public PauseMenuState(BaseStateController stateController, GameplayController gameplayController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(InGameState), null);
            this.stateController = (GameplayStateController)stateController;
            this.gameplayController = gameplayController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            Time.timeScale = 0f;

            stateController.GameplayVisualizer.EventResumeUIButtonClicked += OnClickResumeButton;
            stateController.GameplayVisualizer.EventMainMenuUIButtonClicked += OnClickMainMenuButton;
        }

        public void OnClickResumeButton()
        {
            stateController.GameplayVisualizer.HidePanel();
            stateController.ChangeState(typeof(InGameState));
        }

        public void OnClickMainMenuButton()
        {
            Time.timeScale = 1f;

            gameplayController.ReturnToMainMenu();
        }
        public override void OnExitState()
        {
            Time.timeScale = 1f;

            base.OnExitState();
        }
    }
}
