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
        private MiniHubManager miniHubManager;
        private bool isStartingSession;

        public MainMenuState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(InGameState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            miniHubManager = GameContext.Instance?.Get<MiniHubManager>();
            isStartingSession = false;

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
            miniHubManager = null;
            isStartingSession = false;

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
            if (isStartingSession) return;

            if (miniHubManager == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] MiniHubManager not found in GameContext.");
                return;
            }

            isStartingSession = true;

            miniHubManager.StartGameSession((isSuccess, response, error) =>
            {
                isStartingSession = false;
                if (isSuccess)
                {
                    DebugLogHelper.Log($"[{GetType().Name}] MiniHub StartGameSession approved! SessionId: {response?.SessionId}");
                    visualizer?.PlayStartSequence(() =>
                    {
                        visualizer?.FadeInWorldObjects(0.3f, () =>
                        {
                            StateController.ChangeState(typeof(InGameState));
                        });
                    });
                }
                else
                {
                    DebugLogHelper.LogError($"[{GetType().Name}] Failed to start game session: {error}");
                }
            });
        }

        public void OnExitButtonClicked()
        {
            if (miniHubManager != null)
            {
                miniHubManager.CloseGame();
            }
            else
            {
                DebugLogHelper.Log($"[{GetType().Name}] Application.Quit called.");
                Application.Quit();
            }
        }
    }
}
