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

namespace JumboJumps.EFTB.Manager
{
    public class LevelGeneratorManager
    {
        private GameDataManager gameDataManager;
        private LevelGeneratorConfig config;
        private LevelGeneratorVisualizer visualizer;

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
        public float MediumDifficultyDistance { get; private set; }
        public float HardDifficultyDistance { get; private set; }

        private List<LevelSegmentData> segments;

        public void Initialize(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            gameDataManager = GameContext.Instance.Get<GameDataManager>();

            segments = gameDataManager.LevelSegmentData.Values.ToList();
            LaneXPositions = new float[5] { -2, -1, 0, 1, 2 };
            MaxSegmentAmount = ConstGameplay.LevelGenerator.MaxSegmentAmount;
            SegmentHeight = ConstGameplay.LevelGenerator.SegmentHeight;
            SegmentRecycleTriggerOffset = ConstGameplay.LevelGenerator.SegmentRecycleTriggerOffset;
            MediumDifficultyDistance = ConstGameplay.LevelGenerator.MediumDifficultyDistance;
            HardDifficultyDistance = ConstGameplay.LevelGenerator.HardDifficultyDistance;

            visualizer = new LevelGeneratorVisualizer(gameDataManager, LaneXPositions);
            visualizer.Initialize();

            config = new LevelGeneratorConfig(segments, LaneXPositions, MaxSegmentAmount, SegmentHeight, SegmentRecycleTriggerOffset, MediumDifficultyDistance, HardDifficultyDistance);

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

            EventSpawnerHandler(playerY);
        }

        private void EventSpawnerHandler(float playerY)
        {
            ActiveSegment currentSegment = activeSegmentQueue.Count > 0 ? activeSegmentQueue.Peek() : null;

            while (currentSegment.PendingEvents.Count > 0)
            {
                var pendingEvent = currentSegment.PendingEvents[0];
                float triggerY = currentSegment.SpawnY + pendingEvent.TriggerYOffset;

                if (playerY >= triggerY)
                {
                    visualizer.SpawnEventObstacle(pendingEvent, currentSegment.SpawnY, currentSegment.SegmentGo, currentSegment.GiSegment);
                    currentSegment.PendingEvents.RemoveAt(0);
                }
                else
                {
                    // The player hasn't reached the next event, so skip checking the remaining events for this segment
                    break;
                }

                ReEnqeueu(currentSegment);
            }
        }

        private void ReEnqeueu(ActiveSegment currentSegment)
        {
            activeSegmentQueue.Dequeue();
            activeSegmentQueue.Enqueue(currentSegment);
        }

        private void RecycleSegment()
        {
            visualizer.RecycleOldestSegment();
            activeSegmentQueue.Dequeue();

            SpawnSegmentAt(nextYSpawnPosition);

            nextTriggerPosition += SegmentHeight;
            nextYSpawnPosition += SegmentHeight;
        }

        private SegmentDifficultyEnum GetCurrentDifficulty(float playerY)
        {
            if (playerY < config.MediumDifficultyDistance)
            {
                return SegmentDifficultyEnum.Easy;
            }
            else if (playerY < config.HardDifficultyDistance)
            {
                return SegmentDifficultyEnum.Normal;
            }
            else
            {
                return SegmentDifficultyEnum.Hard;
            }
        }

        private ActiveSegment SpawnSegmentAt(float yPosition)
        {
            var gameDataManager = GameContext.Instance.Get<GameDataManager>();
            if (gameDataManager == null || gameDataManager.LevelSegmentData == null || gameDataManager.LevelSegmentData.Count == 0)
            {
                return null;
            }

            float playerY = playerTransform != null ? playerTransform.position.y : 0f;
            SegmentDifficultyEnum currentDifficulty = GetCurrentDifficulty(playerY);

            List<LevelSegmentData> allSegments = segments;
            List<LevelSegmentData> matchedTemplates = allSegments.FindAll(t => t.Difficulty == currentDifficulty);

            LevelSegmentData selectedTemplate = SelectTemplateFromMatchedTemplate(matchedTemplates, allSegments);
            GameObject segmentInstance = visualizer.SpawnSegment(selectedTemplate, yPosition);

            if (segmentInstance == null) return null;

            GISegment giSegment = segmentInstance.GetComponent<GISegment>();
            if (giSegment == null)
            {
                DebugLogHelper.LogError($"GISegment component not found on the spawned segment instance for template Id : {selectedTemplate.Id}");
                return null;
            }

            var instance = new ActiveSegment(selectedTemplate, yPosition, segmentInstance, giSegment);
            activeSegmentQueue.Enqueue(instance);
            return instance;
        }

        private LevelSegmentData SelectTemplateFromMatchedTemplate(List<LevelSegmentData> matchedTemplates, List<LevelSegmentData> allSegments)
        {
            LevelSegmentData selectedTemplate = null;

            if (matchedTemplates.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, matchedTemplates.Count);
                selectedTemplate = matchedTemplates[randomIndex];
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, allSegments.Count);
                selectedTemplate = allSegments[randomIndex];
            }

            return selectedTemplate;
        }
    }
}
