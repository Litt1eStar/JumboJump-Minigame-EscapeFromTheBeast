using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.GameData.Cat;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIGameplayConfigContainer : MonoBehaviour
    {
        [Header("ScriptableObject Configuration Assets")]
        [Tooltip("Furniture obstacle balancing settings configured by game designers")]
        [SerializeField] private FurnitureConfigSO furnitureConfig;

        [Tooltip("Hazard obstacle balancing settings configured by game designers")]
        [SerializeField] private HazardConfigSO hazardConfig;

        [Tooltip("Collectible treat balancing settings configured by game designers")]
        [SerializeField] private CollectibleConfigSO collectibleConfig;

        [Tooltip("Aggressive cat balancing settings configured by game designers")]
        [SerializeField] private AggressiveCatConfigSO aggressiveCatConfig;

        [Tooltip("UI balancing settings configured by game designers")]
        [SerializeField] private UIConfigSO uiConfig;

        public FurnitureConfigSO FurnitureConfig => furnitureConfig;
        public HazardConfigSO HazardConfig => hazardConfig;
        public CollectibleConfigSO CollectibleConfig => collectibleConfig;
        public AggressiveCatConfigSO AggressiveCatConfig => aggressiveCatConfig;
        public UIConfigSO UIConfig => uiConfig;
    }
}
