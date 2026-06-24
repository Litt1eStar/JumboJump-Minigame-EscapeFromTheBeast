using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJump.EFTB.GameData.LevelSegment
{
    public class LevelGeneratorConfigSO : ScriptableObject
    {
        [Header("Templates")]
        public List<LevelSegmentSO> segmentTemplates;

        [Header("Lane Setting")]
        public float[] laneXPositions = new float[5] { -2f, -1f, 0f, 1f, 2f };
        public int maxSegmentAmount = 3;

        [Header("Progression Setting")] //Adjust and implement it later
        public float mediumDifficultyDistance = 500f;
        public float hardDifficultyDistance = 1500f;
    }
}
