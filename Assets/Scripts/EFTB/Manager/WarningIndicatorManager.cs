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
        private CoroutineHelper coroutineHelper;
        private Coroutine warningRoutine;

        public void Initialize()
        {
            GameContext.Instance.Add(this);
            gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
        }

        public void Dispose()
        {
            if (coroutineHelper != null)
            {
                coroutineHelper.Stop(warningRoutine);
                coroutineHelper = null;
                warningRoutine = null;
            }

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

            gameplayCanvas.SetWarningIndicatorActive(laneIndex, true);
            warningRoutine = coroutineHelper.Restart(warningRoutine, WarningRoutine(laneIndex, duration, onCompleteCallback));
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
