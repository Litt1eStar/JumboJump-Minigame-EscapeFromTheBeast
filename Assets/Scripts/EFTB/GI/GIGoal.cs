using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJump.Assets.Scripts.EFTB.GI
{
    public class GIGoal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                GameContext.Instance.Get<GameplayStateManager>().InvokeFinishLevel(GameStatus.Win);
            }
        }
    }
}
