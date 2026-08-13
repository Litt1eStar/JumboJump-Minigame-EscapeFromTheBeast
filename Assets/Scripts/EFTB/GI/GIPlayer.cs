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
                StopMoveAnimRoutine();
                if (coroutineHelper != null)
                {
                    moveAnimCoroutine = coroutineHelper.Play(ResetMovingAnimRoutine(), this);
                }
                else
                {
                    moveAnimCoroutine = StartCoroutine(ResetMovingAnimRoutine());
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
                else
                {
                    StopCoroutine(moveAnimCoroutine);
                }

                moveAnimCoroutine = null;
            }
        }

        public void ShowPounceWarning(float duration, Action onComplete)
        {
            ShowPounceWarning(duration,
                              ConstGameplay.Cat.AggressiveCat.POUNCE_WARNING_SHAKE_SPEED,
                              ConstGameplay.Cat.AggressiveCat.POUNCE_WARNING_MAX_Z_ROTATION,
                              onComplete);
        }

        public void ShowPounceWarning(float duration, float shakeSpeed, float maxZAngle, Action onComplete)
        {
            StopPounceWarning();
            
            warningCoroutine = coroutineHelper.Play(PounceWarningRoutine(duration, shakeSpeed, maxZAngle, onComplete), this);
            
        }

        private Quaternion originalWarningLocalRotation;
        private SpriteRenderer warningIndicatorSpriteRenderer;

        public void StopPounceWarning()
        {
            if (coroutineHelper != null && warningCoroutine != null)
            {
                coroutineHelper.Stop(warningCoroutine, this);
                warningCoroutine = null;
            }

            if (warningIndicatorObject != null)
            {
                warningIndicatorObject.transform.localRotation = originalWarningLocalRotation;
                warningIndicatorObject.SetActive(false);

                if (warningIndicatorSpriteRenderer != null)
                {
                    Color c = warningIndicatorSpriteRenderer.color;
                    c.a = 1f;
                    warningIndicatorSpriteRenderer.color = c;
                }
            }
        }

        private IEnumerator PounceWarningRoutine(float duration, float shakeSpeed, float maxZAngle, Action onComplete)
        {
            if (warningIndicatorObject != null)
            {
                warningIndicatorObject.SetActive(true);

                if (warningIndicatorSpriteRenderer == null)
                {
                    warningIndicatorSpriteRenderer = warningIndicatorObject.GetComponent<SpriteRenderer>();
                    if (warningIndicatorSpriteRenderer == null)
                    {
                        warningIndicatorSpriteRenderer = warningIndicatorObject.GetComponentInChildren<SpriteRenderer>();
                    }
                }

                originalWarningLocalRotation = warningIndicatorObject.transform.localRotation;
            }

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 1f;

                if (warningIndicatorObject != null)
                {
                    float zAngle = Mathf.Sin(elapsed * shakeSpeed) * maxZAngle;
                    warningIndicatorObject.transform.localRotation = originalWarningLocalRotation * Quaternion.Euler(0f, 0f, zAngle);

                    if (warningIndicatorSpriteRenderer != null)
                    {
                        Color c = warningIndicatorSpriteRenderer.color;
                        c.a = Mathf.Lerp(0f, 1f, progress);
                        warningIndicatorSpriteRenderer.color = c;
                    }
                }

                yield return null;
            }

            StopPounceWarning();
            onComplete?.Invoke();
        }

        public void SetAlpha(float alpha)
        {
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = alpha;
                spriteRenderer.color = c;
            }

            SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in childRenderers)
            {
                if (r != null)
                {
                    Color c = r.color;
                    c.a = alpha;
                    r.color = c;
                }
            }
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
