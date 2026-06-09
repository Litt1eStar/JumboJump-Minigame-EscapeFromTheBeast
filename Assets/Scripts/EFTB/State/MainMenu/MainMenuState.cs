using JumboJumps.EFTB.Constant.Scene;
using JumboJumps.EFTB.State.Base;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.MainMenu
{
    public class MainMenuState : BaseLoadSceneState
    {
        protected override string SceneName => ConstScene.MAIN_MENU;

        private float transitionTime = 1f;
        private float timer;

        public MainMenuState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(GameplayState), null);
        }

        protected override void OnSceneLoadSucceeded()
        {
            base.OnSceneLoadSucceeded();

            DebugLogHelper.Log("MainMenu : OnSceneLoadSucceeded");
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = transitionTime;
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            
#if UNITY_EDITOR
            // For testing, automatically transition to GameplayState after a short delay
            // In a real implementation, this would be triggered by user input (e.g., pressing "Start Game")

            timer -= deltaTime;

            if (timer <= 0f) 
            {
                StateController.ChangeState(typeof(GameplayState));
            }
#endif
        }

    }
}
