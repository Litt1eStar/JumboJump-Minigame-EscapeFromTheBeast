using JumboJumps.EFTB.Constant.UI;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.InitialLoading;
using System.Collections;
using UnityEngine;

namespace JumboJumps.EFTB.State.InitialLoading
{
    public class InitialLoadingState : BaseState
    {
        private InitialLoadingVisualizer visualizer;
        private GameStateController stateController;
        private CoroutineHelper coroutineHelper;
        private Coroutine loadingProgressCoroutine;

        private float simulateDuration = ConstUI.Loading.SIMULATED_LOADING_DURATION; // Simulated loading duration in seconds

        public InitialLoadingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(GameplayState), null);

            this.stateController = (GameStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            visualizer = new InitialLoadingVisualizer();
            visualizer.Initialize();
            visualizer.Show();

            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();

            #if UNITY_EDITOR
                loadingProgressCoroutine = coroutineHelper.Play(SimulatedLoadingRoutine());
            #else 
                // In Real game, do real loading pre-load assets
                StateController.ChangeState(typeof(GameplayState));
            #endif
        }

        private IEnumerator SimulatedLoadingRoutine()
        {
            float timer = 0f;

            while (timer < simulateDuration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / simulateDuration);
                visualizer.SetProgress(progress);

                yield return null;
            }

            visualizer.SetProgress(1.0f);

            yield return new WaitForSeconds(0.5f);

            StateController.ChangeState(typeof(GameplayState));
        }

        public override void OnExitState()
        {
            visualizer?.Dispose();
            visualizer = null;
            coroutineHelper.StopAllCoroutines();
            base.OnExitState();
        }
    }
}
