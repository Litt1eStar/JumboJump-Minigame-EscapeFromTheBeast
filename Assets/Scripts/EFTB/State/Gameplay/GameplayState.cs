using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayState : BaseState
    {
        private GameplayStateManager gameplayStateManager;
        private CollectibleVisualizer collectibleVisualizer;
        private CollectibleManager collectibleManager;

        public GameplayState(BaseStateController stateController) : base(stateController)
        {

        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            DebugLogHelper.Log("GameplayState: Entered Gameplay State");

            collectibleManager = GameContext.Instance.Get<CollectibleManager>();
            if(collectibleManager == null)
            {
                DebugLogHelper.LogError("GameplayState: CollectibleManager not found in GameContext.");
             }

            collectibleVisualizer = new CollectibleVisualizer();
            collectibleVisualizer.Initialize();

            collectibleManager.EventTotalCoinValueChanged += collectibleVisualizer.UpdateCoinChanged;
            
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            
            if(collectibleManager != null)
            {
                collectibleManager.EventTotalCoinValueChanged -= collectibleVisualizer.UpdateCoinChanged;
            }

            collectibleVisualizer?.Dispose();
            collectibleVisualizer = null;

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
