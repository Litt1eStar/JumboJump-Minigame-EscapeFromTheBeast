using JumboJumps.EFTB.Constant.UI;
using JumboJumps.EFTB.Manager;
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
        private Coroutine loadingCoroutine;
        private MiniHubManager miniHubManager;

        private float simulateDuration = ConstUI.Loading.SimulatedLoadingDuration;

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

            coroutineHelper = GameContext.Instance?.Get<CoroutineHelper>();
            miniHubManager = GameContext.Instance?.Get<MiniHubManager>();

            if (coroutineHelper != null)
            {
                loadingCoroutine = coroutineHelper.Play(LoadingRoutine());
            }
            else
            {
                DebugLogHelper.LogError($"[{GetType().Name}] CoroutineHelper missing from GameContext.");
                StateController.ChangeState(typeof(GameplayState));
            }
        }

        private IEnumerator LoadingRoutine()
        {
            float timer = 0f;
            float targetProgress = 0.3f;

            if (miniHubManager != null)
            {
                bool isAuthFinished = false;
                miniHubManager.GetParentAuthInfo(success =>
                {
                    isAuthFinished = true;
                });

                while (!isAuthFinished && timer < 5f)
                {
                    timer += Time.deltaTime;
                    float progress = Mathf.Clamp01(timer / 5f) * 0.3f;
                    visualizer?.SetProgress(progress);
                    yield return null;
                }

                targetProgress = 0.6f;
                bool isProfileFinished = false;
                miniHubManager.GetProfile(success =>
                {
                    isProfileFinished = true;
                });

                while (!isProfileFinished && timer < 10f)
                {
                    timer += Time.deltaTime;
                    float progress = 0.3f + (Mathf.Clamp01((timer - 0.3f) / 10f) * 0.3f);
                    visualizer?.SetProgress(progress);
                    yield return null;
                }

                if (miniHubManager.IsReady && miniHubManager.CachedProfile?.Profile?.LanguageCode != null)
                {
                    var localizationManager = GameContext.Instance?.Get<LocalizationManager>();
                    localizationManager?.ApplyLanguageCode(miniHubManager.CachedProfile.Profile.LanguageCode);
                }
            }

            // Fill remaining progress bar to 1.0f smoothly
            float fillTimer = 0f;
            float fillDuration = 0.5f;
            float startProgress = targetProgress;

            while (fillTimer < fillDuration)
            {
                fillTimer += Time.deltaTime;
                float progress = Mathf.Lerp(startProgress, 1.0f, fillTimer / fillDuration);
                visualizer?.SetProgress(progress);
                yield return null;
            }

            visualizer?.SetProgress(1.0f);
            yield return new WaitForSeconds(0.2f);

            StateController.ChangeState(typeof(GameplayState));
        }

        public override void OnExitState()
        {
            miniHubManager = null;
            visualizer?.Dispose();
            visualizer = null;
            if (coroutineHelper != null && loadingCoroutine != null)
            {
                coroutineHelper.Stop(loadingCoroutine);
                loadingCoroutine = null;
            }
            base.OnExitState();
        }
    }
}
