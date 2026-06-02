
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using System;

namespace JumboJump.Assets.Scripts.EFTB.State.Gameplay
{
    public class GameplayController
    {
        public event Action EventReturnBackToMainMenu;

        private CollectibleManager collectibleManager;
        private CollectibleVisualizer collectibleVisualizer;
        public void Initialize()
        {
            collectibleManager = GameContext.Instance.Get<CollectibleManager>();
            if(collectibleManager == null)
            {
                DebugLogHelper.LogError("GameplayController: CollectibleManager not found in GameContext.");
            }

            collectibleVisualizer = new CollectibleVisualizer();
            collectibleVisualizer.Initialize();

            collectibleManager.EventTotalCoinValueChanged += collectibleVisualizer.UpdateCoinChanged;
        }

        public void Dispose()
        {
            if (collectibleManager != null)
            {
                collectibleManager.EventTotalCoinValueChanged -= collectibleVisualizer.UpdateCoinChanged;
                collectibleManager.Dispose();
            }

            collectibleVisualizer?.Dispose();
            collectibleVisualizer = null;
        }
    }
}
