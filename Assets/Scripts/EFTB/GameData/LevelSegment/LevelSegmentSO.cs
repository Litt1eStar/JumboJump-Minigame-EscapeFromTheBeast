using UnityEngine;
using System.Collections.Generic;
using System;
using JumboJumps.EFTB.Model;

namespace JumboJumps.EFTB.GameData.LevelSegment
{
    [CreateAssetMenu(fileName = "LevelSegmentData", menuName = "EFTB/LevelSegmentData")]
    public class LevelSegmentSO : ScriptableObject
    {
        [Header("Segment Settings")]
        public GameObject segmentPrefab;
        public float segmentHeight = 20f;

        [Header("Object Spawn Layout")]
        public List<LaneObjectSpawnData> prePlacedObjectsData;
        public List<LaneEventSpawnData> spawnEventData;

        [Header("Difficulty Settings")]
        public SegmentDifficultyEnum difficulty = SegmentDifficultyEnum.Easy;
    }

    [Serializable]
    public struct LaneObjectSpawnData
    {
        /// <summary>
        /// laneIndex : index of lane (0 - 4)
        /// </summary>
        public int laneIndex;

        /// <summary>
        /// yOffset : Vertical offset from the bottom of segment
        /// </summary>
        public float yOffset;

        /// <summary>
        /// prefab : HidableObject or coin Prefab
        /// </summary>
        public GameObject prefab;
    }

    [Serializable]
    public struct LaneEventSpawnData
    {
        /// <summary>
        /// targetLaneIndex : Index of Lane that Event will moving through
        /// </summary>
        public int targetLaneIndex;

        /// <summary>
        /// triggerYOffset : Distance from start of segment to trigger the event
        /// </summary>
        public float triggerYOffset;

        public GameObject movingObstaclePrefab;
        public float speed;
    }
}
