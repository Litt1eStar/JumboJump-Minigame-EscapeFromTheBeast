using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIPlayer : MonoBehaviour
    {
        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private float playerMovementSpeed = 5f;

        public Vector3 PlayerPosition => playerTransform.position;

        public void Initialize()
        {

        }

        public void Dispose()
        {

        }

        public void SetXPosition(float x)
        {
            playerTransform.position = new Vector3(x, playerTransform.position.y, playerTransform.position.z);
        }

        public void SetPlayerOnMiddleLane()
        {
            playerTransform.position = Vector3.zero;
        }

        public void MoveForward(float deltaTime)
        {
            playerTransform.position += new Vector3(0, playerMovementSpeed * deltaTime, 0);
        }

        private void FlipSpriteBasedFromInputDirection(float input)
        {
            if (input > 0)
            {
                //face right
                spriteRenderer.flipX = false;
            }
            else if (input < 0)
            {
                //face left
                spriteRenderer.flipX = true;
            }
        }
    }
}
