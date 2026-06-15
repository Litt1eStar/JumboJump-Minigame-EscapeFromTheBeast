using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay
{
    public class UIGameplayPanel : UIBasePanel
    {
        public event Action EventPauseUIButtonClicked;

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

            Subscribe();
        }

        private void Subscribe()
        {
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        public void OnPauseButtonClicked()
        {
            EventPauseUIButtonClicked?.Invoke();
        }

        public void SetCoinCounterLabel(int value)
        {
            coinCounterLabel.text = $"Coins: {value}";
        }
    }
}
