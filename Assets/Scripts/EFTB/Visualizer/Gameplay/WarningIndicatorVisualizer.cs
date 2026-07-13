using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace JumboJumps.EFTB.Visualizer.Gameplay
{
    public class WarningIndicatorVisualizer
    {
        private UIGameplayCanvas gameplayCanvas;

        public void Initialize()
        {
            gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
            if (gameplayCanvas == null)
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Failed to find UIGameplayCanvas in SceneObjectContext!");
            }
        }

        public void Dispose()
        {
            gameplayCanvas = null;
        }

        public void ShowWarning(int laneIndex, float duration, Action onCompleteCallback)
        {
            if (gameplayCanvas == null)
            {
                gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
                if (gameplayCanvas == null)
                {
                    DebugLogHelper.LogError($"[{GetType().Name}] Failed to find UIGameplayCanvas in SceneObjectContext!");
                    return;
                }
            }

            gameplayCanvas.SetWarningIndicatorActive(laneIndex, true);
            gameplayCanvas.StartCoroutine(WarningRoutine(laneIndex, duration, onCompleteCallback));
        }

        private IEnumerator WarningRoutine(int laneIndex, float duration, Action onCompleteCallback)
        {
            yield return new WaitForSeconds(duration);

            DebugLogHelper.Log($"[{GetType().Name}] Timer expired for lane: {laneIndex}");
            if (gameplayCanvas != null)
            {
                gameplayCanvas.SetWarningIndicatorActive(laneIndex, false);
            }
            onCompleteCallback?.Invoke();
        }
    }
}
