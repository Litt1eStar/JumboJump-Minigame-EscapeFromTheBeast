using JumboJumps.EFTB.State.InitialLoading;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.State.MainMenu;
using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.State
{
    public class GameStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(InitialLoadingState);
        public GameStateController()
        {
            States = new Dictionary<Type, BaseState>()
            {
                {typeof(InitialLoadingState), new InitialLoadingState(this) },
                {typeof(GameplayState), new GameplayState(this) },
                {typeof(MainMenuState), new MainMenuState(this) }
            };
        }
    }
}
