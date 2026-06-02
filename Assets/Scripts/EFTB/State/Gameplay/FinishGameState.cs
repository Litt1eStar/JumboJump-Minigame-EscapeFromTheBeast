using JumboJumps.EFTB.State.MainMenu;
using JumboJumps.EFTB.Utilities;

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

            DebugLogHelper.Log("FinishGameState: Entered Finish Game State");

            timer = transitionTime;
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            
            // For testing, automatically transition to MainMenuState after a short delay
            // In a real implementation, the transition to MainMenuState would be triggered by user input (e.g., player click MainMenu Button)

            timer -= deltaTime;
            if(timer <= 0)
            {
                
            }
        }
    }
}
