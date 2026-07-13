using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class AggressiveCatSpawner
    {
        private float minSpawnTime;
        private float maxSpawnTime;
        private float verticalSpawnOffset;

        private ObjectPoolManager poolManager;
        private LevelGeneratorManager levelGeneratorManager;
        private PlayerManager playerManager;
        private CatManager catManager;
        private GameDataManager gameDataManager;
        private GameplayStateManager gameplayStateManager;

        private float nextSpawnTimer;

        public void Initialize()
        {
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
            playerManager = GameContext.Instance.Get<PlayerManager>();
            catManager = GameContext.Instance.Get<CatManager>();
            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();

            ResetSpawnTimer();

            minSpawnTime = ConstGameplay.Cat.AggressiveCat.INITIAL_MIN_SPAWN_TIME;
            maxSpawnTime = ConstGameplay.Cat.AggressiveCat.INITIAL_MAX_SPAWN_TIME;
            verticalSpawnOffset = ConstGameplay.Cat.AggressiveCat.CAT_VERTICAL_SPAWN_OFFSET;

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            nextSpawnTimer -= deltaTime;
            if (nextSpawnTimer <= 0f)
            {
                ResetSpawnTimer();
                SpawnAggressiveCat();
            }
        }

        private void ResetSpawnTimer()
        {
            nextSpawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        private void SpawnAggressiveCat()
        {
            if (playerManager?.PlayerTransform == null || levelGeneratorManager == null || poolManager == null || gameDataManager == null || catManager == null)
            {
                return;
            }

            float playerY = playerManager.PlayerTransform.position.y;
            float spawnY = playerY + verticalSpawnOffset;

            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            if (giSegment == null)
            {
                DebugLogHelper.LogWarning($"[AggressiveCatSpawner] Cannot spawn AggressiveCat: No active segment found at Y = {spawnY}");
                return;
            }

            float targetX = Random.value < 0.5f ? ConstGameplay.Cat.CAT_LEFT_LANE_SPAWN_POSITION : ConstGameplay.Cat.CAT_RIGHT_LANE_SPAWN_POSITION;
            Vector3 spawnPosition = new Vector3(targetX, spawnY, 0f);

            if (!gameDataManager.TryGetPrefab(ConstGameplay.Cat.AggressiveCat.PREFAB_NAME, out GameObject prefab))
            {
                DebugLogHelper.LogError($"[AggressiveCatSpawner] Cannot spawn AggressiveCat: Prefab, '{ConstGameplay.Cat.AggressiveCat.PREFAB_NAME}' not found in registry.");
                return;
            }

            GameObject catGo = poolManager.Spawn(prefab, spawnPosition, Quaternion.identity, giSegment.transform);
            if (catGo == null)
            {
                DebugLogHelper.LogError($"[AggressiveCatSpawner] Failed to spawn AggressiveCat at position {spawnPosition}.");
                return;
            }

            giSegment.RegisterSpawnedObject(catGo);

            var giCat = catGo.GetComponent<GICat>();
            if (giCat != null)
            {
                SceneObjectContext.Instance.Register(giCat);

                CatSightDirection direction = (targetX < 0f) ? CatSightDirection.Right : CatSightDirection.Left;
                giCat.SetDirection(direction);

                catManager.RegisterDynamicCat(giCat, playerManager.PlayerTransform);
            }
            else
            {
                DebugLogHelper.LogError("[AggressiveCatSpawner] Spawned cat GameObject is missing GICat component!");
            }
        }
    }
}
