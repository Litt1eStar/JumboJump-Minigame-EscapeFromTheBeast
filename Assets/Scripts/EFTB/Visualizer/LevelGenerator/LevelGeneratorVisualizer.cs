using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Visualizer.LevelGenerator
{
    public class LevelGeneratorVisualizer
    {
        private GILevelGenerator gILevelGenerator;
        private ObjectPoolManager poolManager;

        public void Initialize()
        {
            gILevelGenerator = SceneObjectContext.Instance.Get<GILevelGenerator>(); 

            if (gILevelGenerator == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find GILevelGenerator in SceneObjectContext");
                return;
            }

            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            
            if (poolManager == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find ObjectPoolManager in GameContext");
                return;
            }
        }

        public void Dispose() 
        {
            gILevelGenerator = null;
        }

        public GameObject SpawnSegment(GameObject segmentPrefab, float yPosition)
        {
            /// <summary>
            /// segmentPrefab : prefab to Spawn
            /// yPosition : y position to spawn an object, note -> we set x, z position to 0 for segment spawn
            /// </summary>

            if (gILevelGenerator == null || poolManager == null)
            {
                return null;
            }

            Vector3 position = new Vector3(0, yPosition, 0);
            GameObject segment = poolManager.Spawn(segmentPrefab, position, Quaternion.identity, gILevelGenerator.transform);
            return segment;
        }

        public void RecycleSegment(GameObject segment)
        {
            if (poolManager == null || segment == null) return;

            poolManager.Recycle(segment);
        }
    }
}
