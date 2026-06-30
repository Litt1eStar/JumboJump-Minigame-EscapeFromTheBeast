using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIMovingObstacle : MonoBehaviour
    {
        private GameplayStateManager gameplayStateManager;
        private GameplayController gameplayController;
        private float speed;
        private bool hasTriggered = false;

        public void Initialize(float speed)
        {
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();    
            gameplayController = gameplayStateManager.GameplayController;

            this.speed = speed;
            hasTriggered = false;
        }

        private void Update()
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hasTriggered) return;

            if (collision.GetComponent<GIPlayer>() != null)
            {
                hasTriggered = true;
                
                if (gameplayStateManager != null && gameplayController != null)
                {
                    DebugLogHelper.Log($"[{GetType().Name}] Player hit by moving obstacle -> Triggering Game Over.");
                    gameplayController.InvokeFinishLevel(GameStatus.Lose);
                }
            }
        }
    }
}
