using UnityEngine;
using System;
using System.Collections.Generic;
namespace JumboJumps.EFTB.GameData
{
    [CreateAssetMenu(fileName = "PrefabRegistry", menuName = "EFTB/PrefabRegistry")]
    public class PrefabRegistrySO : ScriptableObject
    {
        [Serializable]
        public struct PrefabEntry
        {
            public string key;
            public GameObject prefab;
        }

        public List<PrefabEntry> registry = new();
        public GameObject GetPrefab(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;

            var entry = registry.Find(e => e.key == key);

            if (entry.prefab == null)
            {
                Debug.LogWarning($"[{GetType().Name}] Prefab not found for key: {key}");
            }

            return entry.prefab;
        }
    }
}
