using JumboJumps.EFTB.State.Gameplay;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Gameplay.FinishLevel
{
    public class UIFinishLevelPanel : UIBasePanel
    {
        public event Action EventMainMenuUIButtonClicked;

        [SerializeField]
        private TextMeshProUGUI finishLevelLabel;

        [SerializeField]
        private Button mainMenuButton;

        public void Initialize()
        {
            Subscribe();
        }

        public void Subscribe()
        {
            mainMenuButton.onClick.AddListener(OnMainMenuBUttonClicked);
        }

        public void OnMainMenuBUttonClicked()
        {
            EventMainMenuUIButtonClicked?.Invoke();
        }

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
    }
}
