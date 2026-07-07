using JumboJump.EFTB.Constant.UI;
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

        [SerializeField]
        private TextMeshProUGUI gameplayTimerLabel;

        public void Initialize()
        {
            if (coinCounterLabel != null)
            {
                coinCounterLabel.text = $"{ConstUI.Gameplay.BASE_COIN_COUNTER_LABEL}{0}";
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
            coinCounterLabel.text = $"{ConstUI.Gameplay.BASE_COIN_COUNTER_LABEL}{value}";
        }

        public void SetGameplayTimer(float value)
        {
            gameplayTimerLabel.text = $"{value.ToString("F2")}";
        }
    }
}
