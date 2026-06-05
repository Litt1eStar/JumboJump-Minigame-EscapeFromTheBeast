using JumboJumps.EFTB.State;

namespace JumboJumps.EFTB.Manager
{
    public class GameManager
    {
        private GameStateController stateController;

        public void Initialize()
        {
            stateController = new GameStateController();
            stateController.Initialize();
        }

        public void Dispose()
        {
            stateController?.Dispose();
            stateController = null;   
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController?.UpdateLogic(deltaTime);
        }
    }
}
