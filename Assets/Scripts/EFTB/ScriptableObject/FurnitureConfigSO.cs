using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;

namespace JumboJumps.EFTB.Config
{
    [CreateAssetMenu(fileName = "FurnitureConfigSO", menuName = "EFTB/Config/FurnitureConfig")]
    public class FurnitureConfigSO : UnityEngine.ScriptableObject
    {
        [Header("Safe Zone")]
        [Tooltip("Number of safe cells at the start of the level where no furniture obstacles are spawned (default: 5)")]
        [SerializeField] private int safeZoneCells = ConstGameplay.Obstacle.SAFE_ZONE_CELLS;

        [Header("Grid Metrics")]
        [Tooltip("Height of each grid cell in world units/meters (default: 3.0)")]
        [SerializeField] private float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;

        [Header("Row Density & Spacing")]
        [Tooltip("Base spawn ratio for furniture rows (default: 0.20 = 20%)")]
        [SerializeField] private float baseRowRatio = ConstGameplay.Obstacle.Furniture.BASE_FURNITURE_ROW_RATIO;

        [Tooltip("Incremental spawn ratio increase per density step (default: 0.05 = 5%)")]
        [SerializeField] private float densityStepRatio = ConstGameplay.Obstacle.Furniture.DENSITY_STEP_RATIO;

        [Tooltip("Number of cells after which the density step ratio is applied (default: 30)")]
        [SerializeField] private int densityStepCells = ConstGameplay.Obstacle.Furniture.DENSITY_STEP_CELLS;

        [Tooltip("Maximum spawn ratio cap for furniture rows (default: 0.60 = 60%)")]
        [SerializeField] private float maxRowRatio = ConstGameplay.Obstacle.Furniture.MAX_FURNITURE_ROW_RATIO;

        [Tooltip("Minimum spacing between furniture rows in cells (default: 1)")]
        [SerializeField] private int minRowSpacingCells = ConstGameplay.Obstacle.Furniture.MIN_ROW_SPACING_CELLS;

        [Header("Row Furniture Block Limits")]
        [Tooltip("Maximum number of cells for a single furniture block restriction (default: 120)")]
        [SerializeField] private int singleBlockMaxCells = ConstGameplay.Obstacle.Furniture.SINGLE_BLOCK_MAX_CELLS;

        [Tooltip("Maximum number of furniture blocks allowed per row past singleBlockMaxCells (default: 2)")]
        [SerializeField] private int maxBlocksPerRow = ConstGameplay.Obstacle.Furniture.MAX_BLOCKS_PER_ROW;

        // Properties
        public int SafeZoneCells => safeZoneCells;
        public float CellHeight => cellHeight;
        public float BaseRowRatio => baseRowRatio;
        public float DensityStepRatio => densityStepRatio;
        public int DensityStepCells => densityStepCells;
        public float MaxRowRatio => maxRowRatio;
        public int MinRowSpacingCells => minRowSpacingCells;
        public int SingleBlockMaxCells => singleBlockMaxCells;
        public int MaxBlocksPerRow => maxBlocksPerRow;
    }
}
