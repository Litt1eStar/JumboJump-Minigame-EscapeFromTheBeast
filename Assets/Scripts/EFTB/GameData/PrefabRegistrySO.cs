using UnityEngine;
using System.Collections.Generic;
namespace JumboJumps.EFTB.GameData
{
    [CreateAssetMenu(fileName = "PrefabRegistry", menuName = "EFTB/PrefabRegistry")]
    public class PrefabRegistrySO : ScriptableObject
    {
        public List<GameObject> registry = new();
        public GameObject GetPrefab(string prefabName)
        {
            if (string.IsNullOrEmpty(prefabName)) return null;

            var prefab = registry.Find(e => e.name == prefabName);

            if (prefab == null)
            {
                Debug.LogWarning($"[{GetType().Name}] Prefab not found for name: {prefabName}");
            }

            return prefab;
        }
    }
}
