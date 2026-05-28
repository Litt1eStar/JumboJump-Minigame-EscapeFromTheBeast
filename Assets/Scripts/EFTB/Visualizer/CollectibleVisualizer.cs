using Assets.Scripts.EFTB.Manager;
using Assets.Scripts.EFTB.Utilities;
using System;

namespace Assets.Scripts.EFTB.Visualizer
{
    public class CollectibleVisualizer
    {
        private CollectibleManager collectibleManager;
        public void Initialize()
        {
            collectibleManager = GameContext.Instance.Get<CollectibleManager>();
            if(collectibleManager == null)
            {
                DebugLogHelper.LogWarning("CollectibleVisualizer: CollectibleManager not found in GameContext. Visualization will not work.");
            }
            collectibleManager.EventTotalCoinValueChanged += OnTotalCoinValueChanged;
        }

        public void Dispose()
        {
            collectibleManager.EventTotalCoinValueChanged -= OnTotalCoinValueChanged;
        }

        public void OnTotalCoinValueChanged(int newTotalValue)
        {
            DebugLogHelper.Log($"CollectibleVisualizer: Total Coin Value Updated to {newTotalValue}");
            //Update UI 
        }


    }
}
