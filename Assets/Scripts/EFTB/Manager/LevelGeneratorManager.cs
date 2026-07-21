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

        private FurniturePlacementModel furniturePlacementModel = new FurniturePlacementModel();
        private int lastOpenLaneIndex = ConstGameplay.LevelGenerator.InitialLaneIndex;
        private float lastFurnitureWorldY = -999f;
        private List<GIFurnitureObstacle> activeFurnitureObstacles = new List<GIFurnitureObstacle>();
        private List<LevelSegmentData> segments;

        public void Initialize(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayTimeManager = GameContext.Instance.Get<GameplayTimeManager>();

            segments = new List<LevelSegmentData>();
            LaneXPositions = ConstGameplay.LevelGenerator.LaneXPositions;
            MaxSegmentAmount = ConstGameplay.LevelGenerator.MaxSegmentAmount;
            SegmentHeight = ConstGameplay.LevelGenerator.SegmentHeight;
            SegmentRecycleTriggerOffset = ConstGameplay.LevelGenerator.SegmentRecycleTriggerOffset;
            MediumDifficultyTimePercentage = ConstGameplay.LevelGenerator.MediumDifficultyTimePercentage;
            HardDifficultyTimePercentage = ConstGameplay.LevelGenerator.HardDifficultyTimePercentage;

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

        public bool IsCellBlockedByFurniture(int targetLaneIndex, float targetWorldY)
        {
            for (int i = 0; i < activeFurnitureObstacles.Count; i++)
            {
                GIFurnitureObstacle furniture = activeFurnitureObstacles[i];
                if (furniture != null && furniture.gameObject.activeInHierarchy && furniture.BlocksCell(targetLaneIndex, targetWorldY))
                {
                    return true;
                }
            }
            return false;
        }

        private ActiveSegment SpawnSegmentAt(float yPosition)
        {
            LevelSegmentData selectedTemplate = new LevelSegmentData(
                ConstGameplay.LevelGenerator.InitialSegmentId,
                ConstGameplay.LevelGenerator.DefaultInitialSegmentPrefab,
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
                GIFurnitureObstacle giFurniture = visualizer.SpawnFurnitureObstacle(block, yPosition, segmentInstance, giSegment);
                if (giFurniture != null)
                {
                    activeFurnitureObstacles.Add(giFurniture);
                }
            }

            activeSegmentQueue.Enqueue(instance);
            return instance;
        }
    }
}
