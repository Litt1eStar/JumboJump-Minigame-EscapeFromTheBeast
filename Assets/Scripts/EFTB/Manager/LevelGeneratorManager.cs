using JumboJumps.EFTB.GameData.LevelSegment;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.LevelGenerator;
using LevelSegmentData = JumboJumps.EFTB.Model.LevelGeneratorData.LevelSegmentData;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Model.Obstacle;

namespace JumboJumps.EFTB.Manager
{
    public class LevelGeneratorManager
    {
        private GameDataManager gameDataManager;
        private GameplayTimeManager gameplayTimeManager;
        private LevelGeneratorConfig config;
        private LevelGeneratorVisualizer visualizer;
        public LevelGeneratorConfig Config => config;

        private class ActiveSegment
        {
            public LevelSegmentData Template { get; }
            public float SpawnY { get; }
            public GameObject SegmentGo { get; }
            public GISegment GiSegment { get; }
            public List<LevelGeneratorData.LaneEventData> PendingEvents { get; }

            public ActiveSegment(LevelSegmentData template, float spawnY, GameObject segmentGo, GISegment giSegment)
            {
                Template = template;
                SpawnY = spawnY;
                SegmentGo = segmentGo;
                GiSegment = giSegment;
                PendingEvents = template.LaneEventData != null
                    ? new List<LevelGeneratorData.LaneEventData>(template.LaneEventData)
                    : new List<LevelGeneratorData.LaneEventData>();

                // Sort events by TriggerYOffset in ascending order to optimize trigger checks
                PendingEvents.Sort((a, b) => a.TriggerYOffset.CompareTo(b.TriggerYOffset));
            }
        }

        private Transform playerTransform;
        private float nextTriggerPosition;
        private float nextYSpawnPosition;
        private Queue<ActiveSegment> activeSegmentQueue = new();

        public float[] LaneXPositions { get; private set; }
        public int MaxSegmentAmount { get; private set; }
        public float SegmentHeight { get; private set; }
        public float SegmentRecycleTriggerOffset { get; private set; }
        public float MediumDifficultyTimePercentage { get; private set; }
        public float HardDifficultyTimePercentage { get; private set; }
        public float MaxGeneratedWorldY => nextYSpawnPosition;

        private FurniturePlacementModel furniturePlacementModel = new FurniturePlacementModel();
        private int lastOpenLaneIndex = ConstGameplay.LevelGenerator.Initial_Lane_Index;
        private float lastFurnitureWorldY = -999f;
        private List<GIFurnitureObstacle> activeFurnitureObstacles = new List<GIFurnitureObstacle>();
        private List<LevelSegmentData> segments;

        public void Initialize(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayTimeManager = GameContext.Instance.Get<GameplayTimeManager>();

            segments = new List<LevelSegmentData>();
            LaneXPositions = ConstGameplay.LevelGenerator.Lane_X_Positions;
            MaxSegmentAmount = ConstGameplay.LevelGenerator.Max_Segment_Amount;
            SegmentHeight = ConstGameplay.LevelGenerator.Segment_Height;
            SegmentRecycleTriggerOffset = ConstGameplay.LevelGenerator.Segment_Recycle_Trigger_Offset;
            MediumDifficultyTimePercentage = ConstGameplay.LevelGenerator.Medium_Difficulty_Time_Percentage;
            HardDifficultyTimePercentage = ConstGameplay.LevelGenerator.Hard_Difficulty_Time_Percentage;

            visualizer = new LevelGeneratorVisualizer(gameDataManager, LaneXPositions, this);
            visualizer.Initialize();

            config = new LevelGeneratorConfig(segments,
                                              LaneXPositions,
                                              MaxSegmentAmount,
                                              SegmentHeight,
                                              SegmentRecycleTriggerOffset,
                                              MediumDifficultyTimePercentage,
                                              HardDifficultyTimePercentage);

            nextTriggerPosition = SegmentHeight + config.SegmentRecycleTriggerOffset;
            nextYSpawnPosition = SegmentHeight * config.MaxSegmentAmount;

            GameContext.Instance.Add(this);

            for (int i = 0; i < config.MaxSegmentAmount; i++)
            {
                float spawnYPosition = i * SegmentHeight;
                SpawnSegmentAt(spawnYPosition);
            }
        }

        public void Dispose()
        {
            visualizer?.Dispose();
            visualizer = null;

            activeSegmentQueue.Clear();
            activeFurnitureObstacles.Clear();
            lastFurnitureWorldY = -999f;
            lastOpenLaneIndex = ConstGameplay.LevelGenerator.Initial_Lane_Index;
            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            float playerY = playerTransform != null ? playerTransform.position.y : 0f;

            if (playerY >= nextTriggerPosition)
            {
                RecycleSegment();
            }
        }

        public GISegment GetGISegmentAtY(float y)
        {
            foreach (var activeSegment in activeSegmentQueue)
            {
                if (y >= activeSegment.SpawnY && y < activeSegment.SpawnY + SegmentHeight)
                {
                    return activeSegment.GiSegment;
                }
            }
            return null;
        }

