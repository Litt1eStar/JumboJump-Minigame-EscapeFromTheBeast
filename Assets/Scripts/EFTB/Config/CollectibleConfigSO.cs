using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;

namespace JumboJumps.EFTB.Config
{
    [CreateAssetMenu(fileName = "CollectibleConfigSO", menuName = "EFTB/Config/CollectibleConfig")]
    public class CollectibleConfigSO : ScriptableObject
    {
        [Header("Safe Zone")]
        [Tooltip("Number of safe cells at level start where no treats spawn (default: 5)")]
        [SerializeField] private int safeZoneCells = 5;

        [Header("Grid Metrics")]
        [Tooltip("Height of each grid cell in world units/meters (default: 3.0)")]
        [SerializeField] private float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;

        [Header("Treat Spawn & Point Balancing")]
        [Tooltip("Spawn ratio for treats across level rows (default: 0.15 = 15%)")]
        [SerializeField] private float spawnRowRatio = 0.15f;

        [Tooltip("Flat point value awarded upon collecting a treat (default: 100)")]
        [SerializeField] private int treatPointValue = 100;

        [Header("Risk Placement Weights")]
        [Tooltip("Placement weight multiplier for open lanes on hazard rows to favor risk (default: 3.0)")]
        [SerializeField] private float hazardLaneWeightMultiplier = 3.0f;

        [Header("Prefab Reference")]
        [Tooltip("Default prefab name for treat collectibles in GameDataManager (default: Prefab_Collectible_Coin)")]
        [SerializeField] private string prefabName = "Prefab_Collectible_Coin";

        public int SafeZoneCells => safeZoneCells;
        public float CellHeight => cellHeight;
        public float SpawnRowRatio => spawnRowRatio;
        public int TreatPointValue => treatPointValue;
        public float HazardLaneWeightMultiplier => hazardLaneWeightMultiplier;
        public string PrefabName => prefabName;
    }
}
