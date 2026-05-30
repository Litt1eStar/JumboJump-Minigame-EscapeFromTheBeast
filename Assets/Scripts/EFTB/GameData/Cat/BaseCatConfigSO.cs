using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.UI;
using UnityEngine;

namespace JumboJumps.EFTB.GameData.Cat
{
    public abstract class BaseCatConfigSO : ScriptableObject
    {
        public abstract ICatStateController BuildStateController(GICat giCat, Transform transform, UICatStateLabel label);
    }
}
