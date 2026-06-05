using JumboJumps.EFTB.State.Gameplay;
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
            GameplayController = gameplayController;

            States = new Dictionary<Type, BaseState>()
            {
                {typeof(InGameState), new InGameState(this)},
                {typeof(PauseMenuState), new PauseMenuState(this)},
                {typeof(FinishGameState), new FinishGameState(this)},
            };
        }
    }
}
