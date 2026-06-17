using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.Cat.SleepyCat;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Cat
{
    public class CatCatchState : BaseState
    {
        private SleepyCatStateController stateController;
        private GameplayStateManager gameplayStateManager;

        public CatCatchState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (SleepyCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();

            gameplayStateManager.GameplayController.InvokeFinishLevel(GameStatus.Lose);
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        } 
    }
}
