using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer
{
    public class CollectibleVisualizer
    {
        public void Initialize()
        {
          
        }

        public void Dispose()
        {
            
        }

        public void UpdateCoinChanged(int newTotalValue)
        {
            DebugLogHelper.Log($"CollectibleVisualizer: Total Coin Value Updated to {newTotalValue}");
            //Update UI 
        }


    }
}
