namespace JumboJumps.EFTB.Constant.Gameplay
{
    public static class ConstGameplay
    {
        public const float LIMIT_PLAY_TIME = 60f;

        public class LevelGenerator
        {
            public const int MAX_SEGMENT_AMOUNT = 5;
            public const float SEGMENT_HEIGHT = 20f;
            public const float SEGMENT_RECYCLE_TRIGGER_OFFSET = 5f;
            public const float MEDIUM_DIFFICULTY_TIME_PERCENTAGE = 0.5f;
            public const float HARD_DIFFICULTY_TIME_PERCENTAGE = 0.7f;
            public const int INITIAL_LANE_INDEX = 0;
            public const float SEGMENT_Y_POSITION_TOLERANCE = 0.05f;
            public const string DEFAULT_INITIAL_SEGMENT_PREFAB = "Prefab_Segment_Initial";
            public const int INITIAL_SEGMENT_ID = 0;
            public static readonly float[] LANE_X_POSITIONS = new float[] { -1.0f, 1.0f };
            public const float LANE_SIZE = 2.0f;
        }

        public class Player
        {
            public const float STEP_DISTANCE_Y = 2.0f;
            public const float STEP_DURATION = 0.12f;
        }

        public class Score
        {
            public const int DISTANCE_POINT_PER_CELL = 10;
            public const int TREAT_POINT_VALUE = 100;
        }

        public class Cat
        {
            public const float CAT_LEFT_LANE_SPAWN_POSITION = -4.3f;
            public const float CAT_RIGHT_LANE_SPAWN_POSITION = 3.3f;
            public const float CAT_SPAWN_THRESHOLD = 2.5f;

            public class AggressiveCat
            {
                public const string PREFAB_NAME = "Prefab_Event_AggressiveCat";
                public const float INITIAL_MIN_SPAWN_TIME = 7f;
                public const float INITIAL_MAX_SPAWN_TIME = 15f;
                public const float NORMAL_MIN_SPAWN_TIME = 5f;
                public const float NORMAL_MAX_SPAWN_TIME = 10f;
                public const float HARD_MIN_SPAWN_TIME = 2f;
                public const float HARD_MAX_SPAWN_TIME = 5f;
                public const float CAT_VERTICAL_SPAWN_OFFSET = 15f;
                public const float SLIDE_DIRECTION_LEFT_MULTIPLIER = -1f;
                public const float SLIDE_DIRECTION_RIGHT_MULTIPLIER = 1f;
                public const float TRANSITION_PROGRESS_COMPLETE = 1f;
                public const float CAT_LEFT_HAND_Y_ROTATION = 0f;
                public const float CAT_RIGHT_HAND_Y_ROTATION = 180f;
                public const float CAT_APPEAR_SNEAK_IN_DURATION_PERCENTAGE = 0.2f;
                public const float CAT_APPEAR_STAY_DURATION_PERCENTAGE = 0.8f;
                public const float EVENT_WARNING_DURATION = 1.0f;
                public const float DIRECTION_WARNING_DURATION = 1.5f;
                public const float FALLBACK_SPAWN_CHECK_INTERVAL = 0.5f;
                public const float NEXT_SPAWN_TIMER = 7f;
            }
        }
    }
}