        private void RecycleSegment()
        {
            if (activeSegmentQueue.Count == 0)
            {
                DebugLogHelper.LogWarning("[LevelGeneratorManager] RecycleSegment called but activeSegmentQueue is empty!");
                return;
            }

            visualizer.RecycleOldestSegment();
            activeSegmentQueue.Dequeue();

            // Purge recycled furniture references
            activeFurnitureObstacles.RemoveAll(f => f == null || !f.gameObject.activeInHierarchy);

            SpawnSegmentAt(nextYSpawnPosition);

            nextTriggerPosition += SegmentHeight;
            nextYSpawnPosition += SegmentHeight;
        }

        public void RegisterFurnitureObstacle(GIFurnitureObstacle furniture)
        {
            if (furniture != null && !activeFurnitureObstacles.Contains(furniture))
            {
                activeFurnitureObstacles.Add(furniture);
            }
        }

        public void UnregisterFurnitureObstacle(GIFurnitureObstacle furniture)
        {
            if (furniture != null)
            {
                activeFurnitureObstacles.Remove(furniture);
            }
        }

        public bool IsValidFurnitureSpawn(int laneIndex, float worldY)
        {
            if (laneIndex < 0 || laneIndex >= LaneXPositions.Length) return false;
            if (worldY <= ConstGameplay.Obstacle.Safe_Zone_Cells * ConstGameplay.Obstacle.Furniture.Cell_Height) return false;

            if (IsCellBlockedByFurniture(laneIndex, worldY)) return false;

            return true;
        }

        public bool IsCellBlockedByFurniture(int targetLaneIndex, float targetWorldY)
        {
            for (int i = activeFurnitureObstacles.Count - 1; i >= 0; i--)
            {
                if (i >= activeFurnitureObstacles.Count) continue;
                GIFurnitureObstacle furniture = activeFurnitureObstacles[i];
                if (furniture != null && furniture.gameObject.activeInHierarchy && furniture.BlocksCell(targetLaneIndex, targetWorldY))
                {
                    return true;
                }
            }

            // Self-healing fallback: Query the active GISegment at targetWorldY for active furniture
            GISegment segment = GetGISegmentAtY(targetWorldY);
            if (segment != null)
            {
                var furnitureList = segment.GetComponentsInChildren<GIFurnitureObstacle>(false);
                foreach (var furniture in furnitureList)
                {
                    if (furniture != null && furniture.gameObject.activeInHierarchy)
                    {
                        RegisterFurnitureObstacle(furniture);
                        if (furniture.BlocksCell(targetLaneIndex, targetWorldY))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private ActiveSegment SpawnSegmentAt(float yPosition)
        {
            LevelSegmentData selectedTemplate = new LevelSegmentData(
                ConstGameplay.LevelGenerator.Initial_Segment_Id,
                ConstGameplay.LevelGenerator.Default_Initial_Segment_Prefab,
                SegmentHeight,
                SegmentDifficultyEnum.Easy,
                new List<LevelGeneratorData.LaneObjectData>(),
                new List<LevelGeneratorData.LaneEventData>()
            );

            GameObject segmentInstance = visualizer.SpawnSegment(selectedTemplate, yPosition);

            if (segmentInstance == null) return null;

            GISegment giSegment = segmentInstance.GetComponent<GISegment>();
            if (giSegment == null)
            {
                DebugLogHelper.LogError($"GISegment component not found on spawned segment instance at Y: {yPosition}");
                return null;
            }

            var instance = new ActiveSegment(selectedTemplate, yPosition, segmentInstance, giSegment);

            // Scan and register any pre-baked or static GIFurnitureObstacle instances in the segment hierarchy
            GIFurnitureObstacle[] prefabFurniture = segmentInstance.GetComponentsInChildren<GIFurnitureObstacle>(true);
            foreach (var furniture in prefabFurniture)
            {
                if (furniture != null)
                {
                    furniture.UpdateWorldPositionAndLane();
                    RegisterFurnitureObstacle(furniture);
                }
            }

            // Procedurally generate furniture blocks for this segment
            var furnitureBlocks = furniturePlacementModel.GenerateSegmentFurniture(
                yPosition,
                SegmentHeight,
                LaneXPositions.Length,
                ref lastOpenLaneIndex,
                ref lastFurnitureWorldY
            );

            foreach (var block in furnitureBlocks)
            {
                float worldY = yPosition + block.YOffset;
                if (!IsValidFurnitureSpawn(block.LaneIndex, worldY))
                {
                    continue;
                }

                GIFurnitureObstacle giFurniture = visualizer.SpawnFurnitureObstacle(block, yPosition, segmentInstance, giSegment);
                if (giFurniture != null)
                {
                    RegisterFurnitureObstacle(giFurniture);
                }
            }

            activeSegmentQueue.Enqueue(instance);
            return instance;
        }
    }
}
