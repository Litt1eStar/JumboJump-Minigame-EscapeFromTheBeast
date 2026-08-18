using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Model.Obstacle;
using JumboJumps.EFTB.State.Gameplay;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIHazardObstacle : MonoBehaviour
    {
        private GameplayController gameplayController;
        private GameplayStateManager gameplayStateManager;
        private Collider2D hazardCollider;
        private SpriteRenderer spriteRenderer;

        private HazardDirectionEnum direction;
        private float speed;
        private float despawnX;
        private bool hasTriggered;

        public float RowWorldY { get; private set; }

        private ObjectPoolManager poolManager;

        public void Initialize(HazardDirectionEnum direction, float speed, float rowWorldY, float despawnX)
        {
            gameplayController = GameContext.Instance.Get<GameplayController>();
            gameplayStateManager = GameContext.Instance.Get<GameplayStateManager>();
            poolManager = GameContext.Instance.Get<ObjectPoolManager>();

            hazardCollider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            this.direction = direction;
            this.speed = speed;
            this.RowWorldY = rowWorldY;
            this.despawnX = despawnX;
            this.hasTriggered = false;

            float yRotation = (direction == HazardDirectionEnum.LeftToRight) ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            if (hazardCollider != null)
            {
                hazardCollider.enabled = true;
            }
        }

        private void Update()
        {
            if (gameplayStateManager == null || gameplayStateManager.StateController == null || !(gameplayStateManager.StateController.CurrentState is InGameState))
            {
                return;
            }

            float moveStep = (int)direction * speed * Time.deltaTime;
            transform.position += new Vector3(moveStep, 0f, 0f);

            bool passedDespawn = (direction == HazardDirectionEnum.LeftToRight)
                ? transform.position.x >= despawnX
                : transform.position.x <= despawnX;

            if (passedDespawn)
            {
                RecycleSelf();
            }
        }

        private void RecycleSelf()
        {
            if (poolManager == null)
            {
                poolManager = GameContext.Instance?.Get<ObjectPoolManager>();
            }

            if (poolManager != null)
            {
                poolManager.Recycle(gameObject);
            }
        }

        private void HandlePlayerCollision(GameObject collidedObj)
        {
            if (hasTriggered) return;

            if (collidedObj != null)
            {
                GIPlayer player = collidedObj.GetComponent<GIPlayer>() ?? collidedObj.GetComponentInParent<GIPlayer>();
                if (player != null)
                {
                    hasTriggered = true;

                    if (gameplayController == null)
                    {
                        gameplayController = GameContext.Instance?.Get<GameplayController>();
                    }

                    if (gameplayController != null)
                    {
                        gameplayController.InvokeFinishLevel(GameStatus.Lose);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HandlePlayerCollision(collision != null ? collision.gameObject : null);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            HandlePlayerCollision(collision != null ? collision.gameObject : null);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandlePlayerCollision(collision != null ? collision.gameObject : null);
        }
    }
}
