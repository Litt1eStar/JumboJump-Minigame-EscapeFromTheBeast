using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class InGameState : BaseState
    {
        private GameplayStateController stateController;
        public InGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(FinishGameState), null);
            StateTransitionMap.Add(typeof(PauseMenuState), null);

            this.stateController = (GameplayStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            stateController.GameplayVisualizer.EventPauseUIButtonClicked += OnClickPauseButton;
        }

        public void OnClickPauseButton()
        {
            stateController.GameplayVisualizer.ShowPauseMenu();
            stateController.ChangeState(typeof(PauseMenuState));
        }
    }
}
