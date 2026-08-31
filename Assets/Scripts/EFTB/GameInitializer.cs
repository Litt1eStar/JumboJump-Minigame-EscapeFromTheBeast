using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Plugins;
using JumboJumps.EFTB.UI;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField]
        private CoroutineHelper coroutineHelper;

        [SerializeField]
        private GameDataManager gameDataManager;

        [SerializeField]
        private MiniHubBridge miniHubBridge;

        [SerializeField]
        private UIInitializer uiInitializer;

        private GameManager gameManager;
        private LocalizationManager localizationManager;
        private MiniHubManager miniHubManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Start()
        {
            StartGame();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Initialize()
        {
            gameManager = new GameManager();
            gameManager.Initialize();

            coroutineHelper.Initialize();
            gameDataManager.Initialize();
            uiInitializer?.Initialize();

            localizationManager = new LocalizationManager();
            localizationManager.Initialize(coroutineHelper);

            miniHubManager = new MiniHubManager();
            miniHubManager.Initialize(miniHubBridge);
        }

        private void Update()
        {
            gameManager.UpdateLogic(Time.deltaTime);
        }

        private void Dispose()
        {
            if (uiInitializer != null)
               {
                uiInitializer.Dispose();
                uiInitializer = null;
            }

            if (miniHubManager != null)
            {
                miniHubManager.Dispose();
                miniHubManager = null;
            }

            if (localizationManager != null)
            {
                localizationManager.Dispose();
                localizationManager = null;
            }

            if (coroutineHelper != null)
            {
                coroutineHelper.Dispose();
            }

            if (gameDataManager != null)
            {
                gameDataManager.Dispose();
            }

            gameManager?.Dispose();
            gameManager = null;
        }

        private void StartGame()
        {
            gameManager?.StartGame();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SetAppBackgroundAudioSuspended(pauseStatus);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetAppBackgroundAudioSuspended(!hasFocus);
        }

        public void OnWebAppSuspended()
        {
            SetAppBackgroundAudioSuspended(true);
        }

        public void OnWebAppResumed()
        {
            SetAppBackgroundAudioSuspended(false);
        }

        private void SetAppBackgroundAudioSuspended(bool isSuspended)
        {
            var soundManager = GameContext.Instance?.Get<SoundManager>();
            soundManager?.SetAppBackgroundAudioSuspended(isSuspended);
        }
    }
}
