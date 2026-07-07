using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIMovingObstacle : MonoBehaviour
    {
        private GameplayController gameplayController;
        private GameplayStateManager gameplayStateManager;
        private float speed;
        private bool hasTriggered = false;

        public void Initialize(float speed)
        {
            gameplayController = GameContext.Instance.Get<GameplayController>();    
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();

            this.speed = speed;
            hasTriggered = false;
        }

        private void Update()
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hasTriggered) return;

            if (collision.GetComponent<GIPlayer>() != null)
            {
                hasTriggered = true;
                
                if (gameplayController != null)
                {
                    gameplayController.InvokeFinishLevel(GameStatus.Lose);
                }
            }
        }
    }
}
