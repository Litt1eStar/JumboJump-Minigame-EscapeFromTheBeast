using JumboJump.EFTB.GameData.LevelSegment;
using UnityEngine;

namespace JumboJump.EFTB.GI
{
    public class GILevelGenerator : MonoBehaviour
    {
        public LevelGeneratorConfigSO configSo;

        [Range(0f, 1000f)]
        public float yTestPosition = 0f;
    }
}
