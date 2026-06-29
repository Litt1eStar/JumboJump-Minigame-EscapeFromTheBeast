using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Model
{
    public class LevelGeneratorData
    {
        [Serializable]
        public class LevelGeneratorConfigData
        {
            [SerializeField] private List<LevelSegmentData> segmentTemplate;
            public List<LevelSegmentData> SegmentTemplate { get => segmentTemplate; private set => segmentTemplate = value; }

            [SerializeField] private float laneXPositions;
            public float LaneXPositions { get => laneXPositions; private set => laneXPositions = value; }

            [SerializeField] private int maxSegmentAmount;
            public int MaxSegmentAmount { get => maxSegmentAmount; private set => maxSegmentAmount = value; }

            [SerializeField] private float segmentHeight;
            public float SegmentHeight { get => segmentHeight; private set => segmentHeight = value; }

            [SerializeField] private float segmentRecycleTriggerOffset;
            public float SegmentRecycleTriggerOffset { get => segmentRecycleTriggerOffset; private set => segmentRecycleTriggerOffset = value; }

            [SerializeField] private float mediumDifficultyDistance;
            public float MediumDifficultyDistance { get => mediumDifficultyDistance; private set => mediumDifficultyDistance = value; }

            [SerializeField] private float hardDifficultyDistance;
            public float HardDifficultyDistance { get => hardDifficultyDistance; private set => hardDifficultyDistance = value; }
        }

        [Serializable]
        public class LevelSegmentData
        {
            [SerializeField] private int id;
            public int Id { get => id; private set => id = value; }

            [SerializeField] private string segmentPrefabName;
            public string SegmentPrefabName { get => segmentPrefabName; private set => segmentPrefabName = value; }

            [SerializeField] private float segmentHeight;
            public float SegmentHeight { get => segmentHeight; private set => segmentHeight = value; }

            [SerializeField] private SegmentDifficultyEnum difficulty;
            public SegmentDifficultyEnum Difficulty { get => difficulty; private set => difficulty = value; }

            [SerializeField] private List<LaneObjectData> prePlacedObject;
            public List<LaneObjectData> PrePlacedObject { get => prePlacedObject; private set => prePlacedObject = value; }

            [SerializeField] private List<LaneEventData> laneEventData;
            public List<LaneEventData> LaneEventData { get => laneEventData; private set => laneEventData = value; }

            public LevelSegmentData()
            {
                prePlacedObject = new List<LaneObjectData>();
                laneEventData = new List<LaneEventData>();
            }

            public LevelSegmentData(int id, string segmentPrefabName, float segmentHeight, SegmentDifficultyEnum difficulty) : this()
            {
                this.id = id;
                this.segmentPrefabName = segmentPrefabName;
                this.segmentHeight = segmentHeight;
                this.difficulty = difficulty;
            }
        }

        [Serializable]
        public class LaneObjectData
        {
            [SerializeField] private int laneIndex;
            public int LaneIndex { get => laneIndex; private set => laneIndex = value; }

            [SerializeField] private float yOffset;
            public float YOffset { get => yOffset; private set => yOffset = value; }

            [SerializeField] private string prefabName;
            public string PrefabName { get => prefabName; private set => prefabName = value; }

            public LaneObjectData() {}

            public LaneObjectData(int laneIndex, float yOffset, string prefabName)
            {
                this.laneIndex = laneIndex;
                this.yOffset = yOffset;
                this.prefabName = prefabName;
            }
        }

        [Serializable]
        public class LaneEventData
        {
            [SerializeField] private int targetLaneIndex;
            public int TargetLaneIndex { get => targetLaneIndex; private set => targetLaneIndex = value; }

            [SerializeField] private float triggerYOffset;
            public float TriggerYOffset { get => triggerYOffset; private set => triggerYOffset = value; }

            [SerializeField] private float speed;
            public float Speed { get => speed; private set => speed = value; }

            [SerializeField] private string prefabName;
            public string PrefabName { get => prefabName; private set => prefabName = value; }

            public LaneEventData() {}

            public LaneEventData(int targetLaneIndex, float triggerYOffset, float speed, string prefabName)
            {
                this.targetLaneIndex = targetLaneIndex;
                this.triggerYOffset = triggerYOffset;
                this.speed = speed;
                this.prefabName = prefabName;
            }
        }
    }
}
