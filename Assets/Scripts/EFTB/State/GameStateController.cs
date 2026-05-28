using Assets.Scripts.EFTB.State.Gameplay;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.EFTB.State
{
    public class GameStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(GameplayState);
        public GameStateController()
        {
            States = new Dictionary<Type, BaseState>()
            {
                {typeof(GameplayState), new GameplayState(this) }
            };
        }
    }
}
