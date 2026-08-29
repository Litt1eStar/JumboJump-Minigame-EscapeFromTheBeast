using System;
using System.Collections.Generic;
using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Model.Obstacle;
using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    public static class CollectiblePlacementHelper
    {
        private static readonly List<int> validLanesBuffer = new List<int>(4);
        private static readonly List<float> laneWeightsBuffer = new List<float>(4);

        /// <summary>
        /// Procedurally calculates collectible placement per row.
        /// Enforces ~15% row spawn rate, safe zone threshold, strict furniture exclusion,
        /// and 3x weighted placement on hazard rows for risk-reward balancing.
        /// </summary>
        public static List<CollectiblePlacementData> GenerateSegmentCollectibles(
            float segmentStartY,
            float segmentHeight,
            int laneCount,
            Func<int, float, bool> isFurnitureBlockedFunc,
            Func<float, bool> isHazardRowFunc,
            CollectibleConfigSO config = null)
        {
            List<CollectiblePlacementData> generatedCollectibles = new List<CollectiblePlacementData>();
            if (config == null) return generatedCollectibles;

            float cellHeight = config != null ? config.CellHeight : ConstGameplay.Player.STEP_DISTANCE_Y;
            int startRowIndex = Mathf.RoundToInt(segmentStartY / cellHeight);
            int totalRows = Mathf.RoundToInt(segmentHeight / cellHeight);

            for (int r = 0; r < totalRows; r++)
            {
                int globalRowIndex = startRowIndex + r;
                float worldY = (globalRowIndex * cellHeight) + (cellHeight * 0.5f);

                if (globalRowIndex <= config.SafeZoneCells) continue;

                bool isHazardRow = isHazardRowFunc != null && isHazardRowFunc(worldY);

                float effectiveSpawnRatio = isHazardRow
                    ? Mathf.Min(1.0f, config.SpawnRowRatio * config.HazardLaneWeightMultiplier)
                    : config.SpawnRowRatio;

                if (UnityEngine.Random.value >= effectiveSpawnRatio) continue;

                float rowYOffset = worldY - segmentStartY;

                validLanesBuffer.Clear();
                for (int l = 0; l < laneCount; l++)
                {
                    // Rule: Never sit on a furniture cell
                    if (isFurnitureBlockedFunc != null && isFurnitureBlockedFunc(l, worldY))
                    {
                        continue;
                    }

                    validLanesBuffer.Add(l);
                }

                if (validLanesBuffer.Count == 0) continue;

                int selectedLane = validLanesBuffer[UnityEngine.Random.Range(0, validLanesBuffer.Count)];
                generatedCollectibles.Add(new CollectiblePlacementData(selectedLane, rowYOffset));
            }

            return generatedCollectibles;
        }
    }
}
