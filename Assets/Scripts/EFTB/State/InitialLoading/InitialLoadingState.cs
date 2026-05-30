using JumboJumps.EFTB.State;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Utilities;

namespace JumboJump.EFTB.State.InitialLoading
{
    public class InitialLoadingState : BaseState
    {
        private float transitionTime = 1f;
        private float timer;
        public InitialLoadingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(MainMenuState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            DebugLogHelper.Log("InitialLoadingState: Entered Initial Loading State");
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
                StateController.ChangeState(typeof(MainMenuState));
            }
        }
    }
}
