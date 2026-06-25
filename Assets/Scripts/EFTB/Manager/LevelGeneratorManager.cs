using JumboJumps.EFTB.GameData.LevelSegment;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer.LevelGenerator;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class LevelGeneratorManager
    {
        private LevelGeneratorConfigSO configSo;
        private LevelGeneratorVisualizer visualizer;
        private Transform playerTransform;

        private float nextTriggerPosition;
        private float nextYSpawnPosition;
        private Queue<GameObject> segmentQueue = new();

        private float segmentSize = 20f;
        public void Initialize(LevelGeneratorConfigSO configSo, Transform playerTransform)
        {
            this.configSo = configSo;
            this.playerTransform = playerTransform;

            visualizer = new LevelGeneratorVisualizer();
            visualizer.Initialize();

            segmentSize = configSo.segmentHeight;

            nextTriggerPosition = segmentSize + configSo.segmentRecycleTriggerOffset;
            nextYSpawnPosition = segmentSize * configSo.maxSegmentAmount;

            for (int i = 0; i < configSo.maxSegmentAmount; i++)
            {
                float spawnYPosition = i * segmentSize;
                GameObject segment = SpawnSegmentAt(spawnYPosition);
                segmentQueue.Enqueue(segment);
                DebugLogHelper.Log(segment.name);
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

            nextTriggerPosition += segmentSize;
            nextYSpawnPosition += segmentSize;
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
            if (configSo.segmentTemplates == null || configSo.segmentTemplates.Count == 0)
            {
                return null;
            }

            float playerY = playerTransform != null ? playerTransform.position.y : 0f;
            SegmentDifficultyEnum currentDifficulty = GetCurrentDifficulty(playerY);

            List<LevelSegmentSO> matchedTemplates = configSo.segmentTemplates.FindAll(t => t.difficulty == currentDifficulty);
            LevelSegmentSO selectedTemplate = null;

            if (matchedTemplates.Count > 0)
            {
                int randomIndex = Random.Range(0, matchedTemplates.Count);
                selectedTemplate = matchedTemplates[randomIndex];
            }
            else
            {
                int randomIndex = Random.Range(0, configSo.segmentTemplates.Count);
                selectedTemplate = configSo.segmentTemplates[randomIndex];
            }

            GameObject segmentInstance = visualizer.SpawnSegment(selectedTemplate, yPosition);
            return segmentInstance;
        }
    }
}
