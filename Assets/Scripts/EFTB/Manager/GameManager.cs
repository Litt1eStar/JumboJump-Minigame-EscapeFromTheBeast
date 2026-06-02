using JumboJumps.EFTB.Visualizer;
using JumboJumps.EFTB.State;

namespace JumboJumps.EFTB.Manager
{
    public class GameManager
    {
        private GameStateController stateController;
        private GameVisualizer gameVisualizer;
        public void Initialize()
        {
            gameVisualizer = new GameVisualizer();
            gameVisualizer.Initialize();

            stateController = new GameStateController();
            stateController.EventStateChanged += OnGameStateChanged;
            stateController.Initialize();
        }

        public void Dispose()
        {
            if(stateController != null)
            {
                stateController.EventStateChanged -= OnGameStateChanged;    
                stateController.Dispose();
                stateController = null;
            }

            gameVisualizer?.Dispose();
            gameVisualizer = null;
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController?.UpdateLogic(deltaTime);
        }

        public void OnGameStateChanged(BaseState prev, BaseState next)
        {
            gameVisualizer.UpdateOuterStateLabel(next.GetType().Name);
        }
    }
}
