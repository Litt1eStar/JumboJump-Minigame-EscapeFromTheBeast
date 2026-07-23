using System;
using System.Collections;
using JumboJumps.EFTB.Constant.Gameplay;
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

        [SerializeField]
        private GameObject warningIndicatorObject;

        [Header("Player Configuration")]
        [SerializeField]
        private float playerMovementSpeed = 5f;

        [SerializeField]
        private Transform initialStartPosition;

        private Coroutine warningCoroutine;
        private Color originalSpriteColor = Color.white;

        public Vector3 PlayerPosition => playerTransform.position;

        private void Awake()
        {
            if (spriteRenderer != null)
            {
                originalSpriteColor = spriteRenderer.color;
            }
        }

        public void Initialize()
        {
            if (spriteRenderer != null)
            {
                originalSpriteColor = spriteRenderer.color;
            }
        }

        public void Dispose()
        {
            StopPounceWarning();
        }

        private void OnDisable()
        {
            StopPounceWarning();
        }

        public void ShowPounceWarning(float duration, Action onComplete)
        {
            StopPounceWarning();
            warningCoroutine = StartCoroutine(PounceWarningRoutine(duration, onComplete));
        }

        public void StopPounceWarning()
        {
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
                warningCoroutine = null;
            }

            if (warningIndicatorObject != null)
            {
                warningIndicatorObject.SetActive(false);
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = originalSpriteColor;
            }
        }

        private IEnumerator PounceWarningRoutine(float duration, Action onComplete)
        {
            if (warningIndicatorObject != null)
            {
                warningIndicatorObject.SetActive(true);
            }

            float elapsed = 0f;
            float flashInterval = ConstGameplay.Cat.AggressiveCat.Pounce_Flash_Interval;
            bool isFlashColor = false;

            while (elapsed < duration)
            {
                if (spriteRenderer != null)
                {
                    isFlashColor = !isFlashColor;
                    spriteRenderer.color = isFlashColor 
                        ? ConstGameplay.Cat.AggressiveCat.Pounce_Flash_Color 
                        : originalSpriteColor;
                }
                yield return new WaitForSeconds(flashInterval);
                elapsed += flashInterval;
            }

            StopPounceWarning();
            onComplete?.Invoke();
        }

        public void SetPosition(Vector3 position)
        {
            playerTransform.position = position;
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
