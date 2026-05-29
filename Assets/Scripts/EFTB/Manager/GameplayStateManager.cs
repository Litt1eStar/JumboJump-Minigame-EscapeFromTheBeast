using JumboJump.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJump.EFTB.Manager
{
    public class GameplayStateManager
    {
        private GameplayStateController stateController;
        public void Initialize()
        {
            stateController = new GameplayStateController();
            stateController.Initialize();
            stateController.StartStateController();

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
