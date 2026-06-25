using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class ObjectPoolManager
    {
        private Dictionary<string, Queue<PoolableObject>> pools = new();
        private Transform poolContainer;

        public void Initialize()
        {
            var containerGo = new GameObject("ObjectPoolContainer");
            GameObject.DontDestroyOnLoad(containerGo);
            poolContainer = containerGo.transform;

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            if(poolContainer != null)
            {
                GameObject.Destroy(poolContainer.gameObject);
            }

            pools.Clear();
            GameContext.Instance.Remove(this);
        }

        public GameObject Spawn(
            GameObject prefab, 
            Vector3 position, 
            Quaternion rotation, 
            Transform parent = null)
        {
            string key = prefab.name;
            PoolableObject poolableObject = null;

            if(pools.TryGetValue(key, out var queue) && queue.Count > 0)
            {
                poolableObject = queue.Dequeue();
                poolableObject.transform.position = position;
                poolableObject.transform.rotation = rotation;
                poolableObject.transform.SetParent(parent);
            }
            else
            {
                var instance = GameObject.Instantiate(prefab, position, rotation, parent);
                poolableObject = instance.GetComponent<PoolableObject>();
                if(poolableObject == null)
                {
                    poolableObject = instance.AddComponent<PoolableObject>();
                }
                poolableObject.PoolKey = key;
            }

            poolableObject.OnSpawn();
            return poolableObject.gameObject;
        }

        public void Recycle(GameObject instance)
        {
            if (instance == null) return;

            var poolableObject = instance.GetComponent<PoolableObject>();

            if(poolableObject == null || string.IsNullOrEmpty(poolableObject.PoolKey))
            {
                GameObject.Destroy(instance);
                return;
            }

            poolableObject.OnRecycle();
            poolableObject.transform.SetParent(poolContainer);

            if(!pools.TryGetValue(poolableObject.PoolKey, out var queue))
            {
                queue = new Queue<PoolableObject>();
                pools.Add(poolableObject.PoolKey, queue);
            }

            queue.Enqueue(poolableObject);
        }
    }
}
