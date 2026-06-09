using JumboJumps.EFTB.State;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumboJumps.EFTB.State.Base
{
    public abstract class BaseLoadSceneState : BaseState
    {
        private LoadSceneMode LOAD_SCENE_MODE => LoadSceneMode.Single;
        protected abstract string SceneName { get; }

        protected bool IsSceneLoaded;
        protected CoroutineHelper CoroutineHelper;
        private Coroutine loadSceneCoroutine;
        
        public BaseLoadSceneState(BaseStateController stateController) : base(stateController)
        {
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            IsSceneLoaded = false;

            CoroutineHelper = GameContext.Instance.Get<CoroutineHelper>();

            loadSceneCoroutine = CoroutineHelper.StartCoroutine(LoadSceneRoutine());
        }

        public override void OnExitState()
        {
            if(CoroutineHelper != null)
            {
                CoroutineHelper.Stop(loadSceneCoroutine);

                CoroutineHelper = null;
                loadSceneCoroutine = null;
            }

            base.OnExitState();
        }

        private IEnumerator LoadSceneRoutine()
        {
            var activeScene = SceneManager.GetActiveScene();

            if (activeScene.name == SceneName)
            {
                while (!activeScene.isLoaded)
                {
                    yield return null;
                }

                OnSceneLoadSucceeded();

                yield break;
            }

            AsyncOperation handle;

            try
            {
                handle = SceneManager.LoadSceneAsync(SceneName, LOAD_SCENE_MODE);
            }
            catch (Exception e)
            {
                DebugLogHelper.LogError($"{nameof(LoadSceneRoutine)} | Fail to load scene with exception : {e}");
                yield break;
            }

            yield return new WaitUntil(() => handle.isDone);

            OnSceneLoadSucceeded();
        }

        protected virtual void OnSceneLoadSucceeded()
        {
            IsSceneLoaded = true;
        }
    }
}
