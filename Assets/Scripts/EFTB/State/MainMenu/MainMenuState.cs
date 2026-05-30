using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.MainMenu
{
    public class MainMenuState : BaseState
    {
        private float transitionTime = 1f;
        private float timer;
        public MainMenuState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(GameplayState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            DebugLogHelper.Log("MainMenuState: Entered Main Menu State");
            timer = transitionTime;
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            timer -= deltaTime;

            if (timer <= 0f) 
            {
                StateController.ChangeState(typeof(GameplayState));
            }
        }
    }
}
