using JumboJump.EFTB.State.MainMenu;

namespace JumboJump.EFTB.State.InitialLoading
{
    public class InitialLoadingState : BaseState
    {
        public InitialLoadingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(MainMenuState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }
    }
}
