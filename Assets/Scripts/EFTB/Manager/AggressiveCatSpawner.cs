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
        private ObjectPoolManager poolManager;
        private LevelGeneratorManager levelGeneratorManager;
        private PlayerManager playerManager;
        private CatManager catManager;
        private GameDataManager gameDataManager;
        private GameplayStateManager gameplayStateManager;
        private WarningIndicatorManager warningIndicatorManager;

        private bool isAntiCampWarningActive;
        private float nextSpawnTimer;
        private float minSpawnTime;
        private float maxSpawnTime;
        private float verticalSpawnOffset;

        public void Initialize()
        {
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
            playerManager = GameContext.Instance.Get<PlayerManager>();
            catManager = GameContext.Instance.Get<CatManager>();
            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            warningIndicatorManager = GameContext.Instance.Get<WarningIndicatorManager>();

            if (playerManager != null)
            {
                playerManager.EventIdleLimitExceeded += OnPlayerIdleLimitExceeded;
            }

            ResetSpawnTimer();
            minSpawnTime = ConstGameplay.Cat.AggressiveCat.INITIAL_MIN_SPAWN_TIME;
            maxSpawnTime = ConstGameplay.Cat.AggressiveCat.INITIAL_MAX_SPAWN_TIME;
            verticalSpawnOffset = ConstGameplay.Cat.AggressiveCat.CAT_VERTICAL_SPAWN_OFFSET;

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            if (playerManager != null)
            {
                playerManager.EventIdleLimitExceeded -= OnPlayerIdleLimitExceeded;
            }

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
                if (!CanSpawnTimerCat(out float spawnY))
                {
                    nextSpawnTimer = ConstGameplay.Cat.AggressiveCat.NEXT_SPAWN_TIMER;
                    return;
                }

                ResetSpawnTimer();
                AggressiveCatSpawnSequence(spawnY);
            }
        }

        private void OnPlayerIdleLimitExceeded()
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            if (isAntiCampWarningActive) return;

            if (playerManager?.PlayerTransform == null) return;
            Vector3 cachedTargetPos = playerManager.PlayerTransform.position;

            TriggerAlphaCatPounceSequence(cachedTargetPos);
        }

        private void TriggerAlphaCatPounceSequence(Vector3 cachedTargetPos)
        {
            isAntiCampWarningActive = true;
            int sideIndex = (Random.value < 0.5f) ? 0 : 1;
            float warningDuration = ConstGameplay.Cat.AggressiveCat.POUNCE_WARNING_DURATION;

            if (playerManager != null)
            {
                playerManager.TriggerPounceWarning(warningDuration, () =>
                {
                    isAntiCampWarningActive = false;
                    SpawnAggressiveCatAtPosition(sideIndex, cachedTargetPos);
                });
            }
            else
            {
                isAntiCampWarningActive = false;
            }
        }

        private void AggressiveCatSpawnSequence(float spawnY)
        {
            int sideIndex = (Random.value < 0.5f) ? 0 : 1; 
            
            warningIndicatorManager?.ShowCatEventWarning(ConstGameplay.Cat.AggressiveCat.EVENT_WARNING_DURATION, () =>
            {
                warningIndicatorManager?.ShowCatDirectionWarning(sideIndex, ConstGameplay.Cat.AggressiveCat.DIRECTION_WARNING_DURATION, () =>
                {
                    SpawnAggressiveCatOnRow(sideIndex, spawnY);
                });
            });
        }

        private void UpdateSpawnTimes()
        {
            var timeManager = GameContext.Instance.Get<GameplayTimeManager>();
            if (timeManager == null) return;

            switch (timeManager.CurrentDifficulty)
            {
                case GameplayDifficultyEnum.Easy:
                    minSpawnTime = ConstGameplay.Cat.AggressiveCat.INITIAL_MIN_SPAWN_TIME;
                    maxSpawnTime = ConstGameplay.Cat.AggressiveCat.INITIAL_MAX_SPAWN_TIME;
                    break;
                case GameplayDifficultyEnum.Normal:
                    minSpawnTime = ConstGameplay.Cat.AggressiveCat.NORMAL_MIN_SPAWN_TIME;
                    maxSpawnTime = ConstGameplay.Cat.AggressiveCat.NORMAL_MAX_SPAWN_TIME;
                    break;
                case GameplayDifficultyEnum.Hard:
                    minSpawnTime = ConstGameplay.Cat.AggressiveCat.HARD_MIN_SPAWN_TIME;
                    maxSpawnTime = ConstGameplay.Cat.AggressiveCat.HARD_MAX_SPAWN_TIME;
                    break;
            }
        }

        private void ResetSpawnTimer()
        {
            nextSpawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        private bool CanSpawnTimerCat(out float spawnY)
        {
            spawnY = 0f;
            if (playerManager?.PlayerTransform == null || levelGeneratorManager == null) return false;

            spawnY = playerManager.PlayerTransform.position.y + verticalSpawnOffset;
            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            return giSegment != null;
        }

        private bool CanSpawnAggressiveCat(Vector3 cachedTargetPos, out float spawnY)
        {
            spawnY = cachedTargetPos.y;
            if (levelGeneratorManager == null || poolManager == null || gameDataManager == null || catManager == null)
            {
                return false;
            }

            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            return giSegment != null;
        }

        private void SpawnAggressiveCatOnRow(int sideIndex, float spawnY)
        {
            Vector3 spawnTargetPos = new Vector3(0f, spawnY, 0f);
            SpawnAggressiveCatAtPosition(sideIndex, spawnTargetPos);
        }

        private void SpawnAggressiveCatAtPosition(int sideIndex, Vector3 cachedTargetPos)
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            if (!CanSpawnAggressiveCat(cachedTargetPos, out float spawnY))
            {
                return;
            }

            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            if (giSegment == null) return;

            float targetX = (sideIndex == 0) ? ConstGameplay.Cat.CAT_LEFT_LANE_SPAWN_POSITION : ConstGameplay.Cat.CAT_RIGHT_LANE_SPAWN_POSITION;
            Vector3 spawnPosition = new Vector3(targetX, spawnY, 0f);

            if (!gameDataManager.TryGetPrefab(ConstGameplay.Cat.AggressiveCat.PREFAB_NAME, out GameObject prefab))
            {
                return;
            }

            GameObject catGo = poolManager.Spawn(prefab, spawnPosition, Quaternion.identity, giSegment.transform);
            if (catGo == null) return;

            giSegment.RegisterSpawnedObject(catGo);

            var giAggressive = catGo.GetComponent<GIAggressiveCat>();
            if (giAggressive != null)
            {
                giAggressive.SetTargetSmashPosition(cachedTargetPos);
                SceneObjectContext.Instance.Register(giAggressive);

                CatSightDirection direction = (targetX < 0f) ? CatSightDirection.Right : CatSightDirection.Left;
                giAggressive.SetDirection(direction);

                catManager.RegisterDynamicCat(giAggressive, playerManager.PlayerTransform);
            }
        }
    }
}
