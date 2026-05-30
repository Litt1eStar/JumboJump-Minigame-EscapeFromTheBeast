using JumboJumps.EFTB.State.MainMenu;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class FinishGameState : BaseState
    {
        private float transitionTime = 3f;
        private float timer;
        public FinishGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(MainMenuState), null);
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
            timer -= deltaTime;
            if(timer <= 0)
            {
                StateController.ChangeState(typeof(MainMenuState));
            }
        }
    }
}
