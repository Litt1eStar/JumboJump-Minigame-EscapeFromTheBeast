using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using System.Collections;
using UnityEngine;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerMovingState : BaseState
    {
        private PlayerStateController playerStateController => (PlayerStateController)StateController;
        private PlayerVisualizer playerVisualizer => playerStateController.Visualizer;

        private CoroutineHelper coroutineHelper;
        private Coroutine stepCoroutine;

        public PlayerMovingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(PlayerIdleState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            stepCoroutine = coroutineHelper.Restart(stepCoroutine, DiscreteStepForwardRoutine());
        }

        private IEnumerator DiscreteStepForwardRoutine()
        {
            Vector3 startPos = playerVisualizer.PlayerPosition;
            Vector3 targetPos = startPos + new Vector3(0f, ConstGameplay.Player.Step_Distance_Y, 0f);
            float elapsed = 0f;
            float duration = ConstGameplay.Player.Step_Duration;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
                playerVisualizer.SetPosition(currentPos);
                yield return null;
            }

            playerVisualizer.SetPosition(targetPos);
            StateController.ChangeState(typeof(PlayerIdleState));
        }

        public override void OnExitState()
        {
            if (coroutineHelper != null)
            {
                coroutineHelper.Stop(stepCoroutine);
                coroutineHelper = null;
                stepCoroutine = null;
            }

            base.OnExitState();
        }
    }
}
