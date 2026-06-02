using JumboJumps.EFTB.Utilities;

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

            // For testing, automatically transition to FinishGameState after a short delay

            timer -= deltaTime;
            if(timer <= 0)
            {
                StateController.ChangeState(typeof(FinishGameState));
            }

            // In a real implementation, the transition to FinishGameState would be triggered by game events (e.g., player reaches the end of the level or loses all health)
            // The transition to PauseMenuState would be triggered by user input (e.g., pressing the "Pause" button)
        }
    }
}
