using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.Manager
{
    public class WarningIndicatorManager
    {
        private class WarningTimer
        {
            public int LaneIndex { get; }
            public float RemainingTime { get; set; }
            public Action OnComplete { get; }

            public WarningTimer(int laneIndex, float duration, Action onComplete)
            {
                LaneIndex = laneIndex;
                RemainingTime = duration;
                OnComplete = onComplete;
            }
        }

        private List<WarningTimer> activeTimers = new();
        private UIGameplayCanvas gameplayCanvas;

        public void Initialize()
        {
            GameContext.Instance.Add(this);
            gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
        }

        public void Dispose()
        {
            activeTimers.Clear();
            GameContext.Instance.Remove(this);
        }

        public void ShowWarning(int laneIndex, float duration, Action onCompleteCallback)
        {
            DebugLogHelper.Log($"[WarningIndicatorManager] ShowWarning called for lane: {laneIndex}, duration: {duration}");
            activeTimers.Add(new WarningTimer(laneIndex, duration, onCompleteCallback));

            if (gameplayCanvas == null)
            {
                gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
                if (gameplayCanvas == null)
                {
                    DebugLogHelper.LogError("[WarningIndicatorManager] Failed to find UIGameplayCanvas in SceneObjectContext!");
                }
            }

            if (gameplayCanvas != null)
            {
                gameplayCanvas.SetWarningIndicatorActive(laneIndex, true);
            }
        }

        public void UpdateLogic(float deltaTime)
        {
            for (int i = activeTimers.Count - 1; i >= 0; i--)
            {
                WarningTimer timer = activeTimers[i];
                timer.RemainingTime -= deltaTime;
                if (timer.RemainingTime <= 0f)
                {
                    DebugLogHelper.Log($"[WarningIndicatorManager] Timer expired for lane: {timer.LaneIndex}");
                    if (gameplayCanvas != null)
                    {
                        gameplayCanvas.SetWarningIndicatorActive(timer.LaneIndex, false);
                    }
                    timer.OnComplete?.Invoke();
                    activeTimers.RemoveAt(i);
                }
            }
        }
    }
}
