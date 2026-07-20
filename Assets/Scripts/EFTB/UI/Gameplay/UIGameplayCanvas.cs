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
        public UIGameplayPanel UIGameplayPanel => uiGameplayPanel;
        public UIFinishLevelPanel UIFinishLevelPanel => uiFinishLevelPanel;

        public void ShowGameplayPanel()
        {
            uiGameplayPanel?.Show();
        }

        public void HideGameplayPanel()
        {
            uiGameplayPanel?.Hide();
        }

        public void Initialize()
        {
            uiGameplayPanel.Initialize();
            uiPauseMenuPanel.Initialize();
            uiFinishLevelPanel.Initialize();
        }


        #region Score Counter
        public void SetScoreLabel(JumboJumps.EFTB.Model.ScoreData scoreData)
        {
            uiGameplayPanel?.SetScoreLabel(scoreData);
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
            uiFinishLevelPanel?.SetFinishTextLavel(gameStatus);
        }

        #endregion

        #region Warning Indicators
        public void SetWarningIndicatorActive(int laneIndex, bool active)
        {
            uiGameplayPanel?.SetWarningIndicatorActive(laneIndex, active);
        }

        public void SetCatEventWarningActive(bool active)
        {
            uiGameplayPanel?.SetCatEventWarningActive(active);
        }

        public void SetCatDirectionWarningActive(int sideIndex, bool active)
        {
            uiGameplayPanel?.SetCatDirectionWarningActive(sideIndex, active);
        }
        #endregion
    }
}
