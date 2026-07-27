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

        public LevelGeneratorVisualizer(GameDataManager gameDataManager, float[] laneXPosition, LevelGeneratorManager levelGeneratorManager)
        {
            this.gameDataManager = gameDataManager;
            this.laneXPosition = laneXPosition;
            this.levelGeneratorManager = levelGeneratorManager;
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

            if (warningIndicatorManager == null)
            {
                DebugLogHelper.LogError($"{GetType().Name} Failed to find WarningIndicatorManager in GameContex");
                return;
            }

            mainCamera = GetMainCamera();

            if (mainCamera == null)
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Failed to find Main Camera during Initialize, will attempt to find later.");
            }
        }

        private Camera GetMainCamera()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main ?? SceneObjectContext.Instance?.Get<Camera>() ?? UnityEngine.Object.FindAnyObjectByType<Camera>();
            }
            return mainCamera;
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

            if (!gameDataManager.TryGetPrefab(template.SegmentPrefabName, out GameObject segmentPrefab))
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
                    if (!gameDataManager.TryGetPrefab(objectData.PrefabName, out GameObject prefab)) continue;

                    bool isCat = prefab.GetComponent<GICat>() != null;
                    if (isCat) continue;

                    int laneIdx = Mathf.Clamp(objectData.LaneIndex, 0, laneXPosition.Length - 1);
                    float targetX = laneXPosition[laneIdx];

                    Vector3 spawnPosition = new Vector3(targetX, yPosition + objectData.YOffset, 0f);
                    GameObject spawnedObj = poolManager.Spawn(prefab, spawnPosition, Quaternion.identity, segment.transform);
                    giSegment.RegisterSpawnedObject(spawnedObj);
                }
            }
        }

        public void SpawnEventObstacle(LevelGeneratorData.LaneEventData eventData,
                                       float segmentYPosition,
                                       GameObject segment,
                                       GISegment giSegment)
        {
            if (gameDataManager == null || eventData == null) return;

            if (!gameDataManager.TryGetPrefab(eventData.PrefabName, out GameObject prefab))
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Prefab not found for event obstacle: {eventData.PrefabName}");
                return;
            }

            int laneIdx = Mathf.Clamp(eventData.TargetLaneIndex, 0, laneXPosition.Length - 1);
            float targetX = laneXPosition[laneIdx];

            bool isCat = prefab.GetComponent<GICat>() != null;
            if (isCat)
            {
                targetX = GetCatSpawnX(laneIdx);
            }

            warningIndicatorManager?.ShowWarning(laneIdx, 1.5f, () =>
            {
                ExecuteEventObstacleSpawn(eventData, segment, giSegment, targetX, segmentYPosition, prefab);
            });
        }

        private void ExecuteEventObstacleSpawn(LevelGeneratorData.LaneEventData eventData,
                                         GameObject segment,
                                         GISegment giSegment,
                                         float targetX,
                                         float originalSegmentY,
                                         GameObject prefab)
        {
            Camera mainCam = GetMainCamera();

            if (mainCam == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] Main Camera reference is null");
                return;
            }

            float verticalHalfSize = mainCam.orthographic
                ? mainCam.orthographicSize
                : Mathf.Abs(mainCam.transform.position.z) * Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float safetyOffset = Mathf.Max(5f, verticalHalfSize * 0.4f);
            float currentSpawnY = mainCam.transform.position.y + verticalHalfSize + safetyOffset;

            if (segment == null || !segment.activeInHierarchy)
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Aborting obstacle spawn because the segment is inactive.");
                return;
            }

            SpawnObstacleInstance(eventData, targetX, currentSpawnY, segment, giSegment, prefab);
        }

        private void SpawnObstacleInstance(LevelGeneratorData.LaneEventData eventData,
                                           float targetX,
                                           float spawnY,
                                           GameObject segment,
                                           GISegment giSegment,
                                           GameObject prefab)
        {
            if (poolManager == null || eventData == null || prefab == null) return;

            Vector3 spawnPosition = new Vector3(targetX, spawnY, 0f);
            GameObject spawnedObj = poolManager.Spawn(prefab, spawnPosition, Quaternion.identity, segment.transform);

            var giCat = spawnedObj.GetComponent<GICat>();
            if (giCat != null)
            {
                // If the spawned object is a cat, set it up accordingly
                SetupSleepyCat(spawnedObj, targetX);
            }
            else
            {
                // If the spawned object is a moving obstacle, initialize its movement speed
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

        private float GetCatSpawnX(int laneIndex)
        {
            return (laneIndex < laneXPosition.Length / ConstGameplay.LevelGenerator.LANE_SIZE)
                ? ConstGameplay.Cat.CAT_LEFT_LANE_SPAWN_POSITION
                : ConstGameplay.Cat.CAT_RIGHT_LANE_SPAWN_POSITION;
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