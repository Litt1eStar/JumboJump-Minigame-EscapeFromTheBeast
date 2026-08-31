using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Utilities;
using UnityEngine;
using static UnityEngine.Rendering.STP;

namespace JumboJumps.EFTB.Model.Obstacle
{
    public class HazardProgressionModel
    {
        private HazardConfigSO config;
        public HazardConfigSO Config
        {
            get
            {
                var container = SceneObjectContext.Instance?.Get<GI.GIGameplayConfigContainer>();
                if (container != null && container.HazardConfig != null)
                {
                    config = container.HazardConfig;
                    return config;
                }
                if (config == null)
                {
                    DebugLogHelper.LogError($"[{GetType().Name}] HazardConfigSO reference is missing.");
                }
                return config;
            }
            set => config = value;
        }

        /// <summary>
        /// Calculates the spawn interval (cooldown between objects on the same row) based on row height Y.
        /// Starts at Base Range [Base_Interval_Low, Base_Interval_High] and reduces by Step_Interval_Reduction every 30 cells,
        /// clamped to Min_Spawn_Interval.
        /// </summary>
        public float GetRandomSpawnInterval(float worldY)
        {
            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int stepCells = Config != null ? Config.StepIntervalCells : ConstGameplay.Obstacle.Hazard.STEP_INTERVAL_CELLS;
            float stepDistanceInUnits = stepCells * cellHeight;
            float steps = Mathf.Max(0f, Mathf.Floor(worldY / stepDistanceInUnits));

            float stepReduction = Config != null ? Config.StepIntervalReduction : ConstGameplay.Obstacle.Hazard.STEP_INTERVAL_REDUCTION;
            float reduction = steps * stepReduction;

            float minLimit = Config != null ? Config.MinSpawnInterval : ConstGameplay.Obstacle.Hazard.MIN_SPAWN_INTERVAL;
            float baseLow = Config != null ? Config.BaseIntervalLow : ConstGameplay.Obstacle.Hazard.BASE_INTERVAL_LOW;
            float baseHigh = Config != null ? Config.BaseIntervalHigh : ConstGameplay.Obstacle.Hazard.BASE_INTERVAL_HIGH;

            float currentLow = Mathf.Max(minLimit, baseLow - reduction);
            float currentHigh = Mathf.Max(minLimit + 0.2f, baseHigh - reduction);

            return Random.Range(currentLow, currentHigh);
        }

        /// <summary>
        /// Calculates horizontal movement speed (units/sec) for objects on a row.
        /// Picks a random duration per lane from Floor Range [FLOOR_SPEED_DURATION_LOW, FLOOR_SPEED_DURATION_HIGH] and converts to speed (units/sec).
        /// </summary>
        public float GetRandomRowSpeed()
        {
            float speedLow = Config != null ? Config.FloorSpeedDurationLow : 1.0f;
            float speedHigh = Config != null ? Config.FloorSpeedDurationHigh : 2.0f;

            float durationPerLane = Random.Range(speedLow, speedHigh);
            float laneSize = ConstGameplay.LevelGenerator.LANE_SIZE;
            return laneSize / Mathf.Max(0.01f, durationPerLane);
        }
    }
}
