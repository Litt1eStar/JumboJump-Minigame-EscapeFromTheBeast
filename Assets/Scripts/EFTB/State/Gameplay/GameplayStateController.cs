using JumboJump.EFTB.State.Gameplay;
using JumboJumps.EFTB.State.MainMenu;
using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(InGameState);
        public GameplayController GameplayController { get; private set; }
        public GameplayStateController(GameplayController gameplayController)
        {
            States = new Dictionary<Type, BaseState>()
            {
                {typeof(InGameState), new InGameState(this)},
                {typeof(PauseMenuState), new PauseMenuState(this)},
                {typeof(FinishGameState), new FinishGameState(this)},
            };

            GameplayController = gameplayController;
        }
    }
}
