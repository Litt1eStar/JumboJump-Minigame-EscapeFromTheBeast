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
        private CoroutineHelper coroutineHelper;

        private Coroutine warningRoutine;
        private Coroutine catEventRoutine;
        private Coroutine catDirectionRoutine;

        public void Initialize()
        {
            gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
        }

        public void Dispose()
        {
            if (coroutineHelper != null)
            {
                coroutineHelper.Stop(warningRoutine);
                coroutineHelper.Stop(catEventRoutine);
                coroutineHelper.Stop(catDirectionRoutine);
            }

            warningRoutine = null;
            catEventRoutine = null;
            catDirectionRoutine = null;

            gameplayCanvas = null;
            coroutineHelper = null;
        }

        private void EnsureDependencies()
        {
            if (gameplayCanvas == null)
            {
                gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
            }

            if (coroutineHelper == null)
            {
                coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            }
        }

        public void ShowWarning(int laneIndex, float duration, Action onCompleteCallback)
        {
            EnsureDependencies();
            if (gameplayCanvas != null && coroutineHelper != null)
            {
                gameplayCanvas.SetWarningIndicatorActive(laneIndex, true);
                warningRoutine = coroutineHelper.Restart(warningRoutine, HideWarningRoutine(laneIndex, duration, onCompleteCallback));
            }
        }

        private IEnumerator HideWarningRoutine(int laneIndex, float duration, Action onCompleteCallback)
        {
            yield return new WaitForSeconds(duration);

            DebugLogHelper.Log($"[WarningIndicatorVisualizer] Timer expired for lane: {laneIndex}");
            if (gameplayCanvas != null)
            {
                gameplayCanvas.SetWarningIndicatorActive(laneIndex, false);
            }
            onCompleteCallback?.Invoke();
        }

        public void ShowCatEventWarning(float duration, Action onCompleteCallback)
        {
            EnsureDependencies();
            if (gameplayCanvas != null && coroutineHelper != null)
            {
                gameplayCanvas.SetCatEventWarningActive(true);
                catEventRoutine = coroutineHelper.Restart(catEventRoutine, HideCatEventWarningRoutine(duration, onCompleteCallback));
            }
            else
            {
                onCompleteCallback?.Invoke();
            }
        }

        private IEnumerator HideCatEventWarningRoutine(float duration, Action onCompleteCallback)
        {
            yield return new WaitForSeconds(duration);

            DebugLogHelper.Log("[WarningIndicatorVisualizer] CatEventWarning expired");
            if (gameplayCanvas != null)
            {
                gameplayCanvas.SetCatEventWarningActive(false);
            }
            onCompleteCallback?.Invoke();
        }

        public void ShowCatDirectionWarning(int sideIndex, float duration, Action onCompleteCallback)
        {
            EnsureDependencies();
            if (gameplayCanvas != null && coroutineHelper != null)
            {
                gameplayCanvas.SetCatDirectionWarningActive(sideIndex, true);
                catDirectionRoutine = coroutineHelper.Restart(catDirectionRoutine, HideCatDirectionWarningRoutine(sideIndex, duration, onCompleteCallback));
            }
            else
            {
                onCompleteCallback?.Invoke();
            }
        }

        private IEnumerator HideCatDirectionWarningRoutine(int sideIndex, float duration, Action onCompleteCallback)
        {
            yield return new WaitForSeconds(duration);

            DebugLogHelper.Log($"[WarningIndicatorVisualizer] CatDirectionWarning expired for side: {sideIndex}");
            if (gameplayCanvas != null)
            {
                gameplayCanvas.SetCatDirectionWarningActive(sideIndex, false);
            }
            onCompleteCallback?.Invoke();
        }
    }
}
