using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.State.Gameplay
{
    public enum GameStatus
    {
        Win,
        Lose
    }
    public class GameplayController
    {
        /// <summary>
        /// EventReturnBackToMainMenu will triggerd when player want to go back to main menu
        /// </summary>
        public event Action EventReturnBackToMainMenu;

        /// <summary>
        /// Parameter : GameStatus - Win or Lose
        /// </summary>
        public event Action<GameStatus> EventFinishLevel;

        private CollectibleManager collectibleManager;

        public void Initialize(GameplayStateController stateController)
        {
            collectibleManager = GameContext.Instance.Get<CollectibleManager>();
            if (collectibleManager == null)
            {
                DebugLogHelper.LogError("GameplayController: CollectibleManager not found in GameContext.");
                return;
            }
        }

        public void Dispose()
        {
            collectibleManager?.Dispose();
            collectibleManager = null;
        }

        #region Event Handler
        
        public void OnClickMainMenuButton()
        {
            ReturnToMainMenu();
        }

        public void ReturnToMainMenu()
        {
            EventReturnBackToMainMenu?.Invoke();
        }

        public void InvokeFinishLevel(GameStatus gameStatus)
        {
            EventFinishLevel?.Invoke(gameStatus);
        }

        #endregion
    }
}
