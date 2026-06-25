using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIPlayer : MonoBehaviour
    {
        [Header("Player Reference")]
        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [Header("Player Configuration")]
        [SerializeField]
        private float playerMovementSpeed = 5f;

        [SerializeField]
        private Transform initialStartPosition;

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

        public void SetInitialStartPosition()
        {
            transform.position = initialStartPosition.position;
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
