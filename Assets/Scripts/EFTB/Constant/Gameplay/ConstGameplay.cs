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
            public const float SEGMENT_HEIGHT = 18f;
            public const float SEGMENT_RECYCLE_TRIGGER_OFFSET = 18f;
            public const float MEDIUM_DIFFICULTY_TIME_PERCENTAGE = 0.5f;
            public const float HARD_DIFFICULTY_TIME_PERCENTAGE = 0.7f;
            public const int INITIAL_LANE_INDEX = 1;
            public const string DEFAULT_INITIAL_SEGMENT_PREFAB = "Prefab_Segment_Initial";
            public const int INITIAL_SEGMENT_ID = 0;
            public static readonly float[] LANE_X_POSITIONS = new float[] { -2.3f, 0.0f, 2.3f };
            public const float LANE_SIZE = 3.0f;
        }

        public class Player
        {
            public const float STEP_DISTANCE_Y = 3.0f;
            public const float STEP_DURATION = 0.12f;
            public const float MIN_MOVE_ANIM_DURATION = 0.2f;
            public const string MOVING_ANIM_PARAM = "isMoving";
        }

        public class UI
        {
            public class MainMenu
            {
                public const float LOGO_IDLE_SCALE_SPEED = 3f;
                public const float LOGO_IDLE_SCALE_MIN = 0.95f;
                public const float LOGO_IDLE_SCALE_MAX = 1.05f;
                public const float FADE_DURATION = 0.3f;
                public const float READY_GO_HOLD_DURATION = 0.4f;
                public const float READY_GO_SCALE_START = 0.5f;
                public const float READY_GO_SCALE_TARGET = 1.1f;
                public const float READY_SWING_MAX_Z_ANGLE = 20f;
                public const float READY_SWING_SPEED = 15f;
                public const float GO_SCALE_OUT_TARGET = 2.2f;
            }
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
                public const float POUNCE_WARNING_SHAKE_AMOUNT = 0.08f;
                public const float POUNCE_WARNING_SHAKE_SPEED = 25f;
                public const float POUNCE_WARNING_MAX_Z_ROTATION = 12f;
                public static readonly Color POUNCE_FLASH_COLOR = Color.red;
                public static readonly float[] SPAWN_X_LEFT_POSITION = new float[] { -8.5f, -6.0f };
                public static readonly float[] SPAWN_X_RIGHT_POSITION = new float[] { 6.0f, 8.5f };
                public const float OFFSCREEN_X_LEFT_POSITION = -15.0f;
                public const float OFFSCREEN_X_RIGHT_POSITION = 15.0f;
                public const float INITIAL_Z_ROTATION = 15f;
                public const float FINAL_Z_ROTATION = -25f;
                public const float CAT_SMASH_MOVE_IN_PERCENTAGE = 0.4f;
                public const float CAT_SMASH_WAIT_PERCENTAGE = 0.7f;
            }
        }

        public class Obstacle
        {
            public const int SAFE_ZONE_CELLS = 5; // Number of cells at the start of the level where no obstacles are spawned (5)
            public const float DEFAULT_SPAWN_Y_OFFSET = 0.0f;

            public class Furniture
            {
                public const float CELL_HEIGHT = 3.0f;
                public const float BASE_FURNITURE_ROW_RATIO = 0.20f; // Base spawn ratio for furniture rows (20%)
                public const float DENSITY_STEP_RATIO = 0.05f; // Incremental spawn ratio increase per density step (5%)
                public const int DENSITY_STEP_CELLS = 30; // Number of cells after which the density step ratio is applied
                public const float MAX_FURNITURE_ROW_RATIO = 0.60f; // Maximum spawn ratio for furniture rows (60%)
                public const int SINGLE_BLOCK_MAX_CELLS = 120; // Maximum number of cells for a single furniture block (120)
                public const int MAX_BLOCKS_PER_ROW = 2; // Maximum number of furniture blocks allowed per row (2)
                public const int MIN_ROW_SPACING_CELLS = 1; // Minimum spacing between furniture rows in cells (1)
                public const string DEFAULT_FURNITURE_PREFAB = "Prefab_Obstacle_Variant001";
                public const float UNINITIALIZED_LAST_FURNITURE_WORLD_Y = -1f;
                public static readonly string[] FURNITURE_PREFAB_NAMES = new string[]
                {
                    DEFAULT_FURNITURE_PREFAB,
                    "Prefab_Obstacle_Variant001",
                    "Prefab_Obstacle_Variant002"
                };

            }

            public class Hazard
            {
                public const string PREFAB_NAME = "Prefab_Hazard_Variant01";
                public const float BASE_INTERVAL_LOW = 3.0f;        // Base spawn interval low (s) 1.5
                public const float BASE_INTERVAL_HIGH = 6.0f;       // Base spawn interval high (s) 2.4
                public const float STEP_INTERVAL_REDUCTION = 0.15f; // Spawn interval reduction per step (s)
                public const int STEP_INTERVAL_CELLS = 30;          // Step height in cells
                public const float MIN_SPAWN_INTERVAL = 0.5f;       // Minimum spawn interval limit (s)

                public const float FLOOR_SPEED_DURATION_LOW = 0.1f; // Hazard speed floor range low (0.5s / lane)
                public const float FLOOR_SPEED_DURATION_HIGH = 0.3f;// Hazard speed floor range high (0.9s / lane)
                public const float SPAWN_OFFSCREEN_X_OFFSET = 7.5f;   // Offscreen initial spawn X coordinate offset (units)
                public const float DESPAWN_OFFSCREEN_X_OFFSET = 7.5f; // Offscreen despawn X coordinate offset (units)
                public const int SAFE_ZONE_CELLS = 5;

                public const float HAZARD_PRESPAWN_OFFSET = 70f; // Offset from the player to pre-spawn hazards (in cells)
                public const float ROTATION_SPEED = 5.0f; // Hazard rotation speed scalar (default: 5.0)
                public const float ROTATION_SPEED_MULTIPLIER = 72.0f; // Degrees/sec per rotation speed unit
            }
        }
    }
}
