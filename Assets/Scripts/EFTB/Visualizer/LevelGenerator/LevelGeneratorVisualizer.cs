using JumboJump.Assets.Scripts.EFTB.Manager;
using JumboJump.EFTB.GI;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJump.EFTB.Visualizer.LevelGenerator
{
    public class LevelGeneratorVisualizer
    {
        private GILevelGenerator gILevelGenerator;
        private ObjectPoolManager poolManager;
        private Queue<GameObject> activeSegments = new();

        public void Initialize()
        {
            gILevelGenerator = SceneObjectContext.Instance.Get<GILevelGenerator>(); 
            if(gILevelGenerator == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find GILevelGenerator in SceneObjectContext");
                return;
            }

            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            if(poolManager == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find ObjectPoolManager in GameContex");
                return;
            }
 
        }

        public void Dispose() 
        {
            gILevelGenerator = null;
            poolManager = null;
            activeSegments.Clear();
        }

        public GameObject SpawnSegment(GameObject segmentPrefab, float yPosition)
        {
            /// <summary>
            /// segmentPrefab : prefab to Spawn
            /// yPosition : y position to spawn an object, note -> we set x, z position to 0 for segment spawn
            /// </summary>

            if(gILevelGenerator == null || poolManager == null)
            {
                return null;
            }

            Vector3 position = new Vector3(0, yPosition, 0);
            GameObject segment = poolManager.Spawn(segmentPrefab, position, Quaternion.identity, gILevelGenerator.transform);
            activeSegments.Enqueue(segment);
            return segment;
        }

        public void RecycleSegment()
        {
            if (activeSegments.Count <= 0 || poolManager == null) return;

            GameObject oldestSegment = activeSegments.Dequeue();
            poolManager.Recycle(oldestSegment);
        }
    }
}
