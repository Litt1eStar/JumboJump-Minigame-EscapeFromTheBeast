using JumboJumps.EFTB.GameData.LevelSegment;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.LevelGenerator;
using LevelSegmentData = JumboJumps.EFTB.Model.LevelGeneratorData.LevelSegmentData;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using JumboJumps.EFTB.Constant.Gameplay;

namespace JumboJumps.EFTB.Manager
{
    public class LevelGeneratorManager
    {
        private GameDataManager gameDataManager;
        private LevelGeneratorConfig configSo;
        private LevelGeneratorVisualizer visualizer;

        private Transform playerTransform;
        private float nextTriggerPosition;
        private float nextYSpawnPosition;
        private Queue<GameObject> segmentQueue = new();

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

            visualizer = new LevelGeneratorVisualizer(this, gameDataManager);
            visualizer.Initialize();

            segments = gameDataManager.LevelSegmentData.Values.ToList();
            LaneXPositions = new float[5] { -2, -1, 0, 1, 2 };
            MaxSegmentAmount = ConstGameplay.LevelGenerator.MaxSegmentAmount;
            SegmentHeight = ConstGameplay.LevelGenerator.SegmentHeight;
            SegmentRecycleTriggerOffset  =ConstGameplay.LevelGenerator.SegmentRecycleTriggerOffset;
            MediumDifficultyDistance = ConstGameplay.LevelGenerator.mediumDifficultyDistance;
            HardDifficultyDistance = ConstGameplay.LevelGenerator.hardDifficultyDistance;

            configSo = new LevelGeneratorConfig(segments, LaneXPositions, MaxSegmentAmount, SegmentHeight, SegmentRecycleTriggerOffset, MediumDifficultyDistance, HardDifficultyDistance);

            nextTriggerPosition = SegmentHeight + configSo.segmentRecycleTriggerOffset;
            nextYSpawnPosition = SegmentHeight * configSo.maxSegmentAmount;

            for (int i = 0; i < configSo.maxSegmentAmount; i++)
            {
                float spawnYPosition = i * SegmentHeight;
                GameObject segment = SpawnSegmentAt(spawnYPosition);
                segmentQueue.Enqueue(segment);
            }

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            visualizer?.Dispose();
            visualizer = null;

            segmentQueue.Clear();
            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if(playerTransform.position.y >= nextTriggerPosition)
            {
                RecycleSegment();
            }
        }

        private void RecycleSegment()
        {
            visualizer.RecycleOldestSegment();
            segmentQueue.Dequeue();

            GameObject newSegment = SpawnSegmentAt(nextYSpawnPosition);
            segmentQueue.Enqueue(newSegment);

            nextTriggerPosition += SegmentHeight;
            nextYSpawnPosition += SegmentHeight;
        }

        private SegmentDifficultyEnum GetCurrentDifficulty(float playerY)
        {
            if (playerY < configSo.mediumDifficultyDistance)
            {
                return SegmentDifficultyEnum.Easy;
            }
            else if (playerY < configSo.hardDifficultyDistance)
            {
                return SegmentDifficultyEnum.Normal;
            }
            else
            {
                return SegmentDifficultyEnum.Hard;
            }
        }

        private GameObject SpawnSegmentAt(float yPosition)
        {
            var gameDataManager = GameContext.Instance.Get<GameDataManager>();
            if (gameDataManager == null || gameDataManager.LevelSegmentData == null || gameDataManager.LevelSegmentData.Count == 0)
            {
                return null;
            }

            float playerY = playerTransform != null ? playerTransform.position.y : 0f;
            SegmentDifficultyEnum currentDifficulty = GetCurrentDifficulty(playerY);

            List<LevelSegmentData> allSegments = segments;
            List<LevelSegmentData> matchedTemplates = allSegments.FindAll(t => t.difficulty != null && t.difficulty.Equals(currentDifficulty.ToString(), StringComparison.OrdinalIgnoreCase));
            
            LevelSegmentData selectedTemplate = SelectTemplateFromMatchedTemplate(matchedTemplates, allSegments);
            GameObject segmentInstance = visualizer.SpawnSegment(selectedTemplate, yPosition);
           
            return segmentInstance;
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
