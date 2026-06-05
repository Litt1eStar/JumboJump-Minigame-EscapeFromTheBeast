using JumboJumps.EFTB.State.InitialLoading;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.State.MainMenu;
using System;
using System.Collections.Generic;
using JumboJumps.EFTB.Visualizer;

namespace JumboJumps.EFTB.State
{
    public class GameStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(InitialLoadingState);
        public GameVisualizer Visualizer { get; private set; }

        public GameStateController()
        {
            States = new Dictionary<Type, BaseState>()
            {
                {typeof(InitialLoadingState), new InitialLoadingState(this) },
                {typeof(GameplayState), new GameplayState(this) },
                {typeof(MainMenuState), new MainMenuState(this) }
            };
        }

        public override void Initialize()
        {
            base.Initialize();

            Visualizer = new GameVisualizer();
            Visualizer.Initialize();

            Subscribe();
        }

        public override void Dispose()
        {
            base.Dispose();

            Unsubscribe();

            Visualizer?.Dispose();
            Visualizer = null;
        }

        private void Subscribe()
        {
            EventStateChanged += OnGameStateChanged;
        }

        private void Unsubscribe()
        {
            EventStateChanged -= OnGameStateChanged;
        }

        public void OnGameStateChanged(BaseState prev, BaseState next)
        {
            Visualizer.UpdateOuterStateLabel(next.GetType().Name);
        }
    }
}
