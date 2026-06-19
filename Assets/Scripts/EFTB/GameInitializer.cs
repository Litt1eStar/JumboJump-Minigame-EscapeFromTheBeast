using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField]
        private CoroutineHelper coroutineHelper;

        private GameManager gameManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {            
            gameManager.UpdateLogic(Time.deltaTime);
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
        }

        private void Dispose()
        {
            if (coroutineHelper != null)
            {
                coroutineHelper.Dispose();
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
