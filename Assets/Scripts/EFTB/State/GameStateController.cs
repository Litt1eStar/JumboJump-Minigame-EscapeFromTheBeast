using JumboJump.Assets.Scripts.EFTB.State.InitialLoading;
using JumboJump.Assets.Scripts.EFTB.State.MainMenu;
using JumboJumps.EFTB.State.Gameplay;
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
