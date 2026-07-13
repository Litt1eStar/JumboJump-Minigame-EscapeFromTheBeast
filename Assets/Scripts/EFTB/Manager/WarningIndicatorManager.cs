using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.Gameplay;
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
            GameContext.Instance.Remove(this);
        }

        public void ShowWarning(int laneIndex, float duration, Action onCompleteCallback)
        {
            visualizer?.ShowWarning(laneIndex, duration, onCompleteCallback);
        }

        public void ShowCatEventWarning(float duration, Action onCompleteCallback)
        {
            visualizer?.ShowCatEventWarning(duration, onCompleteCallback);
        }

        public void ShowCatDirectionWarning(int sideIndex, float duration, Action onCompleteCallback)
        {
            visualizer?.ShowCatDirectionWarning(sideIndex, duration, onCompleteCallback);
        }
    }
}