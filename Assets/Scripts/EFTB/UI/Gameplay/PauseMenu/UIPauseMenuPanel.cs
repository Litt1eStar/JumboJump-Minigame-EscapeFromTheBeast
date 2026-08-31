using System;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay.PauseMenu
{
    public class UIPauseMenuPanel : UIBasePanel
    {
        public event Action EventResumeUIButtonClicked;
        public event Action EventSFXUIButtonClicked;
        public event Action EventBGMUIButtonClicked;

        [Header("Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button sfxButton;
        [SerializeField] private Button bgmButton;

        [Header("SFX Visual Settings")]
        [SerializeField] private Image sfxImage;
        [SerializeField] private Sprite sfxOnSprite;
        [SerializeField] private Sprite sfxOffSprite;

        [Header("BGM Visual Settings")]
        [SerializeField] private Image bgmImage;
        [SerializeField] private Sprite bgmOnSprite;
        [SerializeField] private Sprite bgmOffSprite;

        public void Initialize()
        {
            Subscribe();
        }

        public void Subscribe()
        {
            Unsubscribe();

            if (resumeButton != null) resumeButton.onClick.AddListener(OnResumeButtonClicked);
            if (sfxButton != null) sfxButton.onClick.AddListener(OnSFXButtonClicked);
            if (bgmButton != null) bgmButton.onClick.AddListener(OnBGMButtonClicked);
        }

        public void Unsubscribe()
        {
            if (resumeButton != null) resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
            if (sfxButton != null) sfxButton.onClick.RemoveListener(OnSFXButtonClicked);
            if (bgmButton != null) bgmButton.onClick.RemoveListener(OnBGMButtonClicked);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void OnResumeButtonClicked()
        {
            EventResumeUIButtonClicked?.Invoke();
        }

        public void OnSFXButtonClicked()
        {
            EventSFXUIButtonClicked?.Invoke();
        }

        public void OnBGMButtonClicked()
        {
            EventBGMUIButtonClicked?.Invoke();
        }

        public void SetSFXVisualState(bool isOn)
        {
            if (sfxImage != null)
            {
                Sprite targetSprite = isOn ? sfxOnSprite : sfxOffSprite;
                if (targetSprite != null) sfxImage.sprite = targetSprite;
            }
        }

        public void SetBGMVisualState(bool isOn)
        {
            if (bgmImage != null)
            {
                Sprite targetSprite = isOn ? bgmOnSprite : bgmOffSprite;
                if (targetSprite != null) bgmImage.sprite = targetSprite;
            }
        }
    }
}
