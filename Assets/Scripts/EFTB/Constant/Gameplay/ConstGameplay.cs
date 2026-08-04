using System.Numerics;
using UnityEngine;

namespace JumboJumps.EFTB.Constant.Gameplay
{
    public static class ConstGameplay
    {
        public const float LIMIT_PLAY_TIME = 60f;

        public class LevelGenerator
        {
            public const int MAX_SEGMENT_AMOUNT = 5;
            public const float SEGMENT_HEIGHT = 21f;
            public const float SEGMENT_RECYCLE_TRIGGER_OFFSET = 8f;
            public const float MEDIUM_DIFFICULTY_TIME_PERCENTAGE = 0.5f;
            public const float HARD_DIFFICULTY_TIME_PERCENTAGE = 0.7f;
            public const int INITIAL_LANE_INDEX = 1;
            public const string DEFAULT_INITIAL_SEGMENT_PREFAB = "Prefab_Segment_Initial";
            public const int INITIAL_SEGMENT_ID = 0;
            public static readonly float[] LANE_X_POSITIONS = new float[] { -2.0f, 0.0f, 2.0f };
            public const float LANE_SIZE = 2.0f;
        }

        public class Player
        {
            public const float STEP_DISTANCE_Y = 3.0f;
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
                public const float SLIDE_DIRECTION_LEFT_MULTIPLIER = -1f;
                public const float SLIDE_DIRECTION_RIGHT_MULTIPLIER = 1f;
                public const float TRANSITION_PROGRESS_COMPLETE = 1f;
                public const float CAT_LEFT_HAND_Y_ROTATION = 0f;
                public const float CAT_RIGHT_HAND_Y_ROTATION = 180f;
                public const float CAT_APPEAR_SNEAK_IN_DURATION_PERCENTAGE = 0.2f;
                public const float CAT_APPEAR_STAY_DURATION_PERCENTAGE = 0.8f;
                public const float IDLE_LIMIT = 4.5f;
                public const float POUNCE_WARNING_DURATION = 1.5f;
                public const float POUNCE_FLASH_INTERVAL = 0.1f;
                public static readonly Color POUNCE_FLASH_COLOR = Color.red;
            }
        }

        public class Obstacle
        {
            public class Furniture
            {
                public const float CELL_HEIGHT = 3.0f;
                public const float BASE_FURNITURE_ROW_RATIO = 0.20f;
                public const float DENSITY_STEP_RATIO = 0.05f;
                public const int DENSITY_STEP_CELLS = 30;
                public const float MAX_FURNITURE_ROW_RATIO = 0.60f;
                public const int SINGLE_BLOCK_MAX_CELLS = 120;
                public const int MAX_BLOCKS_PER_ROW = 2;
                public const int MIN_ROW_SPACING_CELLS = 1;
                public const string DEFAULT_FURNITURE_PREFAB = "Prefab_Obstacle_Chair";
                public static readonly string[] FURNITURE_PREFAB_NAMES = new string[]
                {
                    DEFAULT_FURNITURE_PREFAB,
                    "Prefab_Obstacle_Box",
                    "Prefab_Obstacle_Plant"
                };
            }
        }
    }
}
