using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Model.Obstacle;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    public static class FurniturePlacementHelper
    {
        private static readonly List<int> cachedAvailableLanesBuffer = new List<int>(4);

        /// <summary>
        /// Procedurally generates static furniture blocks for a level segment based on height progression,
        /// corridor connectivity rules, spacing constraints, and maximum block limits.
        /// </summary>
        public static List<FurnitureBlockData> GenerateSegmentFurniture(
            float segmentStartY,
            float segmentHeight,
            int laneCount,
            ref int lastOpenLaneIndex,
            ref float lastFurnitureWorldY,
            FurnitureConfigSO config = null)
        {
            List<FurnitureBlockData> generatedBlocks = new List<FurnitureBlockData>();
            float cellHeight = config != null ? config.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int startRowIndex = Mathf.RoundToInt(segmentStartY / cellHeight);
            int totalRows = Mathf.RoundToInt(segmentHeight / cellHeight);

            for (int r = 0; r < totalRows; r++)
            {
                int globalRowIndex = startRowIndex + r;
                float worldY = globalRowIndex * cellHeight;

                if (!ShouldSpawnFurnitureOnRow(worldY, lastFurnitureWorldY, config))
                {
                    continue;
                }

                float rowYOffset = worldY - segmentStartY;
                int chosenOpenLane = SelectCorridorOpenLane(laneCount, lastOpenLaneIndex);
                lastOpenLaneIndex = chosenOpenLane;
                lastFurnitureWorldY = worldY;

                PopulateFurnitureBlocksForRow(generatedBlocks, laneCount, chosenOpenLane, worldY, rowYOffset, config);
            }

            return generatedBlocks;
        }

        private static bool ShouldSpawnFurnitureOnRow(float worldY, float lastFurnitureWorldY, FurnitureConfigSO config)
        {
            float cellHeight = config != null ? config.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int currentCellIndex = Mathf.RoundToInt(worldY / cellHeight);

            int safeZone = config != null ? config.SafeZoneCells : ConstGameplay.Obstacle.SAFE_ZONE_CELLS;
            if (currentCellIndex <= safeZone) return false;

            // Check minimum 1-cell spacing constraint in cell-index space to prevent float precision drift at high Y
            int lastFurnitureCellIndex = lastFurnitureWorldY >= 0f ? Mathf.RoundToInt(lastFurnitureWorldY / cellHeight) : -1;
            int minSpacing = config != null ? config.MinRowSpacingCells : ConstGameplay.Obstacle.Furniture.MIN_ROW_SPACING_CELLS;
            int minSpacingCells = minSpacing + 1;

            if (lastFurnitureCellIndex >= 0 && (currentCellIndex - lastFurnitureCellIndex) < minSpacingCells)
            {
                return false;
            }

            // Progression density check
            float density = CalculateDensity(worldY, config);
            return Random.value < density;
        }

        private static float CalculateDensity(float worldY, FurnitureConfigSO config)
        {
            float baseRatio = config != null ? config.BaseRowRatio : ConstGameplay.Obstacle.Furniture.BASE_FURNITURE_ROW_RATIO;
            float stepRatio = config != null ? config.DensityStepRatio : ConstGameplay.Obstacle.Furniture.DENSITY_STEP_RATIO;
            float maxRatio = config != null ? config.MaxRowRatio : ConstGameplay.Obstacle.Furniture.MAX_FURNITURE_ROW_RATIO;
            float cellHeight = config != null ? config.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int stepCells = config != null ? config.DensityStepCells : ConstGameplay.Obstacle.Furniture.DENSITY_STEP_CELLS;

            float stepDistanceInUnits = stepCells * cellHeight;
            float steps = Mathf.Floor(worldY / stepDistanceInUnits);
            return Mathf.Min(maxRatio, baseRatio + (steps * stepRatio));
        }

        private static int SelectCorridorOpenLane(int laneCount, int currentOpenLane)
        {
            int minLane = Mathf.Max(0, currentOpenLane - 1);
            int maxLane = Mathf.Min(laneCount - 1, currentOpenLane + 1);
            return Random.Range(minLane, maxLane + 1);
        }

        private static void PopulateFurnitureBlocksForRow(
            List<FurnitureBlockData> destinationList,
            int laneCount,
            int openLane,
            float worldY,
            float rowYOffset,
            FurnitureConfigSO config)
        {
            int maxAllowedBlocks = DetermineMaxAllowedBlocks(worldY, laneCount, config);

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

        private static int DetermineMaxAllowedBlocks(float worldY, int laneCount, FurnitureConfigSO config)
        {
            float cellHeight = config != null ? config.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int cellIndex = Mathf.RoundToInt(worldY / cellHeight);

            int singleBlockMax = config != null ? config.SingleBlockMaxCells : ConstGameplay.Obstacle.Furniture.SINGLE_BLOCK_MAX_CELLS;
            int maxBlocks = config != null ? config.MaxBlocksPerRow : ConstGameplay.Obstacle.Furniture.MAX_BLOCKS_PER_ROW;

            int maxPerConfig = (cellIndex >= singleBlockMax) ? maxBlocks : 1;

            // Ensure every row leaves at least 1 open lane
            return Mathf.Min(maxPerConfig, laneCount - 1);
        }

        private static string SelectRandomFurniturePrefab()
        {
            string[] prefabs = ConstGameplay.Obstacle.Furniture.FURNITURE_PREFAB_NAMES;
            if (prefabs == null || prefabs.Length == 0) return ConstGameplay.Obstacle.Furniture.DEFAULT_FURNITURE_PREFAB;
            return prefabs[Random.Range(0, prefabs.Length)];
        }
    }
}
