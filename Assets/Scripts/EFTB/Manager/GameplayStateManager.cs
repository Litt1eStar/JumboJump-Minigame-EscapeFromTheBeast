using JumboJump.EFTB.State.Gameplay;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Manager
{
    public class GameplayStateManager
    {
        private GameplayStateController stateController;
        private GameplayController gameplayController;
        public void Initialize(GameplayController gameplayController)
        {
            stateController = new GameplayStateController(gameplayController);
            stateController.Initialize();

            this.gameplayController = gameplayController;

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            if(stateController != null)
            {
                stateController.Dispose();
                stateController = null;
            }

            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController.UpdateLogic(deltaTime);
        }
    }
}
