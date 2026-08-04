using System.Collections.Generic;
using JumboJumps.EFTB.Constant.Gameplay;
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

        /// <summary>
        /// Procedurally generates static furniture blocks for a level segment based on height progression,
        /// corridor connectivity rules, spacing constraints, and maximum block limits.
        /// </summary>
        public List<FurnitureBlockData> GenerateSegmentFurniture(float segmentStartY,
                                                                 float segmentHeight,
                                                                 int laneCount,
                                                                 ref int lastOpenLaneIndex,
                                                                 ref float lastFurnitureWorldY)
        {
            List<FurnitureBlockData> generatedBlocks = new List<FurnitureBlockData>();
            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
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
            // Initial row (Y <= 0) is always kept safe/empty for player start
            if (worldY <= 0f) return false;

            // Check minimum 1-cell spacing constraint
            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            float minSpacing = (ConstGameplay.Obstacle.Furniture.MIN_ROW_SPACING_CELLS + 1) * cellHeight - 0.1f;
            if (lastFurnitureWorldY >= 0f && (worldY - lastFurnitureWorldY) < minSpacing)
            {
                return false;
            }

            // Progression density check: base 20%, +5% per 30 cells, cap 60%
            float density = CalculateDensity(worldY);
            return Random.value < density;
        }

        private float CalculateDensity(float worldY)
        {
            float baseRatio = ConstGameplay.Obstacle.Furniture.BASE_FURNITURE_ROW_RATIO;
            float stepRatio = ConstGameplay.Obstacle.Furniture.DENSITY_STEP_RATIO;
            float maxRatio = ConstGameplay.Obstacle.Furniture.MAX_FURNITURE_ROW_RATIO;
            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;

            float stepDistanceInUnits = ConstGameplay.Obstacle.Furniture.DENSITY_STEP_CELLS * cellHeight;
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
            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int cellIndex = Mathf.RoundToInt(worldY / cellHeight);

            // First 120 cells (cellIndex < 120): Strictly max 1 furniture block per row.
            // Past 120 cells (cellIndex >= 120): Up to MAX_BLOCKS_PER_ROW (2) furniture blocks per row.
            int maxPerConfig = (cellIndex >= ConstGameplay.Obstacle.Furniture.SINGLE_BLOCK_MAX_CELLS)
                ? ConstGameplay.Obstacle.Furniture.MAX_BLOCKS_PER_ROW
                : 1;

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
