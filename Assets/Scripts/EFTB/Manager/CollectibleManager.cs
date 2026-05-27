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
            visualizer.Initialize();
            EventTotalCoinValueChanged += visualizer.OnTotalCoinValueChanged;

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {

        }

        public void AddCoin(int value)
        {
            TotalCoinValue += value;
            EventTotalCoinValueChanged?.Invoke(TotalCoinValue); //Notify UI to update coin value
        }
    }
}
