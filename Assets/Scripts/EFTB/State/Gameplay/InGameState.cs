namespace JumboJumps.EFTB.State.Gameplay
{
    public class InGameState : BaseState
    {
        //For Testing Game State Transition
        private float transitionTime = 3f;
        private float timer;
        public InGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(FinishGameState), null);
            StateTransitionMap.Add(typeof(PauseMenuState), null);
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
                StateController.ChangeState(typeof(FinishGameState));
                //PauseMenuState also working
            }
        }
    }
}
