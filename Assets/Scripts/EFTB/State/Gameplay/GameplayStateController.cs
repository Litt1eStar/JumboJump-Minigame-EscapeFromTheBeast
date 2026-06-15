using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using JumboJumps.EFTB.Visualizer.Gameplay;
using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(InGameState);
        public GameplayController GameplayController { get; private set; }
        public GameplayVisualizer GameplayVisualizer { get; private set; }
        public GameplayStateController(GameplayController gameplayController)
        {
            GameplayController = gameplayController;
            GameplayVisualizer = new GameplayVisualizer();
            GameplayVisualizer.Initialize(GameplayController);

            States = new Dictionary<Type, BaseState>()
            {
                {typeof(InGameState), new InGameState(this)},
                {typeof(PauseMenuState), new PauseMenuState(this, GameplayController)},
                {typeof(FinishGameState), new FinishGameState(this, GameplayController)},
            };
        }
    }
}
