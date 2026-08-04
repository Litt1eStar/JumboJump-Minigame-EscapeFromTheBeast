using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Model;
using UnityEngine;
using System.Collections;

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

        private CoroutineHelper coroutineHelper;
        private Coroutine warningCoroutine;
        private bool isAntiCampWarningActive;

        public void Initialize()
        {
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
            playerManager = GameContext.Instance.Get<PlayerManager>();
            catManager = GameContext.Instance.Get<CatManager>();
            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            if (coroutineHelper == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] {nameof(AggressiveCatSpawner)}| Failed to get {typeof(CoroutineHelper).AssemblyQualifiedName} from GameContext");
            }

            if (playerManager != null)
            {
                Subscribe();
            }

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            if (playerManager == null || coroutineHelper == null && warningCoroutine == null) return;

            Unsubscribe();
            coroutineHelper.Stop(warningCoroutine);

            warningCoroutine = null;
            isAntiCampWarningActive = false;
            coroutineHelper = null;
            
            GameContext.Instance.Remove(this);
        }

        public void Subscribe()
        {
            if (playerManager == null) return;

            playerManager.EventIdleLimitExceeded += OnPlayerIdleLimitExceeded;
        }

        public void Unsubscribe()
        {
            if (playerManager == null) return;

            playerManager.EventIdleLimitExceeded -= OnPlayerIdleLimitExceeded;
        }

        private void OnPlayerIdleLimitExceeded()
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            if (isAntiCampWarningActive || playerManager?.PlayerTransform == null) return;

            Vector3 cachedTargetPos = playerManager.PlayerTransform.position;

            TriggerAlphaCatPounceSequence(cachedTargetPos);
        }

        private void TriggerAlphaCatPounceSequence(Vector3 cachedTargetPos)
        {
            isAntiCampWarningActive = true;
            int sideIndex = (Random.value < 0.5f) ? 0 : 1;
            float warningDuration = ConstGameplay.Cat.AggressiveCat.POUNCE_WARNING_DURATION;

            playerManager.TriggerPounceWarning(warningDuration, null);

            if (coroutineHelper != null)
            {
                warningCoroutine = coroutineHelper.Restart(warningCoroutine, WaitAndSpawnRoutine(sideIndex, cachedTargetPos, warningDuration));
            }
            else
            {
                isAntiCampWarningActive = false;
            }
        }

        private IEnumerator WaitAndSpawnRoutine(int sideIndex, Vector3 cachedTargetPos, float warningDuration)
        {
            yield return new WaitForSeconds(warningDuration);
            warningCoroutine = null;
            isAntiCampWarningActive = false;
            SpawnAggressiveCat(sideIndex, cachedTargetPos);
        }

        private bool CanSpawnAggressiveCat(Vector3 cachedTargetPos, out float spawnY)
        {
            spawnY = 0f;
            if (levelGeneratorManager == null || poolManager == null || gameDataManager == null || catManager == null)
            {
                return false;
            }

            spawnY = cachedTargetPos.y;

            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            return giSegment != null;
        }

        private void SpawnAggressiveCat(int sideIndex, Vector3 cachedTargetPos)
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                DebugLogHelper.LogWarning($"[AggressiveCatSpawner] Cannot spawn AggressiveCat: Not in InGameState.");
                return;
            }

            float spawnY;
            if (!CanSpawnAggressiveCat(cachedTargetPos, out spawnY))
            {
                DebugLogHelper.LogWarning($"[AggressiveCatSpawner] Cannot spawn AggressiveCat: Environment state changed during the warning delay.");
                return;
            }

            var giSegment = levelGeneratorManager.GetGISegmentAtY(spawnY);
            if (giSegment == null) return;

            float targetX = (sideIndex == 0) ? ConstGameplay.Cat.CAT_LEFT_LANE_SPAWN_POSITION : ConstGameplay.Cat.CAT_RIGHT_LANE_SPAWN_POSITION;
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

            var giAggressive = catGo.GetComponent<GIAggressiveCat>();
            if (giAggressive != null)
            {
                giAggressive.SetTargetSmashPosition(cachedTargetPos);
                SceneObjectContext.Instance.Register(giAggressive);

                CatSightDirection direction = (targetX < 0f) ? CatSightDirection.Right : CatSightDirection.Left;
                giAggressive.SetDirection(direction);

                catManager.RegisterDynamicCat(giAggressive, playerManager.PlayerTransform);
            }
            else
            {
                DebugLogHelper.LogError("[AggressiveCatSpawner] Spawned cat GameObject is missing GIAggressiveCat component!");
            }
        }
    }
}
