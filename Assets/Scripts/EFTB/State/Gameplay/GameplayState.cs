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
        private PlayerManager playerManager;
        private CatManager catManager;
        private CollectibleManager collectibleManager;

        private GameplayController gameplayController;
        private GameStateController stateController;

        public GameplayState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (GameStateController)stateController;

            StateTransitionMap.Add(typeof(MainMenuState), null);
        }

        protected override void OnSceneLoadSucceeded()
        {
            base.OnSceneLoadSucceeded();

            playerManager = new PlayerManager();
            playerManager.Initialize();

            catManager = new CatManager();
            IEnumerable<GICat> sceneCats = SceneObjectContext.Instance.GetAll<GICat>();
            catManager.Intialize(sceneCats, playerManager.PlayerTransform);

            collectibleManager = new CollectibleManager();
            collectibleManager.Initialize();

            gameplayStateManager = new GameplayStateManager();
            gameplayStateManager.Initialize(gameplayController);

            gameplayController = new GameplayController();
            gameplayController.Initialize(gameplayStateManager.StateController);
            gameplayController.EventReturnBackToMainMenu += ReturnBackToMainMenu;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
        }

        public override void OnExitState()
        {
            base.OnExitState();

            gameplayStateManager?.Dispose();
            gameplayStateManager = null;

            playerManager?.Dispose();
            playerManager = null;

            catManager?.Dispose();
            catManager = null;

            collectibleManager?.Dispose();
            collectibleManager = null;

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

            gameplayStateManager?.UpdateLogic(deltaTime);
            playerManager?.UpdateLogic(deltaTime);
            catManager?.UpdateLogic(deltaTime);
        }

        public void ReturnBackToMainMenu()
        {
            stateController.ChangeState(typeof(MainMenuState));
        }
    }
}
