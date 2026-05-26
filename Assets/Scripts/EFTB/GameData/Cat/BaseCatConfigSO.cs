using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using Assets.Scripts.EFTB.UI;
using UnityEngine;


public abstract class BaseCatConfigSO : ScriptableObject
{
    public abstract ICatStateController BuildStateController(GICat giCat, Transform transform, UICatStateLabel label);
}
