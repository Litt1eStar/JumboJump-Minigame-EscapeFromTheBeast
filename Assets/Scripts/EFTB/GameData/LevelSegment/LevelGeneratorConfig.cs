using System.Collections.Generic;
using static JumboJumps.EFTB.Model.LevelGeneratorData;

namespace JumboJumps.EFTB.GameData.LevelSegment
{
    public class LevelGeneratorConfig
    {
        public List<LevelSegmentData> segmentTemplates {  get; private set; }
        public float[] LaneXPosition { get; private set; } = new float[5] { -2f, -1f, 0f, 1f, 2f };
        public int MaxSegmentAmount { get; private set; } = 3;
        public float SegmentHeight { get; private set; } = 20f;
        public float SegmentRecycleTriggerOffset { get; private set; } = 5f;
        public float MediumDifficultyTimePercentage { get; private set; } = 500f;
        public float HardDifficultyTimePercentage { get; private set; } = 1500f;

        public LevelGeneratorConfig(List<LevelSegmentData> segmentTemplates,
                                    float[] laneXPositions,
                                    int maxSegmentAmount,
                                    float segmentHeight,
                                    float segmentRecycleTriggerOffset,
                                    float mediumDifficultyTimePercentage,
                                    float hardDifficultyTimePercentage)
        {
            this.segmentTemplates = segmentTemplates;
            LaneXPosition = laneXPositions;
            MaxSegmentAmount = maxSegmentAmount;
            SegmentHeight = segmentHeight;
            SegmentRecycleTriggerOffset = segmentRecycleTriggerOffset;
            MediumDifficultyTimePercentage = mediumDifficultyTimePercentage;
            HardDifficultyTimePercentage = hardDifficultyTimePercentage;
        }
    }
}