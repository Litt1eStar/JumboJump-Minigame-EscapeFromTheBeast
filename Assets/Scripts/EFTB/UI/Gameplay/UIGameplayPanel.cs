using JumboJumps.EFTB.Constant.UI;
using JumboJumps.EFTB.Model;
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
        private TextMeshProUGUI scoreCounterLabel;

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

        public void Initialize()
        {
            if (laneWarningIndicators != null)
            {
                activeFadeCoroutines = new Coroutine[laneWarningIndicators.Length];
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

        public void SetScoreLabel(ScoreData scoreData)
        {
            if (scoreCounterLabel != null)
            {
                scoreCounterLabel.text = ConstUI.Gameplay.BASE_SCORE_LABEL + scoreData.TotalScore.ToString();
            }
        }

        public void SetWarningIndicatorActive(int laneIndex, bool active)
        {
            DebugLogHelper.Log($"[UIGameplayPanel] SetWarningIndicatorActive called: lane={laneIndex}, active={active}");
            if (laneWarningIndicators != null && laneIndex >= 0 && laneIndex < laneWarningIndicators.Length)
            {
                var indicator = laneWarningIndicators[laneIndex];
                if (indicator != null)
                {
                    // Cancel any active transitions for this lane to avoid conflicts
                    if (activeFadeCoroutines != null && activeFadeCoroutines[laneIndex] != null)
                    {
                        DebugLogHelper.Log($"[UIGameplayPanel] Canceling active coroutine for lane {laneIndex}");
                        StopCoroutine(activeFadeCoroutines[laneIndex]);
                        activeFadeCoroutines[laneIndex] = null;
                    }

                    if (active)
                    {
                        DebugLogHelper.Log($"[UIGameplayPanel] Starting fade-in for lane {laneIndex}");
                        indicator.SetActive(true);
                        activeFadeCoroutines[laneIndex] = StartCoroutine(FadeCanvasGroup(indicator, 0f, 1f, 0.25f, () => {
                            DebugLogHelper.Log($"[UIGameplayPanel] Fade-in complete callback for lane {laneIndex}");
                            ClearCoroutineTracker(laneIndex);
                        }));
                    }
                    else
                    {
                        DebugLogHelper.Log($"[UIGameplayPanel] Starting fade-out for lane {laneIndex}");
                        activeFadeCoroutines[laneIndex] = StartCoroutine(FadeCanvasGroup(indicator, 1f, 0f, 0.25f, () => {
                            DebugLogHelper.Log($"[UIGameplayPanel] Fade-out complete callback. Deactivating GameObject for lane {laneIndex}");
                            indicator.SetActive(false);
                            ClearCoroutineTracker(laneIndex);
                        }));
                    }
                }
                else
                {
                    DebugLogHelper.LogWarning($"[UIGameplayPanel] Indicator at index {laneIndex} is null!");
                }
            }
            else
            {
                DebugLogHelper.LogWarning($"[UIGameplayPanel] laneWarningIndicators is null or index {laneIndex} out of bounds!");
            }
        }

        private void ClearCoroutineTracker(int laneIndex)
        {
            if (activeFadeCoroutines != null && laneIndex >= 0 && laneIndex < activeFadeCoroutines.Length)
            {
                activeFadeCoroutines[laneIndex] = null;
            }
        }

        private IEnumerator FadeCanvasGroup(GameObject target, float startAlpha, float endAlpha, float duration, Action onComplete = null)
        {
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
            onComplete?.Invoke();
        }

        public void SetCatEventWarningActive(bool active)
        {
            if (aggressiveCatEventWarning != null)
            {
                if (catEventFadeCoroutine != null)
                {
                    StopCoroutine(catEventFadeCoroutine);
                    catEventFadeCoroutine = null;
                }

                if (active)
                {
                    aggressiveCatEventWarning.SetActive(true);
                    catEventFadeCoroutine = StartCoroutine(FadeCanvasGroup(aggressiveCatEventWarning, 0f, 1f, 0.25f, () => {
                        catEventFadeCoroutine = null;
                    }));
                }
                else
                {
                    catEventFadeCoroutine = StartCoroutine(FadeCanvasGroup(aggressiveCatEventWarning, 1f, 0f, 0.25f, () => {
                        aggressiveCatEventWarning.SetActive(false);
                        catEventFadeCoroutine = null;
                    }));
                }
            }
        }

        public void SetCatDirectionWarningActive(int sideIndex, bool active)
        {
            GameObject indicator = (sideIndex == 0) ? aggressiveCatLeftWarning : aggressiveCatRightWarning;
            if (indicator == null) return;

            if (sideIndex == 0)
            {
                if (catLeftFadeCoroutine != null)
                {
                    StopCoroutine(catLeftFadeCoroutine);
                    catLeftFadeCoroutine = null;
                }

                if (active)
                {
                    indicator.SetActive(true);
                    catLeftFadeCoroutine = StartCoroutine(FadeCanvasGroup(indicator, 0f, 1f, 0.25f, () => {
                        catLeftFadeCoroutine = null;
                    }));
                }
                else
                {
                    catLeftFadeCoroutine = StartCoroutine(FadeCanvasGroup(indicator, 1f, 0f, 0.25f, () => {
                        indicator.SetActive(false);
                        catLeftFadeCoroutine = null;
                    }));
                }
            }
            else
            {
                if (catRightFadeCoroutine != null)
                {
                    StopCoroutine(catRightFadeCoroutine);
                    catRightFadeCoroutine = null;
                }

                if (active)
                {
                    indicator.SetActive(true);
                    catRightFadeCoroutine = StartCoroutine(FadeCanvasGroup(indicator, 0f, 1f, 0.25f, () => {
                        catRightFadeCoroutine = null;
                    }));
                }
                else
                {
                    catRightFadeCoroutine = StartCoroutine(FadeCanvasGroup(indicator, 1f, 0f, 0.25f, () => {
                        indicator.SetActive(false);
                        catRightFadeCoroutine = null;
                    }));
                }
            }
        }
    }
}
