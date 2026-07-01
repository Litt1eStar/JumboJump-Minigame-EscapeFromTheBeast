using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    public class SceneObjectContext : MonoBehaviour
    {
        public static SceneObjectContext Instance { get; private set; }

        [SerializeField]
        private MonoBehaviour[] sceneObjects;

        private Dictionary<Type, MonoBehaviour> objects;
        private Dictionary<Type, List<MonoBehaviour>> objectGroups;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            Initialize();
        }

        public void Initialize()
        {
            objects = new Dictionary<Type, MonoBehaviour>();
            objectGroups = new Dictionary<Type, List<MonoBehaviour>>();

            foreach (MonoBehaviour obj in sceneObjects)
            {
                Type key = obj.GetType();

                if (!objects.ContainsKey(key))
                {
                    objects.Add(key, obj);
                }
                else
                {
                    DebugLogHelper.LogWarning($"Duplicate type {key} found in SceneObjectContext. Only the first instance will be stored.");
                }

                if (!objectGroups.TryGetValue(key, out var list))
                {
                    list = new List<MonoBehaviour>();
                    objectGroups.Add(key, list);
                }
                list.Add(obj);
            }
        }

        public void Dispose()
        {
            objects = null;
            objectGroups = null;
        }

        public T Get<T>() where T : class
        {
            objects.TryGetValue(typeof(T), out var result);
            return result as T;
        }

        public IReadOnlyList<T> GetAll<T>() where T : class
        {
            if (!objectGroups.TryGetValue(typeof(T), out var list))
                return Array.Empty<T>();

            var typed = new List<T>(list.Count);
            foreach (var mb in list)
            {
                if (mb is T t)
                {
                    typed.Add(t);
                }
            }
            return typed;
        }

        public void Register(MonoBehaviour obj)
        {
            if (obj == null) return;
            Type key = obj.GetType();

            if (objects == null) objects = new Dictionary<Type, MonoBehaviour>();
            if (objectGroups == null) objectGroups = new Dictionary<Type, List<MonoBehaviour>>();

            if (!objects.ContainsKey(key))
            {
                objects.Add(key, obj);
            }

            if (!objectGroups.TryGetValue(key, out var list))
            {
                list = new List<MonoBehaviour>();
                objectGroups.Add(key, list);
            }
            if (!list.Contains(obj))
            {
                list.Add(obj);
            }
        }

        public void Deregister(MonoBehaviour obj)
        {
            if (obj == null || objects == null || objectGroups == null) return;
            Type key = obj.GetType();

            if (objects.TryGetValue(key, out var stored) && stored == obj)
            {
                objects.Remove(key);
                if (objectGroups.TryGetValue(key, out var list) && list.Count > 1)
                {
                    foreach (var item in list)
                    {
                        if (item != obj)
                        {
                            objects.Add(key, item);
                            break;
                        }
                    }
                }
            }

            if (objectGroups.TryGetValue(key, out var groupList))
            {
                groupList.Remove(obj);
            }
        }
    }
}
