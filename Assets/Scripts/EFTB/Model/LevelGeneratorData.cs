using System;
using System.Collections.Generic;

namespace JumboJump.Assets.Scripts.EFTB.Model
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
            public string segmentPrefabName;
            public string segmentHeight;
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
            public int laneIndex;
            public float yOffset;
            public float speed;
            public string prefabName;
        }
    }
}
