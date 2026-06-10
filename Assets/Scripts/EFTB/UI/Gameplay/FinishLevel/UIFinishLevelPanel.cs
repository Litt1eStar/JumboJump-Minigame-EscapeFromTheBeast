using JumboJumps.EFTB.State.Gameplay;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay.FinishLevel
{
    public class UIFinishLevelPanel : UIBasePanel
    {
        [SerializeField]
        private TextMeshProUGUI finishLevelLabel;

        [SerializeField]
        private Button mainMenuButton;

        public void SetFinishTextLavel(GameStatus gameStatus)
        {
            if (gameStatus == GameStatus.Win)
            {
                finishLevelLabel.text = "Level Complete!";
            }
            else
            {
                finishLevelLabel.text = "Level Failed!";
            }
        }

        public void Subscribe(Action mainMenuButtonCallback)
        {
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(() => mainMenuButtonCallback());
            }
        }
    }
}
