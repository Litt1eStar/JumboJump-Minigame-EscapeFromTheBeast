using JumboJump.Assets.Scripts.EFTB.Manager;
using JumboJumps.EFTB.Visualizer;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayState : BaseState
    {
        private GameplayStateManager gameplayStateManager;
        private CollectibleVisualizer collectibleVisualizer;
        public GameplayState(BaseStateController stateController) : base(stateController)
        {
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            collectibleVisualizer = new CollectibleVisualizer();
            collectibleVisualizer.Initialize();
            
            gameplayStateManager = new GameplayStateManager();
            gameplayStateManager.Initialize();
        }

        public override void OnExitState()
        {
            base.OnExitState();

            collectibleVisualizer?.Dispose();
            collectibleVisualizer = null;
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }
    }
}
