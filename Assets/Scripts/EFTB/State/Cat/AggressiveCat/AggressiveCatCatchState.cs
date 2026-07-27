using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatCatchState : BaseState
    {
        private GameplayStateManager gameplayStateManager;

        public AggressiveCatCatchState(BaseStateController stateController) : base(stateController)
        {

        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();

            gameplayStateManager?.GameplayController?.InvokeFinishLevel(GameStatus.Lose);
        }
    }
}
