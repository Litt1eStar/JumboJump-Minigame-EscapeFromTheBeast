using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.UI;
using UnityEngine;


public abstract class BaseCatConfigSO : ScriptableObject
{
    public abstract ICatStateController BuildStateController(GICat giCat, Transform transform, UICatStateLabel label);
}
