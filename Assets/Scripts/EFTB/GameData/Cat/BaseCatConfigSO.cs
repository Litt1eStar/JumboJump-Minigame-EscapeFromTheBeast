using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseCatConfigSO", menuName = "Scriptable Objects/BaseCatConfigSO")]
public abstract class BaseCatConfigSO : ScriptableObject
{
    public abstract ICatStateContrller BuildStateController(GICatSight sight, Transform transform);
}
