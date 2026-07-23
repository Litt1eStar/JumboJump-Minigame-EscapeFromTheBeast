using System.Numerics;
using UnityEngine;

namespace JumboJumps.EFTB.Constant.Gameplay
{
    public static class ConstGameplay
    {
        public const float LimitPlayTime = 60f;
        public class LevelGenerator
        {
            public const int MaxSegmentAmount = 5;
            public const float SegmentHeight = 21f;
            public const float SegmentRecycleTriggerOffset = 8f;
            public const float MediumDifficultyTimePercentage = 0.5f;
            public const float HardDifficultyTimePercentage = 0.7f;
            public const int InitialLaneIndex = 1;
            public const float SegmentYPositionTolerance = 0.05f;
            public const string DefaultInitialSegmentPrefab = "Prefab_Segment_Initial";
            public const int InitialSegmentId = 0;
            public static readonly float[] LaneXPositions = new float[] { -2.0f, 0.0f, 2.0f };
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
            public const float CatSpawnThreshold = 2.5f;

            public class AggressiveCat
            {
                public const string PrefabName = "Prefab_Event_AggressiveCat";
                public const float InitialMinSpawnTime = 7f;
                public const float InitialMaxSpawnTime = 15f;
                public const float NormalMinSpawnTime = 5f;
                public const float NormalMaxSpawnTime = 10f;
                public const float HardMinSpawnTime = 2f;
                public const float HardMaxSpawnTime = 5f;
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

                public const float INITIAL_MIN_SPAWN_TIME = 7f;
                public const float INITIAL_MAX_SPAWN_TIME = 15f;
                public const float CAT_VERTICAL_SPAWN_OFFSET = 5f;
                public const float NEXT_SPAWN_TIMER = 7f;
                public const string PREFAB_NAME = "Prefab_Event_AggressiveCat";
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
