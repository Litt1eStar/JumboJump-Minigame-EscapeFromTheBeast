using JumboJump.EFTB.Constant.UI;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay
{
    public class UIGameplayPanel : UIBasePanel
    {
        public event Action EventPauseUIButtonClicked;

        [SerializeField]
        private Button pauseButton;

        [SerializeField]
        private TextMeshProUGUI coinCounterLabel;

        [SerializeField]
        private TextMeshProUGUI gameplayTimerLabel;

        [SerializeField]
        private GameObject[] laneWarningIndicators;

        [Header("Aggressive Cat Warnings")]
        [SerializeField]
        private GameObject aggressiveCatEventWarning;

        [SerializeField]
        private GameObject aggressiveCatLeftWarning;

        [SerializeField]
        private GameObject aggressiveCatRightWarning;

        private Coroutine[] activeFadeCoroutines;
        private Coroutine catEventFadeCoroutine;
        private Coroutine catLeftFadeCoroutine;
        private Coroutine catRightFadeCoroutine;

        private CoroutineHelper coroutineHelper;

        public void Initialize()
        {
            if (coinCounterLabel != null)
            {
                coinCounterLabel.text = $"{ConstUI.Gameplay.BASE_COIN_COUNTER_LABEL}{0}";
            }

            if (laneWarningIndicators != null)
            {
                activeFadeCoroutines = new Coroutine[laneWarningIndicators.Length];
            }

            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();

            if (coroutineHelper == null)
            {
                DebugLogHelper.LogError("CoroutineHelper is missing from GameContext!");
                return;
            }

            Subscribe();
        }

        private void Subscribe()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        public void OnPauseButtonClicked()
        {
            EventPauseUIButtonClicked?.Invoke();
        }

        public void SetCoinCounterLabel(int value)
        {
            coinCounterLabel.text = $"{ConstUI.Gameplay.BASE_COIN_COUNTER_LABEL}{value}";
        }

        public void SetGameplayTimer(float value)
        {
            gameplayTimerLabel.text = $"{value.ToString("F2")}";
        }

        public void SetWarningIndicatorActive(int laneIndex, bool active)
        {
            DebugLogHelper.Log($"[UIGameplayPanel] SetWarningIndicatorActive called: lane={laneIndex}, active={active}");
            if (laneWarningIndicators == null || laneIndex < 0 || laneIndex >= laneWarningIndicators.Length)
            {
                DebugLogHelper.LogWarning($"[UIGameplayPanel] laneWarningIndicators is null or index {laneIndex} out of bounds!");
                return;
            }

            var indicator = laneWarningIndicators[laneIndex];
            if (indicator == null)
            {
                DebugLogHelper.LogWarning($"[UIGameplayPanel] Indicator at index {laneIndex} is null!");
                return;
            }

            if (activeFadeCoroutines != null && coroutineHelper != null)
            {
                DebugLogHelper.Log($"[UIGameplayPanel] Starting fade-{(active ? "in" : "out")} for lane {laneIndex}");
                activeFadeCoroutines[laneIndex] = coroutineHelper.Restart(
                    activeFadeCoroutines[laneIndex],
                    FadeCanvasGroup(indicator, active, 0.25f, () => {
                        DebugLogHelper.Log($"[UIGameplayPanel] Fade-{(active ? "in" : "out")} complete callback for lane {laneIndex}");
                        ClearCoroutineTracker(laneIndex);
                    }),
                    this
                );
            }
        }

        private void ClearCoroutineTracker(int laneIndex)
        {
            if (activeFadeCoroutines != null && laneIndex >= 0 && laneIndex < activeFadeCoroutines.Length)
            {
                activeFadeCoroutines[laneIndex] = null;
            }
        }

        private IEnumerator FadeCanvasGroup(GameObject target, bool active, float duration, Action onComplete = null)
        {
            float startAlpha = active ? 0f : 1f;
            float endAlpha = active ? 1f : 0f;

            if (active)
            {
                target.SetActive(true);
            }

            var cg = target.GetComponent<CanvasGroup>() ?? target.AddComponent<CanvasGroup>();
            float elapsed = 0f;
            cg.alpha = startAlpha;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
                yield return null;
            }

            cg.alpha = endAlpha;

            if (!active)
            {
                target.SetActive(false);
            }

            onComplete?.Invoke();
        }

        public void SetCatEventWarningActive(bool active)
        {
            if (aggressiveCatEventWarning == null) return;
            if (coroutineHelper == null) return;

            catEventFadeCoroutine = coroutineHelper.Restart(
                catEventFadeCoroutine,
                FadeCanvasGroup(aggressiveCatEventWarning, active, 0.25f, () => {
                    catEventFadeCoroutine = null;
                }),
                this
            );
        }

        public void SetCatDirectionWarningActive(int sideIndex, bool active)
        {
            GameObject indicator = (sideIndex == 0) ? aggressiveCatLeftWarning : aggressiveCatRightWarning;
            if (indicator == null) return;
            if (coroutineHelper == null) return;

            if (sideIndex == 0)
            {
                catLeftFadeCoroutine = coroutineHelper.Restart(
                    catLeftFadeCoroutine,
                    FadeCanvasGroup(indicator, active, 0.25f, () => {
                        catLeftFadeCoroutine = null;
                    }),
                    this
                );
            }
            else
            {
                catRightFadeCoroutine = coroutineHelper.Restart(
                    catRightFadeCoroutine,
                    FadeCanvasGroup(indicator, active, 0.25f, () => {
                        catRightFadeCoroutine = null;
                    }),
                    this
                );
            }
        }
    }
}
