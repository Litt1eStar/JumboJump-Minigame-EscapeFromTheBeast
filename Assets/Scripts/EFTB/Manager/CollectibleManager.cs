using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Manager
{
    public class CollectibleManager
    {
        public event Action<int> EventTotalCollectibleValueChanged;
        public int TotalCollectibleValue { get; private set; }

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
            TotalCollectibleValue += value;
            EventTotalCollectibleValueChanged?.Invoke(TotalCollectibleValue);
        }
    }
}
