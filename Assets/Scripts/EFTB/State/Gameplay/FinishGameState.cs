using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class FinishGameState : BaseState
    {
        private GameplayStateController stateController;

        public FinishGameState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (GameplayStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            GameplayStateManager gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            gameplayStateManager.InvokeFinishLevel(gameplayStateManager.CurrentGameStatus);
        }
    }
}
