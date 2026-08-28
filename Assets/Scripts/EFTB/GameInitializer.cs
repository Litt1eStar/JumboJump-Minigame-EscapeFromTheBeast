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
        private SoundManager soundManager;

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
            soundManager = new SoundManager();
            soundManager.Initialize();

            gameManager = new GameManager();
            gameManager.Initialize();

            coroutineHelper.Initialize();
            gameDataManager.Initialize();
        }

        private void Update()
        {
            gameManager.UpdateLogic(Time.deltaTime);
        }

        private void Dispose()
        {
            if (soundManager != null)
            {
                soundManager.Dispose();
                soundManager = null;
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
