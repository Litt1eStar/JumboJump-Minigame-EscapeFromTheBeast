using System;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.MainMenu
{
    public class UIMainMenuPanel : UIBasePanel
    {
        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button exitButton;

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void Subscribe(Action playBtnCallback, Action exitBtnCallback)
        {
            if (playButton != null)
            {
                playButton.onClick.AddListener(() => playBtnCallback());
            }

            if (exitButton != null)
            {
                exitButton.onClick.AddListener(() => exitBtnCallback());
            }
        }

        public void OnClickPlayButton()
        {
            if (playButton != null) 
            {
                playButton.onClick.Invoke();
            }
        }

        public void OnClickExitButton()
        {
            if(exitButton != null)
            {
                exitButton.onClick.Invoke();
            }
        }
    }
}
