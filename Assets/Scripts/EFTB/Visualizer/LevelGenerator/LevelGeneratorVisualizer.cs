using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;
using JumboJumps.EFTB.Model;

namespace JumboJumps.EFTB.Visualizer.LevelGenerator
{
    public class LevelGeneratorVisualizer
    {
        private ObjectPoolManager poolManager;
        private Queue<GameObject> activeSegments = new();

        private LevelGeneratorManager levelGeneratorManager;
        private GameDataManager gameDataManager;

        public LevelGeneratorVisualizer(LevelGeneratorManager levelGeneratorManager, GameDataManager gameDataManager)
        {
            this.levelGeneratorManager = levelGeneratorManager;
            this.gameDataManager = gameDataManager;
        }

        public void Initialize()
        {
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            if(poolManager == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find ObjectPoolManager in GameContex");
                return;
            }
        }

        public void Dispose() 
        {
            activeSegments.Clear();
        }

        public GameObject SpawnSegment(LevelGeneratorData.LevelSegmentData template, float yPosition)
        {
            /// <summary>
            /// template : LevelSegmentSO template configuration containing prefab and layout data
            /// yPosition : y position to spawn the segment
            /// </summary>

            if (poolManager == null || template == null || template.segmentPrefabName == null)
            {
                return null;
            }

            Vector3 position = new Vector3(0, yPosition, 0);

            GameObject segmentPrefab = gameDataManager.GetPrefab(template.segmentPrefabName);
            
            GameObject segment = poolManager.Spawn(segmentPrefab, position, Quaternion.identity);

            GISegment giSegment = segment.GetComponent<GISegment>();
            if (giSegment == null)
            {
                giSegment = segment.AddComponent<GISegment>();
            }

            SpawnPrePlacedObject(template, yPosition, segment, giSegment);
            activeSegments.Enqueue(segment);

            return segment;
        }

        private void SpawnPrePlacedObject(LevelGeneratorData.LevelSegmentData template, float yPosition, GameObject segment, GISegment giSegment)
        {
            if (template.prePlacedObjectDatas != null)
            {
                float[] lanePositions = levelGeneratorManager.LaneXPositions;
                foreach (LevelGeneratorData.LaneObjectData objectData in template.prePlacedObjectDatas)
                {
                    int laneIdx = Mathf.Clamp(objectData.laneIndex, 0, lanePositions.Length - 1);
                    float targetX = lanePositions[laneIdx];

                    Vector3 spawnPosition = new Vector3(targetX, yPosition + objectData.yOffset, 0f);
                    GameObject prefab = gameDataManager.GetPrefab(objectData.prefabName);

                    if (prefab == null) continue;

                    GameObject spawnedObj = poolManager.Spawn(prefab, spawnPosition, Quaternion.identity, segment.transform);

                    giSegment.RegisterSpawnedObject(spawnedObj);
                }
            }
        }

        public void RecycleOldestSegment()
        {
            if (activeSegments.Count <= 0 || poolManager == null) return;

            GameObject oldestSegment = activeSegments.Dequeue();

            GISegment giSegment = oldestSegment.GetComponent<GISegment>();
            if (giSegment != null)
            {
                IReadOnlyList<GameObject> spawnedObjs = giSegment.SpawnedObjects;
                for (int i = 0; i < spawnedObjs.Count; i++)
                {
                    if (spawnedObjs[i] != null)
                    {
                        poolManager.Recycle(spawnedObjs[i]);
                    }
                }
                giSegment.ClearSpawnedObjects();
            }

            poolManager.Recycle(oldestSegment);
        }
    }
}
