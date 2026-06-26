using System.Collections.Generic;
using static JumboJumps.EFTB.Model.LevelGeneratorData;

namespace JumboJumps.EFTB.GameData.LevelSegment
{
    public class LevelGeneratorConfig
    {
        public List<LevelSegmentData> segmentTemplates;

        public float[] laneXPositions = new float[5] { -2f, -1f, 0f, 1f, 2f };
        public int maxSegmentAmount = 3;
        public float segmentHeight = 20f;
        public float segmentRecycleTriggerOffset = 5f;

        public float mediumDifficultyDistance = 500f;
        public float hardDifficultyDistance = 1500f;

        public LevelGeneratorConfig(List<LevelSegmentData> segmentTemplates,
                                      float[] laneXPositions,
                                      int maxSegmentAmount,
                                      float segmentHeight,
                                      float segmentRecycleTriggerOffset,
                                      float mediumDifficultyDistance,
                                      float hardDifficultyDistance)
        {
            this.segmentTemplates = segmentTemplates;
            this.laneXPositions = laneXPositions;
            this.maxSegmentAmount = maxSegmentAmount;
            this.segmentHeight = segmentHeight;
            this.segmentRecycleTriggerOffset = segmentRecycleTriggerOffset;
            this.mediumDifficultyDistance = mediumDifficultyDistance;
            this.hardDifficultyDistance = hardDifficultyDistance;
        }
    }
}