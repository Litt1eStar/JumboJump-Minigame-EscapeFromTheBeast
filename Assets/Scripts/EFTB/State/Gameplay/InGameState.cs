using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class InGameState : BaseState
    {
        private Input2DManager input2DManager;

        public InGameState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(FinishGameState), null);
            StateTransitionMap.Add(typeof(PauseMenuState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            input2DManager = GameContext.Instance.Get<Input2DManager>();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            input2DManager = null;
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            
            #if UNITY_EDITOR
            // For testing
            if (input2DManager.IsChangeState())
            {
                StateController.ChangeState(typeof(FinishGameState));
            }
            #endif

            // In a real implementation, the transition to FinishGameState would be triggered by game events (e.g., player reaches the end of the level or loses all health)
            // The transition to PauseMenuState would be triggered by user input (e.g., pressing the "Pause" button)
        }

    }
}
