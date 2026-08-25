using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Plugins;
using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class FinishGameState : BaseState
    {
        private GameplayStateController stateController;
        private GameplayController gameplayController;

        public FinishGameState(BaseStateController stateController, GameplayController gameplayController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(MainMenuState), null);

            this.stateController = (GameplayStateController)stateController;
            this.gameplayController = gameplayController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.EventFinishMainMenuButtonClicked += OnFinishMainMenuButtonClicked;
            }

            int finalScore = GameContext.Instance?.Get<ScoreManager>()?.CurrentScoreData.TotalScore ?? 0;
            var miniHubBridge = GameContext.Instance?.Get<MiniHubBridge>();
            if (miniHubBridge == null)
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] MiniHubBridge not found in GameContext during FinishGameState. Creating fallback instance...");
                var bridgeGo = new UnityEngine.GameObject("MiniHubBridge");
                UnityEngine.Object.DontDestroyOnLoad(bridgeGo);
                miniHubBridge = bridgeGo.AddComponent<MiniHubBridge>();
                GameContext.Instance?.Add(miniHubBridge);
            }

            miniHubBridge.EndGameSession(finalScore, (isSuccess, response, error) =>
            {
                if (isSuccess)
                {
                    DebugLogHelper.Log($"[{GetType().Name}] Submitted score to platform: {finalScore}");
                }
                else
                {
                    DebugLogHelper.LogError($"[{GetType().Name}] Failed to submit score: {error}");
                }
            });
        }

        public override void OnExitState()
        {
            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.HidePanel();
                stateController.GameplayVisualizer.EventFinishMainMenuButtonClicked -= OnFinishMainMenuButtonClicked;
            }

            base.OnExitState();
        }

        public void OnFinishMainMenuButtonClicked()
        {
            gameplayController?.ReturnToMainMenu();
        }
    }
}
