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
            if (gameplayStateManager != null && gameplayStateManager.GameplayController != null)
            {
                gameplayStateManager.GameplayController.InvokeFinishLevel(GameStatus.Lose);
            }
            else
            {
                DebugLogHelper.LogError($"[{GetType().Name}] Failed to retrieve GameplayStateManager to trigger game over.");
            }
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
