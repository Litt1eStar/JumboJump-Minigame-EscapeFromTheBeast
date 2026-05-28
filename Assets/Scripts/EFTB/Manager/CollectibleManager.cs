using Assets.Scripts.EFTB.Utilities;
using Assets.Scripts.EFTB.Visualizer;
using System;

namespace Assets.Scripts.EFTB.Manager
{
    public class CollectibleManager
    {
        public event Action<int> EventTotalCoinValueChanged;
        public int TotalCoinValue { get; private set; }

        private CollectibleVisualizer visualizer;
        public void Initialize()
        {
            visualizer = new CollectibleVisualizer();
            visualizer.Initialize(this);

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            visualizer.Dispose();
            visualizer = null;
            
            GameContext.Instance.Remove(this);
        }

        public void AddValue(int value)
        {
            TotalCoinValue += value;
            EventTotalCoinValueChanged?.Invoke(TotalCoinValue); //Notify UI to update coin value
        }
    }
}
