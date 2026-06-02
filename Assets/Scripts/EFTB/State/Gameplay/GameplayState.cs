using JumboJump.Assets.Scripts.EFTB.State.Gameplay;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayState : BaseState
    {
        private GameplayStateManager gameplayStateManager;
        private GameplayController gameplayController;
        public GameplayState(BaseStateController stateController) : base(stateController)
        {

        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            DebugLogHelper.Log("GameplayState: Entered Gameplay State");

            gameplayController = new GameplayController();
            gameplayController.Initialize();

            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            

            gameplayStateManager?.Dispose();
            gameplayStateManager = null;
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            gameplayStateManager.UpdateLogic(deltaTime);
        }
    }
}
