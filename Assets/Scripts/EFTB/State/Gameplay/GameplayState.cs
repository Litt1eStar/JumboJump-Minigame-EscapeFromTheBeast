using JumboJumps.EFTB.Constant.Scene;
using JumboJumps.EFTB.State.Base;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class GameplayState : BaseLoadSceneState
    {
        protected override string SceneName => ConstScene.GAMEPLAY;

        private GameplayStateManager gameplayStateManager;
        private LevelGeneratorManager levelGeneratorManager;
        private PlayerManager playerManager;
        private CatManager catManager;
        private CollectibleManager collectibleManager;
        private ScoreManager scoreManager;
        private AggressiveCatSpawner aggressiveCatSpawner;
        private WarningIndicatorManager warningIndicatorManager;

        private GameplayController gameplayController;
        private GameStateController stateController;

        private Input2DManager input2DManager;
        private ObjectPoolManager poolManager;

        public GameplayState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (GameStateController)stateController;

            StateTransitionMap.Add(typeof(MainMenuState), null);
        }

        protected override void OnSceneLoadSucceeded()
        {
            base.OnSceneLoadSucceeded();

            input2DManager = SceneObjectContext.Instance.Get<Input2DManager>();
            input2DManager.Initialize();

            playerManager = new PlayerManager();
            playerManager.Initialize();

            catManager = new CatManager();
            IEnumerable<GICat> sceneCats = null;
            sceneCats = SceneObjectContext.Instance.GetAll<GICat>();
            
            catManager.Intialize(sceneCats, playerManager.PlayerTransform);  

            collectibleManager = new CollectibleManager();  
            collectibleManager.Initialize();

            scoreManager = new ScoreManager();
            scoreManager.Initialize(playerManager);

            gameplayController = new GameplayController();

            gameplayStateManager = new GameplayStateManager();
            gameplayStateManager.Initialize(gameplayController);

            gameplayController.Initialize(gameplayStateManager.StateController);
            gameplayController.EventReturnBackToMainMenu += ReturnBackToMainMenu;

            poolManager = new ObjectPoolManager();
            poolManager.Initialize();

            warningIndicatorManager = new WarningIndicatorManager();
            warningIndicatorManager.Initialize();

            levelGeneratorManager = new LevelGeneratorManager();
            levelGeneratorManager.Initialize(playerManager.PlayerTransform);

            aggressiveCatSpawner = new AggressiveCatSpawner();
            aggressiveCatSpawner.Initialize();

        }

        public override void OnEnterState()
        {
            base.OnEnterState();
        }

        public override void OnExitState()
        {
            base.OnExitState();

            scoreManager?.Dispose();
            scoreManager = null;

            gameplayStateManager?.Dispose();
            gameplayStateManager = null;

            aggressiveCatSpawner?.Dispose();
            aggressiveCatSpawner = null;

            warningIndicatorManager?.Dispose();
            warningIndicatorManager = null;

            playerManager?.Dispose();
            playerManager = null;

            levelGeneratorManager?.Dispose();
            levelGeneratorManager = null;

            poolManager?.Dispose();
            poolManager = null;

            catManager?.Dispose();
            catManager = null;

            collectibleManager?.Dispose();
            collectibleManager = null;

            if (input2DManager != null)
            {
                input2DManager.Dispose();
                input2DManager = null;
            }

            if (gameplayController != null)
            {
                gameplayController.EventReturnBackToMainMenu -= ReturnBackToMainMenu;
                gameplayController.Dispose();
                gameplayController = null;
            }
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            if (!IsSceneLoaded) return;

            gameplayStateManager?.UpdateLogic(deltaTime);
            playerManager?.UpdateLogic(deltaTime);
            levelGeneratorManager?.UpdateLogic(deltaTime);
            catManager?.UpdateLogic(deltaTime);
            aggressiveCatSpawner?.UpdateLogic(deltaTime);

            input2DManager.UpdateLogic(deltaTime);
        }

        public void ReturnBackToMainMenu()
        {
            stateController.ChangeState(typeof(MainMenuState));
        }
    }
}
