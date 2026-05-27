using Assets.Scripts.EFTB.Utilities;
using System;

namespace Assets.Scripts.EFTB.Visualizer
{
    public class CollectibleVisualizer
    {
        public void Initialize()
        {

        }

        public void Dispose()
        {

        }

        public void OnTotalCoinValueChanged(int newTotalValue)
        {
            DebugLogHelper.Log($"CollectibleVisualizer: Total Coin Value Updated to {newTotalValue}");
            //Update UI 
        }


    }
}
