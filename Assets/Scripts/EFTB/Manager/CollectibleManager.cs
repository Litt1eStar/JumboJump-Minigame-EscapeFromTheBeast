using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Manager
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
            GameContext.Instance.Remove(this);
        }

        public void AddValue(int value)
        {
            TotalCoinValue += value;
            EventTotalCoinValueChanged?.Invoke(TotalCoinValue); //Notify UI to update coin value
        }
    }
}
