using System.Collections.Generic;
using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Model.Obstacle;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class HazardSpawner
    {
        private ObjectPoolManager poolManager;
        private LevelGeneratorManager levelGeneratorManager;
        private GameDataManager gameDataManager;
        private GameplayStateManager gameplayStateManager;
        private PlayerManager playerManager;

        private readonly HazardProgressionModel progressionModel = new HazardProgressionModel();
        private readonly Dictionary<int, HazardRowData> activeHazardRows = new Dictionary<int, HazardRowData>();
        private readonly List<GIHazardObstacle> activeHazards = new List<GIHazardObstacle>();

        private HazardConfigSO hazardConfig;
        public HazardConfigSO HazardConfig
        {
            get
            {
                if (hazardConfig == null && SceneObjectContext.Instance != null)
                {
                    var container = SceneObjectContext.Instance.Get<GI.GIGameplayConfigContainer>();
                    if (container != null && container.HazardConfig != null)
                    {
                        hazardConfig = container.HazardConfig;
                        progressionModel.Config = hazardConfig;
                    }
                }
                return hazardConfig;
            }
        }

        public float SpawnOffscreenXOffset => HazardConfig != null ? HazardConfig.SpawnOffscreenXOffset : ConstGameplay.Obstacle.Hazard.SPAWN_OFFSCREEN_X_OFFSET;
        public float DespawnOffscreenXOffset => HazardConfig != null ? HazardConfig.DespawnOffscreenXOffset : ConstGameplay.Obstacle.Hazard.DESPAWN_OFFSCREEN_X_OFFSET;

        public void Initialize()
        {
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            playerManager = GameContext.Instance.Get<PlayerManager>();

            var _ = HazardConfig;

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            activeHazardRows.Clear();

            if (poolManager != null)
            {
                for (int i = 0; i < activeHazards.Count; i++)
                {
                    if (activeHazards[i] != null && activeHazards[i].gameObject.activeInHierarchy)
                    {
                        poolManager.Recycle(activeHazards[i].gameObject);
                    }
                }
            }
            activeHazards.Clear();

            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            if (playerManager?.PlayerTransform == null || levelGeneratorManager == null) return;

            float playerY = playerManager.PlayerTransform.position.y;
            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;

            int safeZoneCells = HazardConfig != null ? HazardConfig.SafeZoneCells : ConstGameplay.Obstacle.SAFE_ZONE_CELLS;
            int safeZoneMinRowIndex = safeZoneCells + 1;
            int minRowIndex = Mathf.Max(safeZoneMinRowIndex, Mathf.FloorToInt((playerY - 3f) / cellHeight));

            float prespawnOffset = HazardConfig != null ? HazardConfig.HazardPrespawnOffset : ConstGameplay.Obstacle.Hazard.HAZARD_PRESPAWN_OFFSET;
            float maxAllowedY = Mathf.Min(playerY + prespawnOffset, levelGeneratorManager.MaxGeneratedWorldY - cellHeight);
            int maxRowIndex = Mathf.FloorToInt(maxAllowedY / cellHeight);

            // Update hazard spawning for active visible rows
            for (int r = minRowIndex; r <= maxRowIndex; r++)
            {
                if (r <= safeZoneCells)
                {
                    continue;
                }
                float rowWorldY = r * cellHeight;

                if (IsRowBlockedByFurniture(rowWorldY))
                {
                    continue;
                }

                if (!activeHazardRows.TryGetValue(r, out HazardRowData rowData))
                {
                    HazardDirectionEnum direction = (Random.value < 0.5f) ? HazardDirectionEnum.LeftToRight : HazardDirectionEnum.RightToLeft;
                    float speed = progressionModel.GetRandomRowSpeed();
                    float initialInterval = progressionModel.GetRandomSpawnInterval(rowWorldY);

                    rowData = new HazardRowData(rowWorldY, direction, speed, initialInterval);
                    activeHazardRows[r] = rowData;
                }

                rowData.NextSpawnTimer += deltaTime;
                if (rowData.NextSpawnTimer >= rowData.SpawnInterval)
                {
                    rowData.NextSpawnTimer = 0f;
                    rowData.SpawnInterval = progressionModel.GetRandomSpawnInterval(rowWorldY);
                    SpawnHazardOnRow(rowData);
                }
            }

            // Cleanup hazard row tracking for rows far below player
            CleanupPassedRows(minRowIndex - 5);
        }

        private bool IsRowBlockedByFurniture(float rowWorldY)
        {
            if (levelGeneratorManager == null) return false;

            int laneCount = levelGeneratorManager.LaneXPositions?.Length ?? 3;
            for (int lane = 0; lane < laneCount; lane++)
            {
                if (levelGeneratorManager.IsCellBlockedByFurniture(lane, rowWorldY))
                {
                    return true;
                }
            }
            return false;
        }

        private void SpawnHazardOnRow(HazardRowData rowData)
        {
            if (poolManager == null || gameDataManager == null) return;

            if (IsRowBlockedByFurniture(rowData.RowWorldY))
            {
                int rowIdx = Mathf.RoundToInt(rowData.RowWorldY / ConstGameplay.Obstacle.Furniture.CELL_HEIGHT);
                activeHazardRows.Remove(rowIdx);
                return;
            }

            if (!gameDataManager.TryGetPrefab(ConstGameplay.Obstacle.Hazard.PREFAB_NAME, out GameObject prefab))
            {
                // Fallback to moving obstacle prefab if specific hazard prefab is not yet in registry
                if (!gameDataManager.TryGetPrefab("Prefab_Obstacle_Car", out prefab))
                {
                    DebugLogHelper.LogWarning($"[HazardSpawner] Hazard prefab '{ConstGameplay.Obstacle.Hazard.PREFAB_NAME}' not found in GameDataManager.");
                    return;
                }
            }

            float spawnOffset = SpawnOffscreenXOffset;
            float despawnOffset = DespawnOffscreenXOffset;

            float spawnX = (rowData.Direction == HazardDirectionEnum.LeftToRight) ? -spawnOffset : spawnOffset;
            float despawnX = (rowData.Direction == HazardDirectionEnum.LeftToRight) ? despawnOffset : -despawnOffset;

            Vector3 spawnPos = new Vector3(spawnX, rowData.RowWorldY, 0f);

            GameObject hazardObj = poolManager.Spawn(prefab, spawnPos, Quaternion.identity);
            if (hazardObj == null) return;

            GIHazardObstacle giHazard = hazardObj.GetComponent<GIHazardObstacle>();
            if (giHazard == null)
            {
                giHazard = hazardObj.AddComponent<GIHazardObstacle>();
            }

            activeHazards.RemoveAll(h => h == null || !h.gameObject.activeInHierarchy);
            activeHazards.Add(giHazard);

            giHazard.Initialize(rowData.Direction, rowData.Speed, rowData.RowWorldY, despawnX);
        }

        private void CleanupPassedRows(int cutoffRowIndex)
        {
            List<int> toRemove = null;
            foreach (var kvp in activeHazardRows)
            {
                if (kvp.Key < cutoffRowIndex)
                {
                    if (toRemove == null) toRemove = new List<int>();
                    toRemove.Add(kvp.Key);
                }
            }

            if (toRemove != null)
            {
                for (int i = 0; i < toRemove.Count; i++)
                {
                    activeHazardRows.Remove(toRemove[i]);
                }
            }
        }
    }
}
