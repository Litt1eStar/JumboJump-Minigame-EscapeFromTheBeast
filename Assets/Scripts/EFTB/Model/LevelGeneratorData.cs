using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.Model
{
    public class LevelGeneratorData
    {
        [Serializable]
        public class LevelGeneratorConfigData
        {
            List<LevelSegmentData> segmentTemplate;
            public float laneXPositions;
            public int maxSegmentAmount;
            public float segmentHeight;
            public float segmentRecycleTriggerOffset;
            public float mediumDifficultyDistance;
            public float hardDifficultyDistance;
        }

        [Serializable]
        public class LevelSegmentData
        {
            public int id;
            public string segmentPrefabName;
            public float segmentHeight;
            public string difficulty;
            public List<LaneObjectData> prePlacedObjectDatas;
            public List<LaneEventData> laneEventDatas;
        }

        [Serializable]
        public class LaneObjectData
        {
            public int laneIndex;
            public float yOffset;
            public string prefabName;
        }

        [Serializable]
        public class LaneEventData
        {
            public int targetLaneIndex;
            public float triggerYOffset;
            public float speed;
            public string prefabName;
        }
    }
}
