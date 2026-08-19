using System.Collections.Generic;
using static JumboJumps.EFTB.Model.LevelGeneratorData;

namespace JumboJumps.EFTB.GameData.LevelSegment
{
    public class LevelGeneratorConfig
    {
        public List<LevelSegmentData> segmentTemplates { get; private set; }
        public float[] LaneXPosition { get; private set; } = new float[3] { -2.3f, 0f, 2.3f };
        public int MAX_SEGMENT_AMOUNT { get; private set; } = 3;
        public float SEGMENT_HEIGHT { get; private set; } = 18f;
        public float SEGMENT_RECYCLE_TRIGGER_OFFSET { get; private set; } = 5f;
        public float MEDIUM_DIFFICULTY_TIME_PERCENTAGE { get; private set; } = 500f;
        public float HARD_DIFFICULTY_TIME_PERCENTAGE { get; private set; } = 1500f;

        // Alias properties for backwards compatibility
        public int MaxSegmentAmount => MAX_SEGMENT_AMOUNT;
        public float SegmentHeight => SEGMENT_HEIGHT;
        public float SegmentRecycleTriggerOffset => SEGMENT_RECYCLE_TRIGGER_OFFSET;
        public float MediumDifficultyTimePercentage => MEDIUM_DIFFICULTY_TIME_PERCENTAGE;
        public float HardDifficultyTimePercentage => HARD_DIFFICULTY_TIME_PERCENTAGE;

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
            MAX_SEGMENT_AMOUNT = maxSegmentAmount;
            SEGMENT_HEIGHT = segmentHeight;
            SEGMENT_RECYCLE_TRIGGER_OFFSET = segmentRecycleTriggerOffset;
            MEDIUM_DIFFICULTY_TIME_PERCENTAGE = mediumDifficultyTimePercentage;
            HARD_DIFFICULTY_TIME_PERCENTAGE = hardDifficultyTimePercentage;
        }
    }
}