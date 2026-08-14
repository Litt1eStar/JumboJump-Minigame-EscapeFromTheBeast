using System;
using JumboJumps.EFTB.Model.Obstacle;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIHazardObstacle : MonoBehaviour
    {
        public event Action<GIHazardObstacle> EventPlayerHit;
        public event Action<GIHazardObstacle> EventDespawnRequested;

        private Collider2D hazardCollider;

        private HazardDirectionEnum direction;
        private float speed;
        private float despawnX;
        private bool hasTriggered;
        private bool isMoving;

        public float RowWorldY { get; private set; }

        public void Initialize(HazardDirectionEnum direction, float speed, float rowWorldY, float despawnX)
        {
            hazardCollider = GetComponent<Collider2D>();

            this.direction = direction;
            this.speed = speed;
            this.RowWorldY = rowWorldY;
            this.despawnX = despawnX;
            this.hasTriggered = false;
            this.isMoving = true;

            float yRotation = (direction == HazardDirectionEnum.LeftToRight) ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            if (hazardCollider != null)
            {
                hazardCollider.enabled = true;
            }
        }

        public void SetMoving(bool moving)
        {
            isMoving = moving;
        }

        private void Update()
        {
            if (!isMoving) return;

            float moveStep = (int)direction * speed * Time.deltaTime;
            transform.position += new Vector3(moveStep, 0f, 0f);

            bool passedDespawn = (direction == HazardDirectionEnum.LeftToRight)
                ? transform.position.x >= despawnX
                : transform.position.x <= despawnX;

            if (passedDespawn)
            {
                EventDespawnRequested?.Invoke(this);
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
                    EventPlayerHit?.Invoke(this);
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
