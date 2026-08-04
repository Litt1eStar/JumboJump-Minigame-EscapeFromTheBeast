using System;
using System.Collections.Generic;
using JumboJumps.EFTB.Config;
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

            float cellHeight = 3.0f;
            int startRowIndex = Mathf.RoundToInt(segmentStartY / cellHeight);
            int totalRows = Mathf.RoundToInt(segmentHeight / cellHeight);

            for (int r = 0; r < totalRows; r++)
            {
                int globalRowIndex = startRowIndex + r;
                float worldY = globalRowIndex * cellHeight;

                if (globalRowIndex <= config.SafeZoneCells) continue;

                // 15% spawn ratio check per row
                if (UnityEngine.Random.value >= config.SpawnRowRatio) continue;

                float rowYOffset = worldY - segmentStartY;
                bool isHazardRow = isHazardRowFunc != null && isHazardRowFunc(worldY);

                validLanesBuffer.Clear();
                laneWeightsBuffer.Clear();

                float totalWeight = 0f;
                for (int l = 0; l < laneCount; l++)
                {
                    // Rule: Never sit on a furniture cell
                    if (isFurnitureBlockedFunc != null && isFurnitureBlockedFunc(l, worldY))
                    {
                        continue;
                    }

                    // Rule: 3x weight multiplier for open lanes on hazard rows (favor risk)
                    float weight = isHazardRow ? config.HazardLaneWeightMultiplier : 1.0f;
                    validLanesBuffer.Add(l);
                    laneWeightsBuffer.Add(weight);
                    totalWeight += weight;
                }

                if (validLanesBuffer.Count == 0 || totalWeight <= 0f) continue;

                // Weighted random lane selection
                float randomValue = UnityEngine.Random.value * totalWeight;
                float currentSum = 0f;
                int selectedLane = validLanesBuffer[0];

                for (int i = 0; i < validLanesBuffer.Count; i++)
                {
                    currentSum += laneWeightsBuffer[i];
                    if (randomValue <= currentSum)
                    {
                        selectedLane = validLanesBuffer[i];
                        break;
                    }
                }

                generatedCollectibles.Add(new CollectiblePlacementData(selectedLane, rowYOffset));
            }

            return generatedCollectibles;
        }
    }
}
