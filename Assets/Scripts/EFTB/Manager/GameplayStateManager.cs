using JumboJumps.EFTB.Visualizer;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Manager
{
    public enum GameStatus 
    { 
        Default,
        Winning,
        Losing
    }

    public class GameplayStateManager
    {
        private GameplayStateController stateController;
        private GameVisualizer gameVisualizer;

        public GameplayStateController StateController => stateController;
        public GameStatus GameStatus { get; private set; } = GameStatus.Default;

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

        public void WinGame()
        {
            if(GameStatus != GameStatus.Default) return;

            GameStatus = GameStatus.Winning;
        }

        public void LoseGame()
        {
            if(GameStatus != GameStatus.Default) return;

            GameStatus = GameStatus.Losing;
        }
    }
}
