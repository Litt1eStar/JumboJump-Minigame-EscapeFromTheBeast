using JumboJumps.EFTB.State.MainMenu;

namespace JumboJumps.EFTB.State.InitialLoading
{
    public class InitialLoadingState : BaseState
    {
        private GameStateController stateController;
        private float transitionTime = 1f;
        private float timer;

        public InitialLoadingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(MainMenuState), null);
            this.stateController = (GameStateController)stateController;
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
            //For testing purpose
            timer -= deltaTime;

            if (timer <= 0f)
            {
                StateController.ChangeState(typeof(MainMenuState));
            }

#endif
        }

    }
}
