using System;
using UnityEngine;

namespace JumboJumps.EFTB.UI.MainMenu
{
    public class UIMainMenuCanvas : MonoBehaviour
    {
        [SerializeField]
        private UIMainMenuPanel uiMainMenuPanel;

        public void Show()
        {
            uiMainMenuPanel?.Show();
        }

        public void Hide()
        {
            uiMainMenuPanel?.Hide();
        }
        
        public void Subscribe(Action playBtnCallback, Action exitBtnCallback)
        {
            uiMainMenuPanel?.Subscribe(playBtnCallback, exitBtnCallback);
        }
    }
}
