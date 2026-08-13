using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Visualizer.MainMenu;
using UnityEngine;

namespace JumboJumps.EFTB.State.MainMenu
{
    public class MainMenuState : BaseState
    {
        private MainMenuVisualizer visualizer;

        public MainMenuState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(InGameState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            visualizer = new MainMenuVisualizer();
            visualizer.Initialize();
            visualizer.Show();

            visualizer.EventPlayUIButtonClicked += OnPlayButtonClicked;
            visualizer.EventExitUIButtonClicked += OnExitButtonClicked;

            visualizer.SetWorldObjectsAlpha(0f);
            visualizer.StartLogoIdleAnimation();
        }

        public override void OnExitState()
        {
            if (visualizer != null)
            {
                visualizer.EventPlayUIButtonClicked -= OnPlayButtonClicked;
                visualizer.EventExitUIButtonClicked -= OnExitButtonClicked;
                visualizer.Dispose();
                visualizer = null;
            }

            base.OnExitState();
        }

        public void OnPlayButtonClicked()
        {
            visualizer?.PlayStartSequence(() =>
            {
                visualizer?.FadeInWorldObjects(0.3f, () =>
                {
                    StateController.ChangeState(typeof(InGameState));
                });
            });
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
