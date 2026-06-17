using System;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay.PauseMenu
{
    public class UIPauseMenuPanel : UIBasePanel
    {
        public event Action EventResumeUIButtonClicked;
        public event Action EventMainMenuUIButtonClicked;

        [SerializeField]
        private Button resumeButton;

        [SerializeField]
        private Button mainMenuButton;

        public void Initialize()
        {
            Subscribe();
        }

        public void Subscribe()
        {
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        public void OnResumeButtonClicked()
        {
            EventResumeUIButtonClicked?.Invoke();
        }

        public void OnMainMenuButtonClicked()
        {
            EventMainMenuUIButtonClicked?.Invoke();
        }
    }
}
