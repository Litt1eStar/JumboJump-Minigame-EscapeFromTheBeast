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

        private Coroutine warningCoroutine;
        private CoroutineHelper coroutineHelper;
        private Color originalSpriteColor = Color.white;

        public Vector3 PlayerPosition => playerTransform.position;

        public void Initialize()
        {
            coroutineHelper = GameContext.Instance.Get<CoroutineHelper>();
            if (coroutineHelper == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] {nameof(GIPlayer)}| Failed to get {typeof(CoroutineHelper).AssemblyQualifiedName} from GameContext");
            }

            if (spriteRenderer != null)
            {
                originalSpriteColor = spriteRenderer.color;
            }
        }

        public void Dispose()
        {
            StopPounceWarning();
            coroutineHelper = null;
        }

        public void ShowPounceWarning(float duration, Action onComplete)
        {
            StopPounceWarning();
            if (coroutineHelper != null)
            {
                warningCoroutine = coroutineHelper.Restart(warningCoroutine, PounceWarningRoutine(duration, onComplete), this);
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
                spriteRenderer.color = originalSpriteColor;
            }
        }

        private IEnumerator PounceWarningRoutine(float duration, Action onComplete)
        {
            if (spriteRenderer == null)
            {
                DebugLogHelper.LogError("[GIPlayer] SpriteRender is missing in this component");
                yield return null;
            }
            
            if (warningIndicatorObject != null)
            {
                warningIndicatorObject.SetActive(true);
            }

            float elapsed = 0f;
            float flashInterval = ConstGameplay.Cat.AggressiveCat.POUNCE_FLASH_INTERVAL;
            bool isFlashColor = false;

            while (elapsed < duration)
            {
                
                isFlashColor = !isFlashColor;
                spriteRenderer.color = isFlashColor 
                    ? ConstGameplay.Cat.AggressiveCat.POUNCE_FLASH_COLOR 
                    : originalSpriteColor;
                
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
