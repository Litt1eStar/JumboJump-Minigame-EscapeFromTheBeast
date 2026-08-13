using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.MainMenu
{
    public class UIMainMenuPanel : UIBasePanel
    {
        public event Action EventPlayUIButtonClicked;
        public event Action EventExitUIButtonClicked;

        [Header("Main Menu UI References")]
        [SerializeField] private RectTransform logoTransform;
        [SerializeField] private CanvasGroup logoCanvasGroup;
        [SerializeField] private Button playButton;
        [SerializeField] private CanvasGroup playButtonCanvasGroup;
        [SerializeField] private Button exitButton;
        [SerializeField] private CanvasGroup exitButtonCanvasGroup;

        [Header("Ready / Go Sequence References")]
        [SerializeField] private RectTransform readyTransform;
        [SerializeField] private CanvasGroup readyCanvasGroup;
        [SerializeField] private RectTransform goTransform;
        [SerializeField] private CanvasGroup goCanvasGroup;

        private CoroutineHelper coroutineHelper;
        private Coroutine logoIdleCoroutine;
        private Coroutine startSequenceCoroutine;

        private UIConfigSO uiConfig;
        public UIConfigSO UIConfig
        {
            get
            {
                if (uiConfig == null && SceneObjectContext.Instance != null)
                {
                    var container = SceneObjectContext.Instance.Get<GIGameplayConfigContainer>();
                    if (container != null && container.UIConfig != null)
                    {
                        uiConfig = container.UIConfig;
                    }
                }
                return uiConfig;
            }
        }

        public void Initialize()
        {
            Subscribe();

            if (readyCanvasGroup != null) readyCanvasGroup.gameObject.SetActive(false);
            if (goCanvasGroup != null) goCanvasGroup.gameObject.SetActive(false);
            
            coroutineHelper = GameContext.Instance?.Get<CoroutineHelper>();
        }

        public void Subscribe()
        {
            if (playButton != null) playButton.onClick.AddListener(OnPlayButtonClicked);
            if (exitButton != null) exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        public override void Show()
        {
            base.Show();
            ResetUIVisibility();
        }

        private void ResetUIVisibility()
        {
            if (playButton != null) playButton.interactable = true;

            if (logoCanvasGroup != null) logoCanvasGroup.alpha = 1f;
            if (playButtonCanvasGroup != null) playButtonCanvasGroup.alpha = 1f;
            if (exitButtonCanvasGroup != null) exitButtonCanvasGroup.alpha = 1f;

            if (readyCanvasGroup != null)
            {
                readyCanvasGroup.alpha = 0f;
                readyCanvasGroup.gameObject.SetActive(false);
            }

            if (goCanvasGroup != null)
            {
                goCanvasGroup.alpha = 0f;
                goCanvasGroup.gameObject.SetActive(false);
            }
        }

        public void StartLogoIdleAnimation()
        {
            StopLogoIdleAnimation();
            logoIdleCoroutine = coroutineHelper.Play(LogoIdleRoutine(), this);            
        }

        public void StopLogoIdleAnimation()
        {
            if (logoIdleCoroutine != null)
            {
                coroutineHelper.Stop(logoIdleCoroutine, this);
                logoIdleCoroutine = null;
            }
        }

        private IEnumerator LogoIdleRoutine()
        {
            if (logoTransform == null) yield break;

            Vector3 originalScale = Vector3.one;
            float timer = 0f;
            float speed = (UIConfig != null) ? UIConfig.LogoIdleScaleSpeed : ConstGameplay.UI.MainMenu.LOGO_IDLE_SCALE_SPEED;
            float minScale = (UIConfig != null) ? UIConfig.LogoIdleScaleMin : ConstGameplay.UI.MainMenu.LOGO_IDLE_SCALE_MIN;
            float maxScale = (UIConfig != null) ? UIConfig.LogoIdleScaleMax : ConstGameplay.UI.MainMenu.LOGO_IDLE_SCALE_MAX;

            while (true)
            {
                timer += Time.deltaTime * speed;
                float wave = (Mathf.Sin(timer) + 1f) * 0.5f;
                float scaleFactor = Mathf.Lerp(minScale, maxScale, wave);
                logoTransform.localScale = originalScale * scaleFactor;
                yield return null;
            }
        }

        public void PlayStartSequence(Action onComplete)
        {
            StopLogoIdleAnimation();
            StopStartSequence();

            startSequenceCoroutine = coroutineHelper.Play(StartSequenceRoutine(onComplete), this);
        }

        public void StopStartSequence()
        {
            if (startSequenceCoroutine != null)
            {
                coroutineHelper.Stop(startSequenceCoroutine, this);
                startSequenceCoroutine = null;
            }
        }

        private IEnumerator StartSequenceRoutine(Action onComplete)
        {
            float fadeDuration = (UIConfig != null) ? UIConfig.FadeDuration : ConstGameplay.UI.MainMenu.FADE_DURATION;
            float holdDuration = (UIConfig != null) ? UIConfig.ReadyGoHoldDuration : ConstGameplay.UI.MainMenu.READY_GO_HOLD_DURATION;

            // Phase 1: Fade out Logo & Start Button
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                if (logoCanvasGroup != null) logoCanvasGroup.alpha = 1f - t;
                if (playButtonCanvasGroup != null) playButtonCanvasGroup.alpha = 1f - t;
                if (exitButtonCanvasGroup != null) exitButtonCanvasGroup.alpha = 1f - t;
                yield return null;
            }

            if (logoCanvasGroup != null) logoCanvasGroup.alpha = 0f;
            if (playButtonCanvasGroup != null) playButtonCanvasGroup.alpha = 0f;
            if (exitButtonCanvasGroup != null) exitButtonCanvasGroup.alpha = 0f;

            // Phase 2: Ready GameObject Impactful Z-Rotation Swing
            yield return AnimateReadySwingAndPop(readyTransform, readyCanvasGroup, fadeDuration, holdDuration);

            // Phase 3: Go GameObject Impactful Scale Out
            yield return AnimateGoImpactScaleOut(goTransform, goCanvasGroup, fadeDuration, holdDuration);

            onComplete?.Invoke();
        }

        private IEnumerator AnimateReadySwingAndPop(RectTransform elementTransform, CanvasGroup elementCanvasGroup, float fadeDuration, float holdDuration)
        {
            if (elementTransform == null && elementCanvasGroup == null) yield break;

            if (elementCanvasGroup != null)
            {
                elementCanvasGroup.gameObject.SetActive(true);
                elementCanvasGroup.alpha = 0f;
            }

            float elapsed = 0f;
            float startScale = (UIConfig != null) ? UIConfig.ReadyGoScaleStart : ConstGameplay.UI.MainMenu.READY_GO_SCALE_START;
            float targetScale = (UIConfig != null) ? UIConfig.ReadyGoScaleTarget : ConstGameplay.UI.MainMenu.READY_GO_SCALE_TARGET;
            float maxZAngle = (UIConfig != null) ? UIConfig.ReadySwingMaxZAngle : ConstGameplay.UI.MainMenu.READY_SWING_MAX_Z_ANGLE;
            float swingSpeed = (UIConfig != null) ? UIConfig.ReadySwingSpeed : ConstGameplay.UI.MainMenu.READY_SWING_SPEED;

            Quaternion originalRotation = elementTransform != null ? elementTransform.localRotation : Quaternion.identity;

            // Fade In + Pop Scale + Z-Rotation Swing Impact
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);

                if (elementCanvasGroup != null) elementCanvasGroup.alpha = t;
                if (elementTransform != null)
                {
                    float currentScale = Mathf.Lerp(startScale, targetScale, t);
                    elementTransform.localScale = Vector3.one * currentScale;

                    float zAngle = Mathf.Sin(t * swingSpeed) * maxZAngle * (1f - t);
                    elementTransform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, zAngle);
                }
                yield return null;
            }

            if (elementTransform != null)
            {
                elementTransform.localRotation = originalRotation;
            }

            yield return new WaitForSeconds(holdDuration);

            // Fade Out
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                if (elementCanvasGroup != null) elementCanvasGroup.alpha = 1f - t;
                yield return null;
            }

            if (elementCanvasGroup != null)
            {
                elementCanvasGroup.alpha = 0f;
                elementCanvasGroup.gameObject.SetActive(false);
            }
        }

        private IEnumerator AnimateGoImpactScaleOut(RectTransform elementTransform, CanvasGroup elementCanvasGroup, float fadeDuration, float holdDuration)
        {
            if (elementTransform == null && elementCanvasGroup == null) yield break;

            if (elementCanvasGroup != null)
            {
                elementCanvasGroup.gameObject.SetActive(true);
                elementCanvasGroup.alpha = 0f;
            }

            float elapsed = 0f;
            float startScale = (UIConfig != null) ? UIConfig.ReadyGoScaleStart : ConstGameplay.UI.MainMenu.READY_GO_SCALE_START;
            float targetScale = (UIConfig != null) ? UIConfig.ReadyGoScaleTarget : ConstGameplay.UI.MainMenu.READY_GO_SCALE_TARGET;
            float scaleOutTarget = (UIConfig != null) ? UIConfig.GoScaleOutTarget : ConstGameplay.UI.MainMenu.GO_SCALE_OUT_TARGET;

            // Fade In & Scale Up to target
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                if (elementCanvasGroup != null) elementCanvasGroup.alpha = t;
                if (elementTransform != null)
                {
                    float currentScale = Mathf.Lerp(startScale, targetScale, t);
                    elementTransform.localScale = Vector3.one * currentScale;
                }
                yield return null;
            }

            // Hold at peak scale
            yield return new WaitForSeconds(holdDuration);

            // Impactful Scale Out (Zoom out large while fading alpha)
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                if (elementCanvasGroup != null) elementCanvasGroup.alpha = 1f - t;
                if (elementTransform != null)
                {
                    float currentScale = Mathf.Lerp(targetScale, scaleOutTarget, t);
                    elementTransform.localScale = Vector3.one * currentScale;
                }
                yield return null;
            }

            if (elementCanvasGroup != null)
            {
                elementCanvasGroup.alpha = 0f;
                elementCanvasGroup.gameObject.SetActive(false);
            }

            if (elementTransform != null)
            {
                elementTransform.localScale = Vector3.one;
            }
        }

        private void OnPlayButtonClicked()
        {
            if (playButton != null) playButton.interactable = false;
            EventPlayUIButtonClicked?.Invoke();
        }

        private void OnExitButtonClicked()
        {
            EventExitUIButtonClicked?.Invoke(); 
        }
    }
}
