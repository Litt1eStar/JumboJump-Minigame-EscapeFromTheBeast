using JumboJumps.EFTB.Visualizer;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Manager
{
    public class GameplayStateManager
    {
        private GameplayStateController stateController;
        private GameVisualizer gameVisualizer;

        public GameplayStateController StateController => stateController;
        public GameStatus CurrentGameStatus;

        public void Initialize(GameplayController gameplayController)
        {
            gameVisualizer = GameContext.Instance.Get<GameVisualizer>();

            stateController = new GameplayStateController(gameplayController);

            stateController.Initialize();
            stateController.StartStateController();

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            stateController.Dispose();
            stateController = null;

            gameVisualizer = null;

            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController?.UpdateLogic(deltaTime);
        }

        public void InvokeFinishLevel(GameStatus gameStatus)
        {
            if(stateController.GameplayController == null)
            {
                DebugLogHelper.LogError("GameplayController is null. Cannot invoke finish level.");
                return;
            }
            stateController.GameplayController.InvokeFinishLevel(gameStatus);
        }
    }
}
