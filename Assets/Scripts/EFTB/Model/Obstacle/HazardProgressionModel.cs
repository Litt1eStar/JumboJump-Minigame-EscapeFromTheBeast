using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;

namespace JumboJumps.EFTB.Model.Obstacle
{
    public class HazardProgressionModel
    {
        /// <summary>
        /// Calculates the spawn interval (cooldown between objects on the same row) based on row height Y.
        /// Starts at Base Range [Base_Interval_Low, Base_Interval_High] and reduces by Step_Interval_Reduction every 30 cells,
        /// clamped to Min_Spawn_Interval.
        /// </summary>
        public float GetRandomSpawnInterval(float worldY)
        {
            float cellHeight = ConstGameplay.Obstacle.Furniture.Cell_Height;
            float stepDistanceInUnits = ConstGameplay.Obstacle.Hazard.Step_Interval_Cells * cellHeight;
            float steps = Mathf.Max(0f, Mathf.Floor(worldY / stepDistanceInUnits));
            float reduction = steps * ConstGameplay.Obstacle.Hazard.Step_Interval_Reduction;

            float minLimit = ConstGameplay.Obstacle.Hazard.Min_Spawn_Interval;
            float currentLow = Mathf.Max(minLimit, ConstGameplay.Obstacle.Hazard.Base_Interval_Low - reduction);
            float currentHigh = Mathf.Max(minLimit + 0.2f, ConstGameplay.Obstacle.Hazard.Base_Interval_High - reduction);

            return Random.Range(currentLow, currentHigh);
        }

        /// <summary>
        /// Calculates horizontal movement speed (units/sec) for objects on a row.
        /// Picks a random duration per lane from Floor Range [Floor_Speed_Duration_Low, Floor_Speed_Duration_High] and converts to speed (units/sec).
        /// </summary>
        public float GetRandomRowSpeed()
        {
            float durationPerLane = Random.Range(
                ConstGameplay.Obstacle.Hazard.Floor_Speed_Duration_Low,
                ConstGameplay.Obstacle.Hazard.Floor_Speed_Duration_High
            );
            float laneSize = ConstGameplay.LevelGenerator.Lane_Size;
            return laneSize / Mathf.Max(0.01f, durationPerLane);
        }
    }
}
