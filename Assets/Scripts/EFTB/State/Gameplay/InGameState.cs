using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class InGameState : BaseState
    {
        private GameplayStateController stateController;

        private PlayerManager playerManager;
        private ScoreManager scoreManager;
        private LevelGeneratorManager levelGeneratorManager;
        private CatManager catManager;
        private HazardSpawner hazardSpawner;
        private Input2DManager input2DManager;

        public InGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(FinishGameState), null);
            StateTransitionMap.Add(typeof(PauseMenuState), null);
            StateTransitionMap.Add(typeof(MainMenuState), null);

            this.stateController = (GameplayStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            playerManager = GameContext.Instance?.Get<PlayerManager>();
            scoreManager = GameContext.Instance?.Get<ScoreManager>();
            levelGeneratorManager = GameContext.Instance?.Get<LevelGeneratorManager>();
            catManager = GameContext.Instance?.Get<CatManager>();
            hazardSpawner = GameContext.Instance?.Get<HazardSpawner>();
            input2DManager = SceneObjectContext.Instance?.Get<Input2DManager>();

            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.ShowGameplayCanvas();
                stateController.GameplayVisualizer.EventPauseUIButtonClicked += OnClickPauseButton;
            }
        }

        public override void OnExitState()
        {
            base.OnExitState();

            if (stateController?.GameplayVisualizer != null)
            {
                stateController.GameplayVisualizer.EventPauseUIButtonClicked -= OnClickPauseButton;
            }
        }

        public void OnClickPauseButton()
        {
            stateController.GameplayVisualizer.ShowPauseMenu();
            stateController.ChangeState(typeof(PauseMenuState));
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            input2DManager?.UpdateLogic(deltaTime);
            playerManager?.UpdateLogic(deltaTime);
            scoreManager?.UpdateLogic(deltaTime);
            levelGeneratorManager?.UpdateLogic(deltaTime);
            catManager?.UpdateLogic(deltaTime);
            hazardSpawner?.UpdateLogic(deltaTime);
        }
    }
}
