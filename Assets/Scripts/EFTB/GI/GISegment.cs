using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GISegment : MonoBehaviour
    {
        private readonly List<GameObject> spawnedObjects = new();

        /// <summary>
        /// A read-only list of all currently active spawned objects on this segment.
        /// </summary>
        public IReadOnlyList<GameObject> SpawnedObjects => spawnedObjects;

        public void RegisterSpawnedObject(GameObject obj)
        {
            if (obj != null && !spawnedObjects.Contains(obj))
            {
                spawnedObjects.Add(obj);
            }
        }

        public void DeregisterSpawnedObject(GameObject obj)
        {
            if (obj != null)
            {
                spawnedObjects.Remove(obj);
            }
        }

        public void ClearSpawnedObjects()
        {
            spawnedObjects.Clear();
        }
    }
}
