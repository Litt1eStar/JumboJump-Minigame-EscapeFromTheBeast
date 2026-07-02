namespace JumboJumps.EFTB.Constant.Gameplay
{
    public static class ConstGameplay
    {
        public const float LimitPlayTime = 500f;
        public class LevelGenerator
        {
            public const int MaxSegmentAmount = 5;
            public const float SegmentHeight = 20f;
            public const float SegmentRecycleTriggerOffset = 5f;
            public const float MediumDifficultyDistance = 200f; 
            public const float HardDifficultyDistance = 1000f; 
            public const float MediumDifficultyTimePercentage = 0.5f;
            public const float HardDifficultyTimePercentage = 0.7f;
        }

        public class Cat
        {
            public const float SleepyCatLeftLaneSpawnPosition = -4.3f;
            public const float SleepyCatRightLaneSpawnPosition = 3.3f;
        }
    }
}
