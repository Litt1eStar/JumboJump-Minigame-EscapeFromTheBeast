using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Utilities;
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

        public FurnitureConfigSO FurnitureConfig => furnitureConfig;
        public HazardConfigSO HazardConfig => hazardConfig;

    }
}
