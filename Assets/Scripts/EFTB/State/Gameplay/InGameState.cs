using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class InGameState : BaseState
    {
        private GameplayStateController stateController;

        private PlayerManager playerManager => GameContext.Instance.Get<PlayerManager>();
        private ScoreManager scoreManager => GameContext.Instance.Get<ScoreManager>();
        private LevelGeneratorManager levelGeneratorManager => GameContext.Instance.Get<LevelGeneratorManager>();
        private CatManager catManager => GameContext.Instance.Get<CatManager>();
        private HazardSpawner hazardSpawner => GameContext.Instance.Get<HazardSpawner>();
        private Input2DManager input2DManager => SceneObjectContext.Instance.Get<Input2DManager>();

        public InGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(FinishGameState), null);
            StateTransitionMap.Add(typeof(PauseMenuState), null);

            this.stateController = (GameplayStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            stateController.GameplayVisualizer.EventPauseUIButtonClicked += OnClickPauseButton;
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
