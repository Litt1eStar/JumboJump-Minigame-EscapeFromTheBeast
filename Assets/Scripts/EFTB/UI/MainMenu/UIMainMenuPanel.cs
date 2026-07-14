using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.MainMenu
{
    public class UIMainMenuPanel : UIBasePanel
    {
        public event Action EventPlayUIButtonClicked;
        public event Action EventExitUIButtonClicked;

        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button exitButton;

        public void Initialize()
        {
            Subscribe();
        }

        public void Subscribe()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            EventPlayUIButtonClicked?.Invoke();

            DebugLogHelper.Log("Play button clicked in UIMainMenuPanel");
        }

        private void OnExitButtonClicked()
        {
            EventExitUIButtonClicked?.Invoke(); 
        }
    }
}
