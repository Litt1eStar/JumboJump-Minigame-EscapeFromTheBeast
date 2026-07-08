using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJump.EFTB.Constant.UI;

namespace JumboJumps.EFTB.Visualizer.LevelGenerator
{
    public class LevelGeneratorVisualizer
    {
        private LevelGeneratorManager levelGeneratorManager;
        private ObjectPoolManager poolManager;
        private Queue<GameObject> activeSegments = new();

        private GameDataManager gameDataManager;
        private WarningIndicatorManager warningIndicatorManager;
        private Camera mainCamera;
        private float[] laneXPosition;

        public LevelGeneratorVisualizer(GameDataManager gameDataManager, float[] laneXPosition)
        {
            this.gameDataManager = gameDataManager;
            this.laneXPosition = laneXPosition;
        }

        public void Initialize()
        {
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            if (poolManager == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find ObjectPoolManager in GameContex");
                return;
            }


            warningIndicatorManager = GameContext.Instance.Get<WarningIndicatorManager>();
            if(warningIndicatorManager == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find WarningIndicatorManager in GameContex");
                return;
            }

            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = SceneObjectContext.Instance.Get<Camera>();
            }

            if (mainCamera == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] Failed to find Main Camera in the scene");
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

            if (poolManager == null || template == null || template.SegmentPrefabName == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] SpawnSegment failed : Missing Instance");
                return null;
            }

            Vector3 position = new Vector3(0, yPosition, 0);

