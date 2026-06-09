using UnityEngine;

namespace JumboJumps.EFTB.State.Gameplay
{
    public class PauseMenuState : BaseState
    {
        public PauseMenuState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(InGameState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            Time.timeScale = 0f;
        }

        public override void OnExitState()
        {
            Time.timeScale = 1f;

            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }
    }
}
