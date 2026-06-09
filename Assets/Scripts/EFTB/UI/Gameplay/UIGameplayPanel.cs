using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay
{
    public class UIGameplayPanel : UIBasePanel
    {
        [SerializeField]
        private Button pauseButton;

        [SerializeField]
        private TextMeshProUGUI coinCounterLabel;

        public void Initialize()
        {
            if (coinCounterLabel != null)
            {
                coinCounterLabel.text = "Coins: 0";
            }
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void Subscribe(Action pauseBtnCallback)
        {
            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(() => pauseBtnCallback());
            }
        }

        public void SetCoinCounterLabel(int value)
        {
            coinCounterLabel.text = $"Coins: {value}";
        }
    }
}