            GameObject segmentPrefab = gameDataManager.GetPrefab(template.SegmentPrefabName);
            if (segmentPrefab == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] SpawnSegment failed : Prefab '{template.SegmentPrefabName}' not found in registry.");
                return null;
            }
            
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

        private void SetupSleepyCat(GameObject catObj, float spawnX)
        {
            var giCat = catObj.GetComponent<GICat>();
            if (giCat != null)
            {
                if (SceneObjectContext.Instance != null)
                {
                    SceneObjectContext.Instance.Register(giCat);
                }

                CatSightDirection direction = (spawnX < 0f) ? CatSightDirection.Right : CatSightDirection.Left;
                giCat.SetDirection(direction);

                var catManager = GameContext.Instance.Get<CatManager>();
                var playerManager = GameContext.Instance.Get<PlayerManager>();
                if (catManager != null && playerManager != null)
                {
                    catManager.RegisterDynamicCat(giCat, playerManager.PlayerTransform);
                }
            }
        }

        private void SpawnPrePlacedObject(LevelGeneratorData.LevelSegmentData template, float yPosition, GameObject segment, GISegment giSegment)
        {
            if (template.PrePlacedObject != null)
            {
                foreach (LevelGeneratorData.LaneObjectData objectData in template.PrePlacedObject)
                {
                    bool isSleepyCat = objectData.PrefabName == "Prefab_Event_SleepyCat";
                    float targetX = 0f;

                    if (isSleepyCat)
                    {
                        targetX = (objectData.LaneIndex <= 2)
                            ? ConstGameplay.Cat.CatLeftLaneSpawnPosition
                            : ConstGameplay.Cat.CatRightLaneSpawnPosition;
                    }
                    else
                    {
                        int laneIdx = Mathf.Clamp(objectData.LaneIndex, 0, laneXPosition.Length - 1);
                        targetX = laneXPosition[laneIdx];
                    }

                    Vector3 spawnPosition = new Vector3(targetX, yPosition + objectData.YOffset, 0f);
                    GameObject prefab = gameDataManager.GetPrefab(objectData.PrefabName);

                    if (prefab == null) continue;

                    GameObject spawnedObj = poolManager.Spawn(prefab, spawnPosition, Quaternion.identity, segment.transform);

                    if (isSleepyCat)
                    {
                        SetupSleepyCat(spawnedObj, targetX);
                    }

                    giSegment.RegisterSpawnedObject(spawnedObj);
                }
            }
        }

        public void SpawnEventObstacle(LevelGeneratorData.LaneEventData eventData, float segmentYPosition, GameObject segment, GISegment giSegment)
        {
            if (poolManager == null || eventData == null || string.IsNullOrEmpty(eventData.PrefabName)) return;

            bool isSleepyCat = eventData.PrefabName == "Prefab_Event_SleepyCat";
            float targetX = 0f;
            float spawnY = 0f;

            if (isSleepyCat)
            {
                targetX = (eventData.TargetLaneIndex <= 2)
                    ? ConstGameplay.Cat.CatLeftLaneSpawnPosition
                    : ConstGameplay.Cat.CatRightLaneSpawnPosition;

                spawnY = segmentYPosition + eventData.TriggerYOffset;
                SpawnObstacleInstance(eventData, targetX, spawnY, segment, giSegment, isSleepyCat);
            }
            else
            {
                int laneIdx = Mathf.Clamp(eventData.TargetLaneIndex, 0, laneXPosition.Length - 1);
                targetX = laneXPosition[laneIdx];
                float originalSegmentY = segment != null ? segment.transform.position.y : 0f;

                // Spawn warning indicator for 1.5s before spawning the actual lane obstacle
                if (warningIndicatorManager != null)
                {
                    warningIndicatorManager.ShowWarning(laneIdx, ConstUI.Gameplay.WarningIndicatorDuration, () =>
                    {
                        HandleSpawnObstacle(eventData, segment, giSegment, isSleepyCat, targetX, originalSegmentY);
                    });
                }
                else
                {
                    HandleSpawnObstacle(eventData, segment, giSegment, isSleepyCat, targetX, originalSegmentY);
                }
            }
        }

        private void HandleSpawnObstacle(LevelGeneratorData.LaneEventData eventData, GameObject segment, GISegment giSegment, bool isSleepyCat, float targetX, float originalSegmentY)
        {
            float currentSpawnY = 0f;
            Camera mainCam = mainCamera;
            if (mainCam == null)
            {
                mainCam = Camera.main ?? UnityEngine.Object.FindAnyObjectByType<Camera>();
            }

            if (mainCam != null)
            {
                float verticalHalfSize = mainCam.orthographic
                    ? mainCam.orthographicSize
                    : Mathf.Abs(mainCam.transform.position.z) * Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                float safetyOffset = Mathf.Max(5f, verticalHalfSize * 0.4f);
                currentSpawnY = mainCam.transform.position.y + verticalHalfSize + safetyOffset;
            }
            else
            {
                DebugLogHelper.LogError($"[{GetType().Name}] Can't find Camera in this Scene");
                return;
            }

            GISegment targetGiSegment = null;
            GameObject targetSegmentGo = null;
            if (levelGeneratorManager != null)
            {
                targetGiSegment = levelGeneratorManager.GetGISegmentAtY(currentSpawnY);
                if (targetGiSegment != null)
                {
                    targetSegmentGo = targetGiSegment.gameObject;
                }
            }

            // Fallback to the original segment if the far-ahead segment is not found
            if (targetSegmentGo == null)
            {
                if (segment == null || !segment.activeInHierarchy)
                {
                    DebugLogHelper.LogWarning($"[{GetType().Name}] Aborting obstacle spawn because both target and original segments are inactive.");
                    return;
                }
                targetSegmentGo = segment;
                targetGiSegment = giSegment;
            }

            SpawnObstacleInstance(eventData, targetX, currentSpawnY, targetSegmentGo, targetGiSegment, isSleepyCat);
        }

        private void SpawnObstacleInstance(LevelGeneratorData.LaneEventData eventData, float targetX, float spawnY, GameObject segment, GISegment giSegment, bool isSleepyCat)
        {
            if (poolManager == null || eventData == null || string.IsNullOrEmpty(eventData.PrefabName)) return;

            Vector3 spawnPosition = new Vector3(targetX, spawnY, 0f);
            GameObject prefab = gameDataManager.GetPrefab(eventData.PrefabName);

            if (prefab == null)
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Prefab not found for event: {eventData.PrefabName}");
                return;
            }

            GameObject spawnedObj = poolManager.Spawn(prefab, spawnPosition, Quaternion.identity, segment.transform);

            if (isSleepyCat)
            {
                SetupSleepyCat(spawnedObj, targetX);
            }
            else
            {
                var movingObstacle = spawnedObj.GetComponent<GIMovingObstacle>();
                if (movingObstacle == null)
                {
                    movingObstacle = spawnedObj.AddComponent<GIMovingObstacle>();
                }
                movingObstacle.Initialize(eventData.Speed);
            }

            if (giSegment != null)
            {
                giSegment.RegisterSpawnedObject(spawnedObj);
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
                        var giCat = spawnedObjs[i].GetComponent<GICat>();
                        if (giCat != null)
                        { 
                            SceneObjectContext.Instance.Deregister(giCat);
                           
                            var catManager = GameContext.Instance.Get<CatManager>();
                            if (catManager != null)
                            {
                                catManager.DeregisterCat(giCat);
                            }
                        }
                        poolManager.Recycle(spawnedObjs[i]);
                    }
                }
                giSegment.ClearSpawnedObjects();
            }

            poolManager.Recycle(oldestSegment);
        }
    }
}