using UnityEngine;
using System.Collections.Generic;
namespace JumboJumps.EFTB.GameData
{
    [CreateAssetMenu(fileName = "PrefabRegistry", menuName = "EFTB/PrefabRegistry")]
    public class PrefabRegistrySO : ScriptableObject
    {
        public List<GameObject> registry = new();

        private Dictionary<string, GameObject> prefabCache;
        public Dictionary<string, GameObject> PrefabCache
        {
            get
            {
                if (prefabCache == null)
                {
                    prefabCache = new Dictionary<string, GameObject>();
                    
                    foreach (var prefab in registry)
                    {
                        if (prefab != null && !prefabCache.ContainsKey(prefab.name))
                        {
                            prefabCache.Add(prefab.name, prefab);
                        }
                    }
                }
                return prefabCache;
            }
        }

        public GameObject GetPrefab(string prefabName)
        {
            if (string.IsNullOrEmpty(prefabName)) return null;

            if(PrefabCache.TryGetValue(prefabName, out GameObject prefab))
            {
                return prefab;
            }

            Debug.LogWarning($"[{GetType().Name}] Prefab not found for name: {prefabName}");
            return null;
        }

        private void OnValidate()
        {
            prefabCache = null;
        }
    }
}
