using JumboJumps.EFTB.GameData.LevelSegment;
using JumboJumps.EFTB.GI;
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

            nextTriggerPosition = segmentSize;
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


        /// <summary>
        /// UpdateLogic : use yTestPosition only when player movement is not implemented for testing purpose
        /// Working on PlayerMovement in next PR
        /// </summary>
        public void UpdateLogic(float deltaTime)
        {
            if (playerTransform.position.y >= nextTriggerPosition)
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

        private GameObject SpawnSegmentAt(float yPosition)
        {
            int randomIndex = Random.Range(0, configSo.segmentTemplates.Count);
            
            LevelSegmentSO selectedTemplate = configSo.segmentTemplates[randomIndex];
            GameObject segmentInstance = visualizer.SpawnSegment(selectedTemplate.segmentPrefab, yPosition);

            return segmentInstance;
        }
    }
}
