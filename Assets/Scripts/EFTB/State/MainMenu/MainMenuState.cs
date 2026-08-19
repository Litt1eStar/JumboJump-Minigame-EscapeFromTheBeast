using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Sound;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
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

            EFTBSound.PlayGameplayBGM();

            visualizer = new MainMenuVisualizer();
            visualizer.Initialize();
            visualizer.Show();

            GameplayStateController gameplayStateController = StateController as GameplayStateController;
            gameplayStateController?.GameplayVisualizer?.HidePanel();

            visualizer.EventPlayUIButtonClicked += OnPlayButtonClicked;
            visualizer.EventExitUIButtonClicked += OnExitButtonClicked;

            ResetGameplay();

            visualizer.SetWorldObjectsAlpha(0f);
            visualizer.StartLogoIdleAnimation();
        }

        private static void ResetGameplay()
        {
            GameContext.Instance?.Get<PlayerManager>()?.ResetPlayer();
            GameContext.Instance?.Get<CollectibleManager>()?.ResetValue();
            GameContext.Instance?.Get<ScoreManager>()?.ResetScore();
            GameContext.Instance?.Get<LevelGeneratorManager>()?.ResetLevel();
            GameContext.Instance?.Get<HazardSpawner>()?.ResetLevel();
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
