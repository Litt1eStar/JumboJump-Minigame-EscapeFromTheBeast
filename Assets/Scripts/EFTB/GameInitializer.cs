using JumboJump.EFTB.Utilities;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField]
        private Input2DManager input2DManager;

        [SerializeField]
        private CoroutineHelper coroutineHelper;

        private GameManager gameManager;
        private CollectibleManager collectibleManager;

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
            input2DManager.UpdateLogic(Time.deltaTime);
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

            input2DManager.Initialize();
            coroutineHelper.Initialize();
        }

        private void Dispose()
        {
            if (input2DManager != null)
            {
                input2DManager.Dispose();
            }

            if (coroutineHelper != null)
            {
                coroutineHelper.Dispose();
            }

            gameManager?.Dispose();
            gameManager = null;
        }

        private void StartGame()
        {
            DebugLogHelper.Log($"{GetType().Name}: StartGame");
            gameManager?.StartGame();
        }
    }
}
