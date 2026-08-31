using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;

namespace JumboJumps.EFTB.Config
{
    [CreateAssetMenu(fileName = "HazardConfigSO", menuName = "EFTB/Config/HazardConfig")]
    public class HazardConfigSO : UnityEngine.ScriptableObject
    {
        [Header("Safe Zone")]
        [Tooltip("Number of safe cells at level start where no hazards spawn (default: 5)")]
        [SerializeField] private int safeZoneCells = ConstGameplay.Obstacle.SAFE_ZONE_CELLS;

        [Header("Grid Metrics")]
        [Tooltip("Height of each grid cell in world units/meters (default: 3.0)")]
        [SerializeField] private float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;

        [Header("Spawn Interval Balancing")]
        [Tooltip("Base spawn interval range low in seconds (default: 3.0s)")]
        [SerializeField] private float baseIntervalLow = ConstGameplay.Obstacle.Hazard.BASE_INTERVAL_LOW;

        [Tooltip("Base spawn interval range high in seconds (default: 6.0s)")]
        [SerializeField] private float baseIntervalHigh = ConstGameplay.Obstacle.Hazard.BASE_INTERVAL_HIGH;

        [Tooltip("Spawn interval reduction per step in seconds (default: 0.15s)")]
        [SerializeField] private float stepIntervalReduction = ConstGameplay.Obstacle.Hazard.STEP_INTERVAL_REDUCTION;

        [Tooltip("Step height in cells for spawn interval reduction (default: 30 cells)")]
        [SerializeField] private int stepIntervalCells = ConstGameplay.Obstacle.Hazard.STEP_INTERVAL_CELLS;

        [Tooltip("Minimum spawn interval floor limit in seconds (default: 0.5s)")]
        [SerializeField] private float minSpawnInterval = ConstGameplay.Obstacle.Hazard.MIN_SPAWN_INTERVAL;

        [Header("Hazard Car Speed Balancing")]
        [Tooltip("Hazard speed floor range low in seconds per lane (default: 1.0s / lane)")]
        [SerializeField] private float floorSpeedDurationLow = ConstGameplay.Obstacle.Hazard.FLOOR_SPEED_DURATION_LOW;

        [Tooltip("Hazard speed floor range high in seconds per lane (default: 2.0s / lane)")]
        [SerializeField] private float floorSpeedDurationHigh = ConstGameplay.Obstacle.Hazard.FLOOR_SPEED_DURATION_HIGH;

        [Header("Offscreen & Pre-Spawn Offsets")]
        [Tooltip("Offscreen initial spawn X coordinate offset in units/meters (default: 7.5)")]
        [SerializeField] private float spawnOffscreenXOffset = ConstGameplay.Obstacle.Hazard.SPAWN_OFFSCREEN_X_OFFSET;

        [Tooltip("Offscreen despawn X coordinate offset in units/meters (default: 7.5)")]
        [SerializeField] private float despawnOffscreenXOffset = ConstGameplay.Obstacle.Hazard.DESPAWN_OFFSCREEN_X_OFFSET;

        [Tooltip("Offset distance from the player to pre-spawn hazards in cells (default: 70)")]
        [SerializeField] private float hazardPrespawnOffset = ConstGameplay.Obstacle.Hazard.HAZARD_PRESPAWN_OFFSET;

        [Header("Hazard Animation Balancing")]
        [Tooltip("Rotation speed multiplier for basketball hazard animation (default: 5.0)")]
        [SerializeField] private float rotationSpeed = ConstGameplay.Obstacle.Hazard.ROTATION_SPEED;

        // Properties
        public int SafeZoneCells => safeZoneCells;
        public float CellHeight => cellHeight;
        public float BaseIntervalLow => baseIntervalLow;
        public float BaseIntervalHigh => baseIntervalHigh;
        public float StepIntervalReduction => stepIntervalReduction;
        public int StepIntervalCells => stepIntervalCells;
        public float MinSpawnInterval => minSpawnInterval;
        public float FloorSpeedDurationLow => floorSpeedDurationLow;
        public float FloorSpeedDurationHigh => floorSpeedDurationHigh;
        public float SpawnOffscreenXOffset => spawnOffscreenXOffset;
        public float DespawnOffscreenXOffset => despawnOffscreenXOffset;
        public float HazardPrespawnOffset => hazardPrespawnOffset;
        public float RotationSpeed => rotationSpeed;
    }
}
