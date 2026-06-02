using JumboJumps.EFTB.Visualizer;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.State;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Manager
{
    public class GameplayStateManager
    {
        private GameplayStateController stateController;
        private GameVisualizer gameVisualizer;
        public void Initialize(GameplayController gameplayController)
        {
            gameVisualizer = GameContext.Instance.Get<GameVisualizer>();

            stateController = new GameplayStateController(gameplayController);
            stateController.EventStateChanged += OnGameStateChanged;
            stateController.Initialize();

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            if(stateController != null)
            {
                stateController.EventStateChanged -= OnGameStateChanged;
                stateController.Dispose();
                stateController = null;
            }
            gameVisualizer?.UpdateInnerStateLabel("");
            gameVisualizer = null;

            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController?.UpdateLogic(deltaTime);
        }

        private void OnGameStateChanged(BaseState prev, BaseState next)
        {
            gameVisualizer?.UpdateInnerStateLabel(next.GetType().Name);
        }
    }
}
