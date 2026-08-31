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

            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.EventResumeUIButtonClicked += OnClickResumeButton;
                stateController.GameplayVisualizer.RefreshPauseMenuSoundVisuals();
            }
        }

        public void OnClickResumeButton()
        {
            stateController.GameplayVisualizer.HidePanel();
            stateController.ChangeState(typeof(InGameState));
        }

        public override void OnExitState()
        {
            Time.timeScale = 1f;

            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.EventResumeUIButtonClicked -= OnClickResumeButton;
            }

            base.OnExitState();
        }
    }
}
