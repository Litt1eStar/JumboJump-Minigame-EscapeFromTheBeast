using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class WarningIndicatorManager
    {
        private UIGameplayCanvas gameplayCanvas;

        public void Initialize()
        {
            GameContext.Instance.Add(this);
            gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
        }

        public void ShowWarning(int laneIndex, float duration, Action onCompleteCallback)
        {
            DebugLogHelper.Log($"[WarningIndicatorManager] ShowWarning called for lane: {laneIndex}, duration: {duration}");

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
                gameplayCanvas.StartCoroutine(WarningRoutine(laneIndex, duration, onCompleteCallback));
            }
        }

        private IEnumerator WarningRoutine(int laneIndex, float duration, Action onCompleteCallback)
        {
            yield return new WaitForSeconds(duration);

            DebugLogHelper.Log($"[WarningIndicatorManager] Timer expired for lane: {laneIndex}");
            if (gameplayCanvas != null)
            {
                gameplayCanvas.SetWarningIndicatorActive(laneIndex, false);
            }
            onCompleteCallback?.Invoke();
        }
    }
}
