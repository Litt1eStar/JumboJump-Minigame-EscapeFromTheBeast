using System.Numerics;

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
            public const float MediumDifficultyTimePercentage = 0.5f;
            public const float HardDifficultyTimePercentage = 0.7f;
            public const int InitialLaneIndex = 0;
            public const float SegmentYPositionTolerance = 0.05f;
            public const string DefaultInitialSegmentPrefab = "Prefab_Segment_Initial";
            public const int InitialSegmentId = 0;
            public static readonly float[] LaneXPositions = new float[] { -1.0f, 1.0f };
        }

        public class Cat
        {
            public const float CatLeftLaneSpawnPosition = -4.3f;
            public const float CatRightLaneSpawnPosition = 3.3f;
            public const float CatSpawnThreshold = 2.5f;

            public class AggressiveCat
            {
                public const string PrefabName = "Prefab_Event_AggressiveCat";
                public const float InitialMinSpawnTime = 5f;
                public const float InitialMaxSpawnTime = 10f;
                public const float CatVerticalSpawnOffset = 15f;
                public const float SlideDirectionLeftMultiplier = -1f;
                public const float SlideDirectionRightMultiplier = 1f;
                public const float TransitionProgressComplete = 1f;
                public const float CatLeftHandYRotation = 0f;
                public const float CatRightHandYRotation = 180f;
                public const float CatAppearSneakInDurationPercentage = 0.2f;
                public const float CatAppearStayDurationPercentage = 0.8f;
                public const float EventWarningDuration = 1.0f;
                public const float DirectionWarningDuration = 1.5f;
                public const float FallbackSpawnCheckInterval = 0.5f;
            }
        }
    }
}
