using System;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay.PauseMenu
{
    public class UIPauseMenuPanel : UIBasePanel
    {
        [SerializeField]
        private Button resumeButton;

        [SerializeField]
        private Button mainMenuButton;
        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void Subscribe(Action resumeButtonCallback, Action mainMenuButtonCallback)
        {
            if(resumeButton != null)
            {
                resumeButton.onClick.AddListener(() => resumeButtonCallback());
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(() => mainMenuButtonCallback());
            }
        }
    }
}
