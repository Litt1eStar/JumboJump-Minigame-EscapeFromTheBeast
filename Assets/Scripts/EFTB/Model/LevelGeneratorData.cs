using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.Model
{
    public class LevelGeneratorData
    {
        public class LevelGeneratorConfigData
        {
            public List<LevelSegmentData> SegmentTemplate { get; private set; }
            public float LANE_X_POSITIONS { get; private set; }
            public int MAX_SEGMENT_AMOUNT { get; private set; }
            public float SEGMENT_HEIGHT { get; private set; }
            public float SEGMENT_RECYCLE_TRIGGER_OFFSET { get; private set; }
            public float MediumDifficultyDistance { get; private set; }
            public float HardDifficultyDistance { get; private set; }

            public LevelGeneratorConfigData(List<LevelSegmentData> segmentTemplate,
                                            float laneXPositions,
                                            int maxSegmentAmount,
                                            float segmentHeight,
                                            float segmentRecycleTriggerOffset,
                                            float mediumDifficultyDistance,
                                            float hardDifficultyDistance)
            {
                SegmentTemplate = segmentTemplate;
                LANE_X_POSITIONS = laneXPositions;
                MAX_SEGMENT_AMOUNT = maxSegmentAmount;
                SEGMENT_HEIGHT = segmentHeight;
                SEGMENT_RECYCLE_TRIGGER_OFFSET = segmentRecycleTriggerOffset;
                MediumDifficultyDistance = mediumDifficultyDistance;
                HardDifficultyDistance = hardDifficultyDistance;
            }
        }

        public class LevelSegmentData
        {
            public int Id { get; private set; }
            public string SegmentPrefabName { get; private set; }
            public float SEGMENT_HEIGHT { get; private set; }
            public SegmentDifficultyEnum Difficulty { get; private set; }
            public List<LaneObjectData> PrePlacedObject { get; private set; }
            public List<LaneEventData> LaneEventData { get; private set; }

            public LevelSegmentData(int id,
                                    string segmentPrefabName,
                                    float segmentHeight,
                                    SegmentDifficultyEnum difficulty,
                                    List<LaneObjectData> prePlacedObject,
                                    List<LaneEventData> laneEventData)
            {
                Id = id;
                SegmentPrefabName = segmentPrefabName;
                SEGMENT_HEIGHT = segmentHeight;
                Difficulty = difficulty;
                PrePlacedObject = prePlacedObject ?? new List<LaneObjectData>();
                LaneEventData = laneEventData ?? new List<LaneEventData>();
            }
        }

        public class LaneObjectData
        {
            public int LaneIndex { get; private set; }
            public float YOffset { get; private set; }
            public string PrefabName { get; private set; }

            public LaneObjectData(int laneIndex, float yOffset, string prefabName)
            {
                LaneIndex = laneIndex;
                YOffset = yOffset;
                PrefabName = prefabName;
            }
        }

        public class LaneEventData
        {
            public int TargetLaneIndex { get; private set; }
            public float TriggerYOffset { get; private set; }
            public float Speed { get; private set; }
            public string PrefabName { get; private set; }

            public LaneEventData(int targetLaneIndex, float triggerYOffset, float speed, string prefabName)
            {
                TargetLaneIndex = targetLaneIndex;
                TriggerYOffset = triggerYOffset;
                Speed = speed;
                PrefabName = prefabName;
            }
        }
    }
}
