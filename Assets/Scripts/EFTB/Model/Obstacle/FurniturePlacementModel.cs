using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Model.Obstacle
{
    public class FurnitureBlockData
    {
        public int LaneIndex { get; }
        public float YOffset { get; }
        public string PrefabName { get; }

        public FurnitureBlockData(int laneIndex, float yOffset, string prefabName)
        {
            LaneIndex = laneIndex;
            YOffset = yOffset;
            PrefabName = prefabName;
        }
    }

    public class FurniturePlacementModel
    {
        private readonly List<int> cachedAvailableLanesBuffer = new List<int>(4);

        private FurnitureConfigSO config;
        public FurnitureConfigSO Config
        {
            get
            {
                var container = SceneObjectContext.Instance?.Get<GI.GIGameplayConfigContainer>();
                if (container != null && container.FurnitureConfig != null)
                {
                    config = container.FurnitureConfig;
                    return config;
                }
                if (config == null)
                {
                    DebugLogHelper.LogError($"[{GetType().Name}] FurnitureConfigSO reference is missing.");
                }
                return config;
            }
            set => config = value;
        }

        /// <summary>
        /// Procedurally generates static furniture blocks for a level segment based on height progression,
        /// corridor connectivity rules, spacing constraints, and maximum block limits.
        /// </summary>
        public List<FurnitureBlockData> GenerateSegmentFurniture(
            float segmentStartY,
            float segmentHeight,
            int laneCount,
            ref int lastOpenLaneIndex,
            ref float lastFurnitureWorldY)
        {
            List<FurnitureBlockData> generatedBlocks = new List<FurnitureBlockData>();
            float cellHeight = Config != null ? Config.CellHeight : 3.0f;
            int startRowIndex = Mathf.RoundToInt(segmentStartY / cellHeight);
            int totalRows = Mathf.RoundToInt(segmentHeight / cellHeight);

            for (int r = 0; r < totalRows; r++)
            {
                int globalRowIndex = startRowIndex + r;
                float worldY = globalRowIndex * cellHeight;

                if (!ShouldSpawnFurnitureOnRow(worldY, lastFurnitureWorldY))
                {
                    continue;
                }

                float rowYOffset = worldY - segmentStartY;
                int chosenOpenLane = SelectCorridorOpenLane(laneCount, lastOpenLaneIndex);
                lastOpenLaneIndex = chosenOpenLane;
                lastFurnitureWorldY = worldY;

                PopulateFurnitureBlocksForRow(generatedBlocks, laneCount, chosenOpenLane, worldY, rowYOffset);
            }

            return generatedBlocks;
        }

        private bool ShouldSpawnFurnitureOnRow(float worldY, float lastFurnitureWorldY)
        {
            float cellHeight = Config != null ? Config.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int currentCellIndex = Mathf.RoundToInt(worldY / cellHeight);

            int safeZone = Config != null ? Config.SafeZoneCells : ConstGameplay.Obstacle.SAFE_ZONE_CELLS;
            if (currentCellIndex <= safeZone) return false;

            // Check minimum 1-cell spacing constraint in cell-index space to prevent float precision drift at high Y
            int lastFurnitureCellIndex = lastFurnitureWorldY >= 0f ? Mathf.RoundToInt(lastFurnitureWorldY / cellHeight) : -999;
            int minSpacing = Config != null ? Config.MinRowSpacingCells : ConstGameplay.Obstacle.Furniture.MIN_ROW_SPACING_CELLS;
            int minSpacingCells = minSpacing + 1;

            if (lastFurnitureCellIndex >= 0 && (currentCellIndex - lastFurnitureCellIndex) < minSpacingCells)
            {
                return false;
            }

            // Progression density check
            float density = CalculateDensity(worldY);
            return Random.value < density;
        }

        private float CalculateDensity(float worldY)
        {
            float baseRatio = Config != null ? Config.BaseRowRatio : ConstGameplay.Obstacle.Furniture.BASE_FURNITURE_ROW_RATIO;
            float stepRatio = Config != null ? Config.DensityStepRatio : ConstGameplay.Obstacle.Furniture.DENSITY_STEP_RATIO;
            float maxRatio = Config != null ? Config.MaxRowRatio : ConstGameplay.Obstacle.Furniture.MAX_FURNITURE_ROW_RATIO;
            float cellHeight = Config != null ? Config.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int stepCells = Config != null ? Config.DensityStepCells : ConstGameplay.Obstacle.Furniture.DENSITY_STEP_CELLS;

            float stepDistanceInUnits = stepCells * cellHeight;
            float steps = Mathf.Floor(worldY / stepDistanceInUnits);
            return Mathf.Min(maxRatio, baseRatio + (steps * stepRatio));
        }

        private int SelectCorridorOpenLane(int laneCount, int currentOpenLane)
        {
            int minLane = Mathf.Max(0, currentOpenLane - 1);
            int maxLane = Mathf.Min(laneCount - 1, currentOpenLane + 1);
            return Random.Range(minLane, maxLane + 1);
        }

        private void PopulateFurnitureBlocksForRow(
            List<FurnitureBlockData> destinationList,
            int laneCount,
            int openLane,
            float worldY,
            float rowYOffset)
        {
            int maxAllowedBlocks = DetermineMaxAllowedBlocks(worldY, laneCount);
            
            cachedAvailableLanesBuffer.Clear();
            for (int l = 0; l < laneCount; l++)
            {
                if (l != openLane)
                {
                    cachedAvailableLanesBuffer.Add(l);
                }
            }

            int blocksToSpawn = (maxAllowedBlocks > 1 && cachedAvailableLanesBuffer.Count > 1)
                ? Random.Range(1, maxAllowedBlocks + 1)
                : 1;

            blocksToSpawn = Mathf.Min(blocksToSpawn, cachedAvailableLanesBuffer.Count);

            for (int i = 0; i < blocksToSpawn; i++)
            {
                int randomIndex = Random.Range(0, cachedAvailableLanesBuffer.Count);
                int blockedLane = cachedAvailableLanesBuffer[randomIndex];
                cachedAvailableLanesBuffer.RemoveAt(randomIndex);

                string prefabName = SelectRandomFurniturePrefab();
                destinationList.Add(new FurnitureBlockData(blockedLane, rowYOffset, prefabName));
            }
        }

        private int DetermineMaxAllowedBlocks(float worldY, int laneCount)
        {
            float cellHeight = Config != null ? Config.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int cellIndex = Mathf.RoundToInt(worldY / cellHeight);

            int singleBlockMax = Config != null ? Config.SingleBlockMaxCells : ConstGameplay.Obstacle.Furniture.SINGLE_BLOCK_MAX_CELLS;
            int maxBlocks = Config != null ? Config.MaxBlocksPerRow : ConstGameplay.Obstacle.Furniture.MAX_BLOCKS_PER_ROW;

            int maxPerConfig = (cellIndex >= singleBlockMax) ? maxBlocks : 1;

            // Ensure every row leaves at least 1 open lane
            return Mathf.Min(maxPerConfig, laneCount - 1);
        }

        private string SelectRandomFurniturePrefab()
        {
            string[] prefabs = ConstGameplay.Obstacle.Furniture.FURNITURE_PREFAB_NAMES;
            if (prefabs == null || prefabs.Length == 0) return ConstGameplay.Obstacle.Furniture.DEFAULT_FURNITURE_PREFAB;
            return prefabs[Random.Range(0, prefabs.Length)];
        }
    }
}
