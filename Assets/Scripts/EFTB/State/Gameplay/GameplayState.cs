using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayState : BaseState
    {
        private CollectibleVisualizer collectibleVisualizer;
        private CollectibleManager collectibleManager;

        public GameplayState(BaseStateController stateController) : base(stateController)
        {

        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            collectibleManager = GameContext.Instance.Get<CollectibleManager>();
            if(collectibleManager == null)
            {
                DebugLogHelper.LogError("GameplayState: CollectibleManager not found in GameContext.");
             }

            collectibleVisualizer = new CollectibleVisualizer();
            collectibleVisualizer.Initialize();

            collectibleManager.EventTotalCoinValueChanged += collectibleVisualizer.UpdateCoinChanged;
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
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }
    }
}
