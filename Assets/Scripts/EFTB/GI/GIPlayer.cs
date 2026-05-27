using UnityEngine;

namespace Assets.Scripts.EFTB.GI
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

        public void Initialize()
        {

        }
        public void Dispose()
        {

        }

        public void Move(float input)
        {
            FlipSpriteBasedFromInputDirection(input);
            playerTransform.position += new Vector3(input * playerMovementSpeed * Time.deltaTime, 0, 0);
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
