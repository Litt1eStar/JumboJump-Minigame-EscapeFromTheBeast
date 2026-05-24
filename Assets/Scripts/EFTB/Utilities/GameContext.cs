using System;
using System.Collections.Generic;

namespace Assets.Scripts.EFTB.Utilities
{
    public class GameContext
    {
        private static GameContext instance;
        public static GameContext Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new GameContext();
                }

                return instance;
            }
        }

        private readonly Dictionary<Type, object> objects = new();

        public void Add<T>(T obj) where T : class
        {
            Type key = typeof(T);
            if(!objects.ContainsKey(key))
            {
                objects.Add(key, obj);
            }
            else
            {
                DebugLogHelper.LogError($"[{GetType().Name}] {nameof(Add)}| Object of same key detected: {key.AssemblyQualifiedName}");
            }
        }

        public void Remove<T>(T obj) where T : class 
        {
            objects.Remove(typeof(T));
        }

        public T Get<T>(T obj) where T : class
        {
            objects.TryGetValue(typeof(T), out object result);
            return result as T;
        }

        public bool TryGet<T>(out T obj) where T : class
        {
            bool result = objects.TryGetValue(typeof(T), out object resultObj);
            obj = resultObj as T;
            return result;
        }
    }
}
