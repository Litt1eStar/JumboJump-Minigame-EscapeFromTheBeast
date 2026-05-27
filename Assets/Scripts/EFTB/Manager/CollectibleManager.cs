using Assets.Scripts.EFTB.Utilities;
using System;

namespace Assets.Scripts.EFTB.Manager
{
    public class CollectibleManager
    {
        public event Action<int> EventTotalCoinValueChanged;
        public int TotalCoinValue { get; private set; }
        public void Initialize()
        {
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {

        }

        public void AddCoin(int value)
        {
            TotalCoinValue += value;
            DebugLogHelper.Log($"Total Coin Value: {TotalCoinValue}");
            EventTotalCoinValueChanged?.Invoke(TotalCoinValue); //Notify UI to update coin value
        }
    }
}
