using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.GameData.LevelSegment;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Visualizer.LevelGenerator
{
    public class LevelGeneratorVisualizer
    {
        private GILevelGenerator gILevelGenerator;
        private ObjectPoolManager poolManager;
        private Queue<GameObject> activeSegments = new();
        private float[] lanePositions;

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

            if (gILevelGenerator.configSo != null)
            {
                lanePositions = gILevelGenerator.configSo.laneXPositions;
            }
        }

        public void Dispose() 
        {
            gILevelGenerator = null;
            lanePositions = null;
            activeSegments.Clear();
        }

        public GameObject SpawnSegment(LevelSegmentSO template, float yPosition)
        {
            /// <summary>
            /// template : LevelSegmentSO template configuration containing prefab and layout data
            /// yPosition : y position to spawn the segment
            /// </summary>

            if (gILevelGenerator == null || poolManager == null || template == null || template.segmentPrefab == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] SpawnSegment failed : Missing Instance");
                return null;
            }

            Vector3 position = new Vector3(0, yPosition, 0);
            GameObject segment = poolManager.Spawn(template.segmentPrefab, position, Quaternion.identity, gILevelGenerator.transform);

            GISegment giSegment = segment.GetComponent<GISegment>();
            if (giSegment == null)
            {
                giSegment = segment.AddComponent<GISegment>();
            }

            SpawnPrePlacedObject(template, yPosition, segment, giSegment);
            activeSegments.Enqueue(segment);

            return segment;
        }

        private void SpawnPrePlacedObject(LevelSegmentSO template, float yPosition, GameObject segment, GISegment giSegment)
        {
            if (template.prePlacedObjectsData != null && lanePositions != null)
            {
                foreach (var objectData in template.prePlacedObjectsData)
                {
                    if (objectData.prefab == null) continue;

                    int laneIdx = Mathf.Clamp(objectData.laneIndex, 0, lanePositions.Length - 1);
                    float targetX = lanePositions[laneIdx];

                    Vector3 spawnPosition = new Vector3(targetX, yPosition + objectData.yOffset, 0f);

                    GameObject spawnedObj = poolManager.Spawn(objectData.prefab, spawnPosition, Quaternion.identity, segment.transform);

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
