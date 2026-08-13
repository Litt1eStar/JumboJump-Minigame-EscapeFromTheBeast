using JumboJumps.EFTB.Config;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GameData.LevelSegment;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Model.Obstacle;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.LevelGenerator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LevelSegmentData = JumboJumps.EFTB.Model.LevelGeneratorData.LevelSegmentData;

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
        private CollectibleConfigSO collectibleConfig;
        public CollectibleConfigSO CollectibleConfig
        {
            get
            {
                if (collectibleConfig == null && SceneObjectContext.Instance != null)
                {
                    var container = SceneObjectContext.Instance.Get<GIGameplayConfigContainer>();
                    if (container != null && container.CollectibleConfig != null)
                    {
                        collectibleConfig = container.CollectibleConfig;
                    }
                }
                return collectibleConfig;
            }
        }
        private FurnitureConfigSO furnitureConfig;
        public FurnitureConfigSO FurnitureConfig
        {
            get
            {
                if (furnitureConfig == null && SceneObjectContext.Instance != null)
                {
                    var container = SceneObjectContext.Instance.Get<GIGameplayConfigContainer>();
                    if (container != null && container.FurnitureConfig != null)
                    {
                        furnitureConfig = container.FurnitureConfig;
                        furniturePlacementModel.Config = furnitureConfig;
                    }
                }
                return furnitureConfig;
            }
        }

        private int lastOpenLaneIndex = ConstGameplay.LevelGenerator.INITIAL_LANE_INDEX;
        private float lastFurnitureWorldY = -999f;
        private List<GIFurnitureObstacle> activeFurnitureObstacles = new List<GIFurnitureObstacle>();
        private List<GICollectible> activeTreats = new List<GICollectible>();
        private List<LevelSegmentData> segments;

        public void Initialize(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            gameDataManager = GameContext.Instance.Get<GameDataManager>();
            gameplayTimeManager = GameContext.Instance.Get<GameplayTimeManager>();

            // Ensure FurnitureConfig property initializes placement model config
            var _ = FurnitureConfig;

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

            for (int i = 0; i < activeTreats.Count; i++)
            {
                if (activeTreats[i] != null)
                {
                    activeTreats[i].EventCollected -= OnCollectibleCollected;
                    activeTreats[i].EventRecycleRequested -= OnCollectibleRecycle;
                }
            }
            activeTreats.Clear();
            lastFurnitureWorldY = ConstGameplay.Obstacle.Furniture.UNINITIALIZED_LAST_FURNITURE_WORLD_Y;
            lastOpenLaneIndex = ConstGameplay.LevelGenerator.INITIAL_LANE_INDEX;
            GameContext.Instance.Remove(this);
        }

        public void ResetLevel()
        {
            while (activeSegmentQueue.Count > 0)
            {
                visualizer?.RecycleOldestSegment();
                activeSegmentQueue.Dequeue();
            }

            activeFurnitureObstacles.Clear();

            var poolManager = GameContext.Instance?.Get<ObjectPoolManager>();
            for (int i = 0; i < activeTreats.Count; i++)
            {
                if (activeTreats[i] != null)
                {
                    activeTreats[i].EventCollected -= OnCollectibleCollected;
                    activeTreats[i].EventRecycleRequested -= OnCollectibleRecycle;
                    poolManager?.Recycle(activeTreats[i].gameObject);
                }
            }
            activeTreats.Clear();

            lastFurnitureWorldY = ConstGameplay.Obstacle.Furniture.UNINITIALIZED_LAST_FURNITURE_WORLD_Y;
            lastOpenLaneIndex = ConstGameplay.LevelGenerator.INITIAL_LANE_INDEX;

            if (config != null)
            {
                nextTriggerPosition = SegmentHeight + config.SegmentRecycleTriggerOffset;
                nextYSpawnPosition = SegmentHeight * config.MaxSegmentAmount;

                for (int i = 0; i < config.MaxSegmentAmount; i++)
                {
                    float spawnYPosition = i * SegmentHeight;
                    SpawnSegmentAt(spawnYPosition);
                }
            }
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

            int safeZone = FurnitureConfig != null ? FurnitureConfig.SafeZoneCells : ConstGameplay.Obstacle.SAFE_ZONE_CELLS;
            float cellHeight = FurnitureConfig != null ? FurnitureConfig.CellHeight : ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;

            if (worldY <= safeZone * cellHeight) return false;

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
                ConstGameplay.LevelGenerator.INITIAL_SEGMENT_ID,
                ConstGameplay.LevelGenerator.DEFAULT_INITIAL_SEGMENT_PREFAB,
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

            GIFurnitureObstacle[] prefabFurniture = segmentInstance.GetComponentsInChildren<GIFurnitureObstacle>(true);
            foreach (var furniture in prefabFurniture)
            {
                if (furniture != null)
                {
                    furniture.UpdateWorldPositionAndLane();
                    RegisterFurnitureObstacle(furniture);
                }
            }

            ProcedurallyGenerateFurniture(yPosition, segmentInstance, giSegment);
            ProcedurallyGenerateCollectible(yPosition, segmentInstance, giSegment);

            activeSegmentQueue.Enqueue(instance);
            return instance;
        }

        private void ProcedurallyGenerateCollectible(float yPosition, GameObject segmentInstance, GISegment giSegment)
        {
            if (segmentInstance == null || visualizer == null || CollectibleConfig == null) return;

            var collectibles = CollectiblePlacementHelper.GenerateSegmentCollectibles(
                yPosition,
                SegmentHeight,
                LaneXPositions.Length,
                IsCellBlockedByFurniture,
                (rowY) => rowY > (CollectibleConfig.SafeZoneCells * 3.0f),
                CollectibleConfig
            );

            string prefabName = CollectibleConfig.PrefabName;
            int pointValue = CollectibleConfig.TreatPointValue;

            foreach (var collectibleData in collectibles)
            {
                GICollectible giCollectible = visualizer.SpawnCollectible(collectibleData, yPosition, segmentInstance, giSegment, prefabName, pointValue);
                if (giCollectible != null)
                {
                    giCollectible.EventCollected += OnCollectibleCollected;
                    giCollectible.EventRecycleRequested += OnCollectibleRecycle;
                    activeTreats.Add(giCollectible);
                }
            }
        }

        private void OnCollectibleCollected(GICollectible giCollectible)
        {
            if (giCollectible != null)
            {
                var collectibleManager = GameContext.Instance?.Get<CollectibleManager>();
                collectibleManager?.AddValue(giCollectible.PointValue);
            }
        }

        private void OnCollectibleRecycle(GICollectible giCollectible)
        {
            if (giCollectible != null)
            {
                giCollectible.EventCollected -= OnCollectibleCollected;
                giCollectible.EventRecycleRequested -= OnCollectibleRecycle;
                activeTreats.Remove(giCollectible);

                var poolManager = GameContext.Instance?.Get<ObjectPoolManager>();
                poolManager?.Recycle(giCollectible.gameObject);
            }
        }

        private void ProcedurallyGenerateFurniture(float yPosition, GameObject segmentInstance, GISegment giSegment)
        {
            if (segmentInstance == null) return;

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
        }
    }
}
