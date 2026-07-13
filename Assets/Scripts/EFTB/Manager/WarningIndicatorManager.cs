using JumboJumps.EFTB.Visualizer.Gameplay;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Manager
{
    public class WarningIndicatorManager
    {
        private WarningIndicatorVisualizer visualizer;

        public void Initialize()
        {
            GameContext.Instance.Add(this);
            visualizer = new WarningIndicatorVisualizer();
            visualizer.Initialize();
        }

        public void Dispose()
        {
            visualizer?.Dispose();
            visualizer = null;
            GameContext.Instance.Remove(this);
        }

        public void ShowWarning(int laneIndex, float duration, Action onCompleteCallback)
        {
            DebugLogHelper.Log($"[WarningIndicatorManager] ShowWarning called for lane: {laneIndex}, duration: {duration}");
            visualizer?.ShowWarning(laneIndex, duration, onCompleteCallback);
        }
    }
}
