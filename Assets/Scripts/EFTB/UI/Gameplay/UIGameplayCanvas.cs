using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.UI.Gameplay.FinishLevel;
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

        [SerializeField]
        private UIFinishLevelPanel uiFinishLevelPanel;

        public UIPauseMenuPanel UIPauseMenuPanel => uiPauseMenuPanel;
        public void Show()
        {
            uiGameplayPanel?.Show();
        }

        public void Hide()
        {
            uiGameplayPanel?.Hide();
        }

        public void Initialize()
        {
            uiPauseMenuPanel.Initialize();
        }

        #region Coin Counter
        public void SetCoinCounterLabel(int value)
        {
            uiGameplayPanel?.SetCoinCounterLabel(value);
        }
        #endregion

        #region Pause Menu
        public void ShowPauseMenu()
        {
            uiPauseMenuPanel?.Show();
        }

        public void HidePauseMenu()
        {
            uiPauseMenuPanel?.Hide();
        }
        #endregion

        #region Finish Level Panel
        public void ShowFinishLevelPanel()
        {
            uiFinishLevelPanel?.Show();
        }

        public void HideFinishLevelPanel()
        {
            uiFinishLevelPanel?.Hide();
        }

        public void SetFinishLevelTextLabel(GameStatus gameStatus)
        {
            ShowFinishLevelPanel();
            uiFinishLevelPanel?.SetFinishTextLavel(gameStatus);
        }

        #endregion
    }
}
