using System.Numerics;
using UnityEngine;

namespace JumboJumps.EFTB.Constant.Gameplay
{
    public static class ConstGameplay
    {
        public const float Limit_Play_Time = 60f;

        public class LevelGenerator
        {
            public const int Max_Segment_Amount = 5;
            public const float Segment_Height = 21f;
            public const float Segment_Recycle_Trigger_Offset = 8f;
            public const float Medium_Difficulty_Time_Percentage = 0.5f;
            public const float Hard_Difficulty_Time_Percentage = 0.7f;
            public const int Initial_Lane_Index = 1;
            public const string Default_Initial_Segment_Prefab = "Prefab_Segment_Initial";
            public const int Initial_Segment_Id = 0;
            public static readonly float[] Lane_X_Positions = new float[] { -2.0f, 0.0f, 2.0f };
            public const float Lane_Size = 3.0f;
        }

        public class Player
        {
            public const float Step_Distance_Y = 3.0f;
            public const float Step_Duration = 0.12f;
        }

        public class Score
        {
            public const int Distance_Point_Per_Cell = 10;
            public const int Treat_Point_Value = 100;
        }

        public class Cat
        {
            public const float Cat_Left_Lane_Spawn_Position = -4.3f;
            public const float Cat_Right_Lane_Spawn_Position = 3.3f;
            public const float Cat_Spawn_Threshold = 2.5f;

            public class AggressiveCat
            {
                public const string Prefab_Name = "Prefab_Event_AggressiveCat";
                public const float Slide_Direction_Left_Multiplier = -1f;
                public const float Slide_Direction_Right_Multiplier = 1f;
                public const float Transition_Progress_Complete = 1f;
                public const float Cat_Left_Hand_Y_Rotation = 0f;
                public const float Cat_Right_Hand_Y_Rotation = 180f;
                public const float Cat_Appear_Sneak_In_Duration_Percentage = 0.2f;
                public const float Cat_Appear_Stay_Duration_Percentage = 0.8f;
                public const float Idle_Limit = 4.5f;
                public const float Pounce_Warning_Duration = 1.5f;
                public const float Pounce_Flash_Interval = 0.1f;
                public static readonly Color Pounce_Flash_Color = Color.red;
            }
        }

        public class Obstacle
        {
            public const int Safe_Zone_Cells = 5;

            public class Furniture
            {
                public const float Cell_Height = 3.0f;
                public const float Base_Furniture_Row_Ratio = 0.20f;
                public const float Density_Step_Ratio = 0.05f;
                public const int Density_Step_Cells = 30;
                public const float Max_Furniture_Row_Ratio = 0.60f;
                public const int Single_Block_Max_Cells = 120;
                public const int Max_Blocks_Per_Row = 2;
                public const int Min_Row_Spacing_Cells = 1;
                public const string Default_Furniture_Prefab = "Prefab_Obstacle_Chair";
                public static readonly string[] Furniture_Prefab_Names = new string[]
                {
                    Default_Furniture_Prefab,
                    "Prefab_Obstacle_Box",
                    "Prefab_Obstacle_Plant"
                };
            }

            public class Hazard
            {
                public const string Prefab_Name = "Prefab_Hazard_Variant01";
                public const float Telegraph_Duration = 0.3f;
                public const float Telegraph_Move_In_Duration_Percentage = 0.25f;
                public const float Telegraph_Stay_Duration_Percentage = 0.75f;
                public const float Base_Interval_Low = 3.0f;        // Base spawn interval low (s) 1.5
                public const float Base_Interval_High = 6.0f;       // Base spawn interval high (s) 2.4
                public const float Step_Interval_Reduction = 0.15f; // Spawn interval reduction per step (s)
                public const int Step_Interval_Cells = 30;          // Step height in cells
                public const float Min_Spawn_Interval = 0.5f;       // Minimum spawn interval limit (s)

                public const float Floor_Speed_Duration_Low = 1.0f; // Hazard speed floor range low (0.5s / lane)
                public const float Floor_Speed_Duration_High = 2.0f;// Hazard speed floor range high (0.9s / lane)
                public const float Spawn_Offscreen_X_Offset = 7.5f;   // Offscreen initial spawn X coordinate offset (units)
                public const float Despawn_Offscreen_X_Offset = 7.5f; // Offscreen despawn X coordinate offset (units)
                public const float Telegraph_Offscreen_X_Offset = 2.3f;
                public const int Safe_Zone_Cells = 10;

                public const float Hazard_Prespawn_Offset = 70f; // Offset from the player to pre-spawn hazards (in cells)
            }
        }
    }
}
