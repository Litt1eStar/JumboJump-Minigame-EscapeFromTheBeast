using System;
using UnityEngine;

namespace JumboJumps.EFTB.UI.MainMenu
{
    public class UIMainMenuCanvas : MonoBehaviour
    {
        [SerializeField]
        private UIMainMenuPanel uiMainMenuPanel;

        public UIMainMenuPanel UIMainMenuPanel => uiMainMenuPanel;

        public void Initialize()
        {
            uiMainMenuPanel.Initialize();
        }
        public void Show()
        {
            uiMainMenuPanel?.Show();
        }

        public void Hide()
        {
            uiMainMenuPanel?.Hide();
        }

        public void StartLogoIdleAnimation()
        {
            uiMainMenuPanel?.StartLogoIdleAnimation();
        }

        public void PlayStartSequence(Action onComplete)
        {
            uiMainMenuPanel?.PlayStartSequence(onComplete);
        }
    }
}
