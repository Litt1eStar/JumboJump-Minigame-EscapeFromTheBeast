using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Visualizer.Gameplay;
using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(MainMenuState);
        public GameplayController GameplayController { get; private set; }
        public GameplayVisualizer GameplayVisualizer { get; private set; }
        public GameplayStateController(GameplayController gameplayController)
        {
            GameplayController = gameplayController;
            GameplayVisualizer = new GameplayVisualizer();
            GameplayVisualizer.Initialize(GameplayController);

            States = new Dictionary<Type, BaseState>()
            {
                {typeof(MainMenuState), new MainMenuState(this)},
                {typeof(InGameState), new InGameState(this)},
                {typeof(PauseMenuState), new PauseMenuState(this, GameplayController)},
                {typeof(FinishGameState), new FinishGameState(this, GameplayController)},
            };
        }
        
        public override void Dispose()
        {
            GameplayVisualizer?.Dispose();
            GameplayVisualizer = null;
            base.Dispose();
        }
    }
}
