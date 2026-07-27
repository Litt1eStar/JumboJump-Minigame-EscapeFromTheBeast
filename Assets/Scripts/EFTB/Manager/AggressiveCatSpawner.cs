using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Model;
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
        private WarningIndicatorManager warningIndicatorManager;

        private float nextSpawnTimer;

        public void Initialize()
        {
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
            playerManager = GameContext.Instance.Get<PlayerManager>();
            catManager = GameContext.Instance.Get<CatManager>();
            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            warningIndicatorManager = GameContext.Instance.Get<WarningIndicatorManager>();  

            ResetSpawnTimer();

            minSpawnTime = ConstGameplay.Cat.AggressiveCat.Initial_Min_Spawn_Time;
            maxSpawnTime = ConstGameplay.Cat.AggressiveCat.Initial_Max_Spawn_Time;
            verticalSpawnOffset = ConstGameplay.Cat.AggressiveCat.Cat_Vertical_Spawn_Offset;

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

            UpdateSpawnTimes();

            nextSpawnTimer -= deltaTime;
            if (nextSpawnTimer <= 0f)
            {
                float spawnY;
                if (CanSpawnAggressiveCat(out spawnY))
                {
                    ResetSpawnTimer();
                    
                    int sideIndex = (Random.value < 0.5f) ? 0 : 1; 
                    
                    if (warningIndicatorManager != null)
                    {
                        warningIndicatorManager.ShowCatEventWarning(ConstGameplay.Cat.AggressiveCat.Event_Warning_Duration, () =>
                        {
                            warningIndicatorManager.ShowCatDirectionWarning(sideIndex, ConstGameplay.Cat.AggressiveCat.Direction_Warning_Duration, () =>
                            {
                                SpawnAggressiveCat(sideIndex);
                            });
                        });
                    }
                    else
                    {
                        SpawnAggressiveCat(sideIndex);
                    }
                }
                else
                {
                    nextSpawnTimer = ConstGameplay.Cat.AggressiveCat.Fallback_Spawn_Check_Interval;
                }
            }
        }

        private void UpdateSpawnTimes()
        {
            var timeManager = GameContext.Instance.Get<GameplayTimeManager>();
            if (timeManager == null) return;

            switch (timeManager.CurrentDifficulty)
            {
                case GameplayDifficultyEnum.Easy:
                {
                    minSpawnTime = ConstGameplay.Cat.AggressiveCat.Initial_Min_Spawn_Time;
                    maxSpawnTime = ConstGameplay.Cat.AggressiveCat.Initial_Max_Spawn_Time;
                    break;
                }
                case GameplayDifficultyEnum.Normal:
                {
                    minSpawnTime = ConstGameplay.Cat.AggressiveCat.Normal_Min_Spawn_Time;
                    maxSpawnTime = ConstGameplay.Cat.AggressiveCat.Normal_Max_Spawn_Time;
                    break;
                }
                case GameplayDifficultyEnum.Hard:
                {
                    minSpawnTime = ConstGameplay.Cat.AggressiveCat.Hard_Min_Spawn_Time;
                    maxSpawnTime = ConstGameplay.Cat.AggressiveCat.Hard_Max_Spawn_Time;
                    break;
                }
            }
        }

        private void ResetSpawnTimer()
        {
            nextSpawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        private bool CanSpawnAggressiveCat(out float spawnY)
        {
            spawnY = 0f;
            if (playerManager?.PlayerTransform == null || levelGeneratorManager == null || poolManager == null || gameDataManager == null || catManager == null)
            {
                return false;
            }

            float playerY = playerManager.PlayerTransform.position.y;
            spawnY = playerY + verticalSpawnOffset;

            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            return giSegment != null;
        }

        private void SpawnAggressiveCat(int sideIndex)
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                DebugLogHelper.LogWarning($"[AggressiveCatSpawner] Cannot spawn AggressiveCat: Not in InGameState.");
                return;
            }

            float spawnY;
            if (!CanSpawnAggressiveCat(out spawnY))
            {
                DebugLogHelper.LogWarning($"[AggressiveCatSpawner] Cannot spawn AggressiveCat: Environment state changed during the warning delay.");
                return;
            }

            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            if (giSegment == null) return;

            float targetX = (sideIndex == 0) ? ConstGameplay.Cat.Cat_Left_Lane_Spawn_Position : ConstGameplay.Cat.Cat_Right_Lane_Spawn_Position;
            Vector3 spawnPosition = new Vector3(targetX, spawnY, 0f);

            GameObject prefab = gameDataManager.GetPrefab(ConstGameplay.Cat.AggressiveCat.Prefab_Name);
            if (prefab == null)
            {
                DebugLogHelper.LogError($"[AggressiveCatSpawner] Cannot spawn AggressiveCat: Prefab, '{ConstGameplay.Cat.AggressiveCat.Prefab_Name}' not found in registry.");
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
