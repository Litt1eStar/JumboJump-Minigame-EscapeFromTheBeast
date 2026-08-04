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
            public List<(int laneIndex, int rowIndex)> BlockedCells { get; } = new List<(int, int)>();

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

        private FurniturePlacementModel furniturePlacementModel = new FurniturePlacementModel();
        private int lastOpenLaneIndex = ConstGameplay.LevelGenerator.INITIAL_LANE_INDEX;
        private float lastFurnitureWorldY = -999f;
        private List<GIFurnitureObstacle> activeFurnitureObstacles = new List<GIFurnitureObstacle>();
        private readonly HashSet<(int laneIndex, int rowIndex)> blockedCells = new HashSet<(int laneIndex, int rowIndex)>();
        private List<LevelSegmentData> segments;

        public void Initialize(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayTimeManager = GameContext.Instance.Get<GameplayTimeManager>();

            segments = new List<LevelSegmentData>();
            LaneXPositions = ConstGameplay.LevelGenerator.LANE_X_POSITIONS;
            MaxSegmentAmount = ConstGameplay.LevelGenerator.MAX_SEGMENT_AMOUNT;
            SegmentHeight = ConstGameplay.LevelGenerator.SEGMENT_HEIGHT;
            SegmentRecycleTriggerOffset = ConstGameplay.LevelGenerator.SEGMENT_RECYCLE_TRIGGER_OFFSET;
            MediumDifficultyTimePercentage = ConstGameplay.LevelGenerator.MEDIUM_DIFFICULTY_TIME_PERCENTAGE;
            HardDifficultyTimePercentage = ConstGameplay.LevelGenerator.HARD_DIFFICULTY_TIME_PERCENTAGE;

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

            for (int i = 0; i < config.MaxSegmentAmount; i++)
            {
                float spawnYPosition = i * SegmentHeight;
                SpawnSegmentAt(spawnYPosition);
            }

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            visualizer?.Dispose();
            visualizer = null;

            activeSegmentQueue.Clear();
            activeFurnitureObstacles.Clear();
            blockedCells.Clear();
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
            ActiveSegment oldestSegment = activeSegmentQueue.Dequeue();

            foreach (var cell in oldestSegment.BlockedCells)
            {
                blockedCells.Remove(cell);
            }

            // Purge recycled furniture references
            activeFurnitureObstacles.RemoveAll(f => f == null || !f.gameObject.activeInHierarchy);

            SpawnSegmentAt(nextYSpawnPosition);

            nextTriggerPosition += SegmentHeight;
            nextYSpawnPosition += SegmentHeight;
        }

        public bool IsCellBlockedByFurniture(int targetLaneIndex, float targetWorldY)
        {
            int rowIndex = Mathf.RoundToInt(targetWorldY / ConstGameplay.Obstacle.Furniture.CELL_HEIGHT);
            return blockedCells.Contains((targetLaneIndex, rowIndex));
        }

        private ActiveSegment SpawnSegmentAt(float yPosition)
        {
            LevelSegmentData selectedTemplate = new LevelSegmentData(ConstGameplay.LevelGenerator.INITIAL_SEGMENT_ID,
                                                                     ConstGameplay.LevelGenerator.DEFAULT_INITIAL_SEGMENT_PREFAB,
                                                                     SegmentHeight,
                                                                     SegmentDifficultyEnum.Easy,
                                                                     new List<LevelGeneratorData.LaneObjectData>(),
                                                                     new List<LevelGeneratorData.LaneEventData>());

            GameObject segmentInstance = visualizer.SpawnSegment(selectedTemplate, yPosition);

            if (segmentInstance == null) return null;

            GISegment giSegment = segmentInstance.GetComponent<GISegment>();
            if (giSegment == null)
            {
                DebugLogHelper.LogError($"GISegment component not found on spawned segment instance at Y: {yPosition}");
                return null;
            }

            var instance = new ActiveSegment(selectedTemplate, yPosition, segmentInstance, giSegment);

            // Procedurally generate furniture blocks for this segment
            var furnitureBlocks = furniturePlacementModel.GenerateSegmentFurniture(yPosition,
                                                                                   SegmentHeight,
                                                                                   LaneXPositions.Length,
                                                                                   ref lastOpenLaneIndex,
                                                                                   ref lastFurnitureWorldY);

            foreach (var block in furnitureBlocks)
            {
                GIFurnitureObstacle giFurniture = visualizer.SpawnFurnitureObstacle(block, yPosition, segmentInstance, giSegment);
                if (giFurniture != null)
                {
                    activeFurnitureObstacles.Add(giFurniture);

                    int rowIndex = Mathf.RoundToInt(giFurniture.WorldY / ConstGameplay.Obstacle.Furniture.CELL_HEIGHT);
                    var cellKey = (giFurniture.LaneIndex, rowIndex);
                    blockedCells.Add(cellKey);
                    instance.BlockedCells.Add(cellKey);
                }
            }

            activeSegmentQueue.Enqueue(instance);
            return instance;
        }
    }
}
