using System.Numerics;

namespace JumboJumps.EFTB.Constant.Gameplay
{
    public static class ConstGameplay
    {
        public const float LIMIT_PLAY_TIME = 500f;
        public class LevelGenerator
        {
            public const int MAX_SEGMENT_AMOUNT = 5;
            public const float SEGMENT_HEIGHT = 20f;
            public const float SEGMENT_RECYCLE_TRIGGER_OFFSET = 5f;
            public const float MEDIUM_DIFFICULTY_TIME_PERCENTAGE = 0.5f;
            public const float HARD_DIFFICULTY_TIME_PERCENTAGE = 0.7f;
            public const int INITIAL_LANE_INDEX = 0;
            public static readonly float[] LANE_X_POSITIONS = new float[] { -0.8f, 0.8f };
            public const float LANE_SIZE = 2.0f;
        }

        public class Cat
        {
            public const float CAT_LEFT_LANE_SPAWN_POSITION = -4.3f;
            public const float CAT_RIGHT_LANE_SPAWN_POSITION = 3.3f;
            public const float CAT_SPAWN_THRESHOLD = 2.5f;

            public class AggressiveCat
            {
                public const string PREFAB_NAME = "Prefab_Event_AggressiveCat";
                public const float INITIAL_MIN_SPAWN_TIME = 5f;
                public const float INITIAL_MAX_SPAWN_TIME = 10f;
                public const float CAT_VERTICAL_SPAWN_OFFSET = 15f;
                public const float SLIDE_DIRECTION_LEFT_MULTIPLIER = -1f;
                public const float SLIDE_DIRECTION_RIGHT_MULTIPLIER = 1f;
                public const float TRANSITION_PROGRESS_COMPLETE = 1f;
                public const float CatLeftHandYRotation = 0f;
                public const float CatRightHandYRotation = 180f;
                public const float CatAppearSneakInDurationPercentage = 0.2f;
                public const float CatAppearStayDurationPercentage = 0.8f;
                public const float EventWarningDuration = 1.0f;
                public const float DirectionWarningDuration = 1.5f;
                public const float FallbackSpawnCheckInterval = 0.5f;
                public const float NEXT_SPAWN_TIMER = 0.5f;
            }
        }
    }
}
