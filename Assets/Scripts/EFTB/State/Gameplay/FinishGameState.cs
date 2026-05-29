using JumboJump.EFTB.State.MainMenu;

namespace JumboJump.Assets.Scripts.EFTB.State.Gameplay
{
    public class FinishGameState : BaseState
    {
        public FinishGameState(BaseStateController stateController) : base(stateController)
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
