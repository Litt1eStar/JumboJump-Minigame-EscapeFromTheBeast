using JumboJumps.EFTB.GameData;
using JumboJumps.EFTB.Manager;
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

        private GameManager gameManager;
        private LocalizationManager localizationManager;

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

            localizationManager = new LocalizationManager();
            localizationManager.Initialize(coroutineHelper);
        }

        private void Update()
        {
            gameManager.UpdateLogic(Time.deltaTime);
        }

        private void Dispose()
        {
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
    }
}
