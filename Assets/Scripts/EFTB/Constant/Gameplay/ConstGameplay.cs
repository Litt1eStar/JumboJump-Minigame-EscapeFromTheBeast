using System.Numerics;

namespace JumboJumps.EFTB.Constant.Gameplay
{
    public static class ConstGameplay
    {
        public const float Limit_Play_Time = 60f;
        public class LevelGenerator
        {
            public const int Max_Segment_Amount = 5;
            public const float Segment_Height = 20f;
            public const float Segment_Recycle_Trigger_Offset = 5f;
            public const float Medium_Difficulty_Time_Percentage = 0.5f;
            public const float Hard_Difficulty_Time_Percentage = 0.7f;
            public const int Initial_Lane_Index = 0;
            public const float Segment_Y_Position_Tolerance = 0.05f;
            public const string Default_Initial_Segment_Prefab = "Prefab_Segment_Initial";
            public const int Initial_Segment_Id = 0;
            public static readonly float[] Lane_X_Positions = new float[] { -1.0f, 1.0f };
        }

        public class Player
        {
            public const float Step_Distance_Y = 2.0f;
            public const float Step_Duration = 0.12f;
        }

        public class Cat
        {
            public const float Cat_Left_Lane_Spawn_Position = -4.3f;
            public const float Cat_Right_Lane_Spawn_Position = 3.3f;
            public const float Cat_Spawn_Threshold = 2.5f;

            public class AggressiveCat
            {
                public const string Prefab_Name = "Prefab_Event_AggressiveCat";
                public const float Initial_Min_Spawn_Time = 7f;
                public const float Initial_Max_Spawn_Time = 15f;
                public const float Normal_Min_Spawn_Time = 5f;
                public const float Normal_Max_Spawn_Time = 10f;
                public const float Hard_Min_Spawn_Time = 2f;
                public const float Hard_Max_Spawn_Time = 5f;
                public const float Cat_Vertical_Spawn_Offset = 15f;
                public const float Slide_Direction_Left_Multiplier = -1f;
                public const float Slide_Direction_Right_Multiplier = 1f;
                public const float Transition_Progress_Complete = 1f;
                public const float Cat_Left_Hand_Y_Rotation = 0f;
                public const float Cat_Right_Hand_Y_Rotation = 180f;
                public const float Cat_Appear_Sneak_In_Duration_Percentage = 0.2f;
                public const float Cat_Appear_Stay_Duration_Percentage = 0.8f;
                public const float Event_Warning_Duration = 1.0f;
                public const float Direction_Warning_Duration = 1.5f;
                public const float Fallback_Spawn_Check_Interval = 0.5f;
            }
        }
    }
}
