using JumboJumps.EFTB.UI.Gameplay.PauseMenu;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.UI.Gameplay
{
    public class UIGameplayCanvas : MonoBehaviour
    {
        [SerializeField]
        private UIGameplayPanel uiGameplayPanel;

        [SerializeField]
        private UIPauseMenuPanel uiPauseMenuPanel;
        public void Show()
        {
            uiGameplayPanel?.Show();
        }

        public void Hide()
        {
            uiGameplayPanel?.Hide();
        }

        public void Subscribe(
            Action pauseBtnCallback,
            Action resumeButtonCallback,
            Action mainMenuButtonCallback
            )
        {
            uiGameplayPanel?.Subscribe(pauseBtnCallback);
            uiPauseMenuPanel?.Subscribe(resumeButtonCallback, mainMenuButtonCallback);
        }

        public void SetCoinCounterLabel(int value)
        {
            uiGameplayPanel?.SetCoinCounterLabel(value);
        }

        public void ShowPauseMenu()
        {
            uiPauseMenuPanel?.Show();
        }

        public void HidePauseMenu()
        {
            uiPauseMenuPanel?.Hide();
        }
    }
}
