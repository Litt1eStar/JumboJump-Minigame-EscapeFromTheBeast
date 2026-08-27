using JumboJumps.EFTB.Constant.Localization;
using JumboJumps.EFTB.Constant.UI;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.UI.Utilities;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay.FinishLevel
{
    public class UIFinishLevelPanel : UIBasePanel
    {
        public event Action EventMainMenuUIButtonClicked;
        public event Action EventReplayUIButtonClicked;

        [Header("UI Text References")]
        [SerializeField] private TextMeshProUGUI scoreTextLabel;

        [Header("Localization References (Optional)")]
        [SerializeField] private LocalizedText scoreHeaderLocalizedText;
        [SerializeField] private LocalizedImage gameOverHeaderLocalizedImage;

        [Header("UI Button References")]
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button replayButton;

        [Header("Panel Animation References")]
        [SerializeField] private RectTransform panelContentTransform;
        [SerializeField] private CanvasGroup panelCanvasGroup;

        private CoroutineHelper coroutineHelper;
        private Coroutine scaleInCoroutine;

        public void Initialize()
        {
            if (scoreHeaderLocalizedText != null)
            {
                scoreHeaderLocalizedText.SetLocalizedKey(ConstLocalization.Keys.RESULT_SCORE_LABEL);
            }

            if (gameOverHeaderLocalizedImage != null)
            {
                gameOverHeaderLocalizedImage.SetLocalizedKey(ConstLocalization.Keys.ASSET_GAME_OVER);
            }

            Subscribe();

            coroutineHelper = GameContext.Instance?.Get<CoroutineHelper>();
        }

        public void Subscribe()
        {
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            }

            if (replayButton != null)
            {
                replayButton.onClick.AddListener(OnReplayButtonClicked);
            }
        }

        public override void Show()
        {
            base.Show();
            PlayScaleInAnimation();
        }

        public void OnMainMenuButtonClicked()
        {
            EventMainMenuUIButtonClicked?.Invoke();
        }

        public void OnReplayButtonClicked()
        {
            EventReplayUIButtonClicked?.Invoke();
            EventMainMenuUIButtonClicked?.Invoke();
        }

        public void SetScore(int score)
        {
            if (scoreTextLabel != null)
            {
                scoreTextLabel.text = score.ToString();
            }
        }

        public void SetScore(ScoreData scoreData)
        {
            if (scoreTextLabel != null)
            {
                scoreTextLabel.text = scoreData.TotalScore.ToString();
            }
        }

        public void PlayScaleInAnimation()
        {
            StopScaleInAnimation();
            scaleInCoroutine = coroutineHelper.Play(ScaleInRoutine(), this);
        }

        public void StopScaleInAnimation()
        {
            if (scaleInCoroutine != null)
            {
                coroutineHelper.Stop(scaleInCoroutine, this);
                scaleInCoroutine = null;
            }
        }

        private IEnumerator ScaleInRoutine()
        {
            Transform targetTransform = (panelContentTransform != null) ? panelContentTransform : transform;
            CanvasGroup cg = (panelCanvasGroup != null) ? panelCanvasGroup : GetComponent<CanvasGroup>();

            float duration = 0.35f;
            float elapsed = 0f;
            Vector3 startScale = Vector3.one * 0.4f;
            Vector3 popScale = Vector3.one * 1.08f;
            Vector3 endScale = Vector3.one;

            if (cg != null) cg.alpha = 0f;
            targetTransform.localScale = startScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                if (cg != null) cg.alpha = t;

                if (t < 0.7f)
                {
                    float subT = t / 0.7f;
                    targetTransform.localScale = Vector3.Lerp(startScale, popScale, subT);
                }
                else
                {
                    float subT = (t - 0.7f) / 0.3f;
                    targetTransform.localScale = Vector3.Lerp(popScale, endScale, subT);
                }

                yield return null;
            }

            if (cg != null) cg.alpha = 1f;
            targetTransform.localScale = endScale;
        }
    }
}
