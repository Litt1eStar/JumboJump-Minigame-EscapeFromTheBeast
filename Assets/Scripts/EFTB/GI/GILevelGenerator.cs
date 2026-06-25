using JumboJumps.EFTB.GameData.LevelSegment;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GILevelGenerator : MonoBehaviour
    {
        public LevelGeneratorConfigSO configSo;

        [Range(0f, 1000f)]
        public float yTestPosition = 0f;
    }
}
