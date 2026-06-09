using JumboJumps.EFTB.Constant.Scene;
using JumboJumps.EFTB.State.Base;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.MainMenu;
using UnityEngine;

namespace JumboJumps.EFTB.State.MainMenu
{
    public class MainMenuState : BaseLoadSceneState
    {
        protected override string SceneName => ConstScene.MAIN_MENU;

        private MainMenuVisualizer visualizer;

        private float transitionTime = 1f;
        private float timer;

        public MainMenuState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(GameplayState), null);
        }

        protected override void OnSceneLoadSucceeded()
        {
            base.OnSceneLoadSucceeded();

            visualizer = new MainMenuVisualizer();
            visualizer.Initialize();
            visualizer.Subscribe(OnPlayButtonClicked, OnExitButtonClicked);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
        }

        public override void OnExitState()
        {
            visualizer?.Dispose();
            visualizer = null;

            base.OnExitState();
        }

        public void OnPlayButtonClicked()
        {
            StateController.ChangeState(typeof(GameplayState));
        }   

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
