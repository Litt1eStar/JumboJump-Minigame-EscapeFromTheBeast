using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EFTB.Utilities
{
    public class SceneObjectContext : MonoBehaviour
    {
        public static SceneObjectContext Instance { get; private set; }

        [SerializeField]
        private MonoBehaviour[] sceneObjects;
        private Dictionary<Type, MonoBehaviour> objects;

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
            }
        }

        public void Dispose()
        {
            objects = null;
        }

        public T Get<T>() where T : class
        {
            objects.TryGetValue(typeof(T), out var result);
            return result as T;
        }
    }
}
