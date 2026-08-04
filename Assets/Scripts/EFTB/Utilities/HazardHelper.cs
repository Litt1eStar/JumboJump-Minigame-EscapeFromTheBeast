using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    public static class HazardHelper
    {
        /// <summary>
        /// Calculates the spawn interval (cooldown between objects on the same row) based on row height Y.
        /// Starts at Base Range [Base_Interval_Low, Base_Interval_High] and reduces by Step_Interval_Reduction every 30 cells,
        /// clamped to Min_Spawn_Interval.
        /// </summary>
        public static float GetRandomSpawnInterval(float worldY)
        {
            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            float stepDistanceInUnits = ConstGameplay.Obstacle.Hazard.STEP_INTERVAL_CELLS * cellHeight;
            float steps = Mathf.Max(0f, Mathf.Floor(worldY / stepDistanceInUnits));
            float reduction = steps * ConstGameplay.Obstacle.Hazard.STEP_INTERVAL_REDUCTION;

            float minLimit = ConstGameplay.Obstacle.Hazard.MIN_SPAWN_INTERVAL;
            float currentLow = Mathf.Max(minLimit, ConstGameplay.Obstacle.Hazard.BASE_INTERVAL_LOW - reduction);
            float currentHigh = Mathf.Max(minLimit + 0.2f, ConstGameplay.Obstacle.Hazard.BASE_INTERVAL_HIGH - reduction);

            return Random.Range(currentLow, currentHigh);
        }

        /// <summary>
        /// Calculates horizontal movement speed (units/sec) for objects on a row.
        /// Picks a random duration per lane from Floor Range [FLOOR_SPEED_DURATION_LOW, FLOOR_SPEED_DURATION_HIGH] and converts to speed (units/sec).
        /// </summary>
        public static float GetRandomRowSpeed()
        {
            float durationPerLane = Random.Range(
                ConstGameplay.Obstacle.Hazard.FLOOR_SPEED_DURATION_LOW,
                ConstGameplay.Obstacle.Hazard.FLOOR_SPEED_DURATION_HIGH
            );
            float laneSize = ConstGameplay.LevelGenerator.LANE_SIZE;
            return laneSize / Mathf.Max(0.01f, durationPerLane);
        }
    }
}
