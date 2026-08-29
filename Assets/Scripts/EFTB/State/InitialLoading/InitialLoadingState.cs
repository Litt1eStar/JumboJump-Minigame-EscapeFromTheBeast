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
        private CoroutineHelper coroutineHelper;
        private Coroutine loadingCoroutine;
        private MiniHubManager miniHubManager;

        public InitialLoadingState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(GameplayState), null);
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
            visualizer?.SetProgress(0.0f);

            if (miniHubManager != null)
            {
                bool isFinished = false;

                miniHubManager.GetParentAuthInfo(isAuthSuccess =>
                {
                    if (isAuthSuccess)
                    {
                        miniHubManager.GetProfile(isProfileSuccess =>
                        {
                            isFinished = true;
                        });
                    }
                    else
                    {
                        isFinished = true;
                    }
                });

                yield return new WaitUntil(() => isFinished);

                if (miniHubManager.IsReady && miniHubManager.CachedProfile?.Profile?.LanguageCode != null)
                {
                    var localizationManager = GameContext.Instance?.Get<LocalizationManager>();
                    localizationManager?.ApplyLanguageCode(miniHubManager.CachedProfile.Profile.LanguageCode);
                }
            }

            visualizer?.SetProgress(1.0f);
            yield return new WaitForSeconds(ConstUI.Loading.LoadingFinishDelay);

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
