using System;
using UnityEngine;

namespace JumboJumps.EFTB.UI.Gameplay
{
    public class UIGameplayCanvas : MonoBehaviour
    {
        [SerializeField]
        private UIGameplayPanel uiGameplayPanel;

        public void Show()
        {
            uiGameplayPanel?.Show();
        }

        public void Hide()
        {
            uiGameplayPanel?.Hide();
        }

        public void Subscribe(Action pauseBtnCallback)
        {
            uiGameplayPanel?.Subscribe(pauseBtnCallback);
        }

        public void SetCoinCounterLabel(int value)
        {
            uiGameplayPanel?.SetCoinCounterLabel(value);
        }
    }
}
