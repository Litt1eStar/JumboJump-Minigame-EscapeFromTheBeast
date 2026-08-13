using System;
using System.Collections;
using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Utilities;
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

        private CoroutineHelper coroutineHelper;
        private Coroutine warningCoroutine;
        private Coroutine moveAnimCoroutine;
        private Color originalSpriteColor = Color.white;

        public Vector3 PlayerPosition => playerTransform.position;

        public void Initialize()
        {
            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            
            if (spriteRenderer != null)
            {
                originalSpriteColor = spriteRenderer.color;
            }
        }

        public void Dispose()
        {
            StopPounceWarning();
            StopMoveAnimRoutine();
        }

        public void SetMovingAnimation(bool isMoving)
        {
            if (animator == null) return;

            if (isMoving)
            {
                StopMoveAnimRoutine();
                animator.SetBool(ConstGameplay.Player.MOVING_ANIM_PARAM, true);
            }
            else
            {
                if (moveAnimCoroutine == null && coroutineHelper != null)
                {    
                    moveAnimCoroutine = coroutineHelper.Play(ResetMovingAnimRoutine(), this);  
                }
            }
        }

        private IEnumerator ResetMovingAnimRoutine()
        {
            if (animator == null) yield break;

            yield return new WaitForSeconds(ConstGameplay.Player.MIN_MOVE_ANIM_DURATION);
            
            animator.SetBool(ConstGameplay.Player.MOVING_ANIM_PARAM, false);
            moveAnimCoroutine = null;
        }

        private void StopMoveAnimRoutine()
        {
            if (moveAnimCoroutine != null)
            {
                if (coroutineHelper != null)
                {
                    coroutineHelper.Stop(moveAnimCoroutine, this);
                }

                moveAnimCoroutine = null;
            }
        }

        public void ShowPounceWarning(float duration, Action onComplete)
        {
            StopPounceWarning();
            if (coroutineHelper != null)
            {
                warningCoroutine = coroutineHelper.Play(PounceWarningRoutine(duration, onComplete), this);
            }
            else
            {
                warningCoroutine = StartCoroutine(PounceWarningRoutine(duration, onComplete));
            }
        }

        public void StopPounceWarning()
        {
            if (coroutineHelper != null && warningCoroutine != null)
            {
                coroutineHelper.Stop(warningCoroutine, this);
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
            float flashInterval = ConstGameplay.Cat.AggressiveCat.POUNCE_FLASH_INTERVAL;
            bool isFlashColor = false;

            while (elapsed < duration)
            {
                if (spriteRenderer != null)
                {
                    isFlashColor = !isFlashColor;
                    spriteRenderer.color = isFlashColor 
                        ? ConstGameplay.Cat.AggressiveCat.POUNCE_FLASH_COLOR 
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
            if (playerTransform != null && initialStartPosition != null)
            {
                playerTransform.position = initialStartPosition.position;
            }
            else if (initialStartPosition != null)
            {
                transform.position = initialStartPosition.position;
            }
        }

        public void MoveForward(float deltaTime)
        {
            playerTransform.position += new Vector3(0, playerMovementSpeed * deltaTime, 0);
        }
    }
}
