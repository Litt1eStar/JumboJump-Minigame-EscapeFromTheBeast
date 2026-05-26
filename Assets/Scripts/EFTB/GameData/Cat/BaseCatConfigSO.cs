using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using UnityEngine;


public abstract class BaseCatConfigSO : ScriptableObject
{
    public abstract ICatStateController BuildStateController(GICatSight sight, Transform transform);
}
