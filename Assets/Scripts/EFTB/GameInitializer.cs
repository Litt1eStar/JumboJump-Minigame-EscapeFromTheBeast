using JumboJump.EFTB.Utilities;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
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
        private PlayerManager playerManager;
        private CatManager catManager;

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
            playerManager.UpdateLogic(Time.deltaTime);
            input2DManager.UpdateLogic(Time.deltaTime);
            catManager.UpdateLogic(Time.deltaTime);

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

            playerManager = new PlayerManager();
            playerManager.Initialize();

            catManager = new CatManager();
            IEnumerable<GICat> sceneCats = SceneObjectContext.Instance.GetAll<GICat>();
            catManager.Intialize(sceneCats, playerManager.PlayerTransform);

            collectibleManager = new CollectibleManager();
            collectibleManager.Initialize();

            input2DManager.Initialize();
            coroutineHelper.Initialize();            
        }

        private void Dispose()
        {
            if(input2DManager != null)
            {
                input2DManager.Dispose();
            }

            if(coroutineHelper != null)
            {
                coroutineHelper.Dispose();
            }

            gameManager?.Dispose();
            gameManager = null;

            playerManager?.Dispose();
            playerManager = null;

            catManager?.Dispose();
            catManager = null;

            collectibleManager?.Dispose();
            collectibleManager = null;
        }

        private void StartGame()
        {
            DebugLogHelper.Log($"{GetType().Name}: StartGame");
            gameManager?.StartGame();
        }
    }
}
