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
        public float MediumDifficultyDistance { get; private set; } = 500f;
        public float HardDifficultyDistance { get; private set; } = 1500f;

        public LevelGeneratorConfig(List<LevelSegmentData> segmentTemplates,
                                    float[] laneXPositions,
                                    int maxSegmentAmount,
                                    float segmentHeight,
                                    float segmentRecycleTriggerOffset,
                                    float mediumDifficultyDistance,
                                    float hardDifficultyDistance)
        {
            this.segmentTemplates = segmentTemplates;
            this.LaneXPosition = laneXPositions;
            this.MaxSegmentAmount = maxSegmentAmount;
            this.SegmentHeight = segmentHeight;
            this.SegmentRecycleTriggerOffset = segmentRecycleTriggerOffset;
            this.MediumDifficultyDistance = mediumDifficultyDistance;
            this.HardDifficultyDistance = hardDifficultyDistance;
        }
    }
}