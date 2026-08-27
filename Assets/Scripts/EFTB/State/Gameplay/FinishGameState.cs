using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class FinishGameState : BaseState
    {
        private GameplayStateController stateController;
        private GameplayController gameplayController;
        private MiniHubManager miniHubManager;

        public FinishGameState(BaseStateController stateController, GameplayController gameplayController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(MainMenuState), null);

            this.stateController = (GameplayStateController)stateController;
            this.gameplayController = gameplayController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            miniHubManager = GameContext.Instance?.Get<MiniHubManager>();

            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.HideGameplayCanvas();
                stateController.GameplayVisualizer.EventFinishMainMenuButtonClicked += OnFinishMainMenuButtonClicked;
            }

            int finalScore = GameContext.Instance?.Get<ScoreManager>()?.CurrentScoreData.TotalScore ?? 0;

            if (miniHubManager == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] MiniHubManager not found in GameContext during FinishGameState.");
                return;
            }

            miniHubManager.EndGameSession(finalScore, (isSuccess, response, error) =>
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
            miniHubManager = null;

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
