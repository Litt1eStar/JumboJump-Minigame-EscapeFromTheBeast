using JumboJump.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class Input2DManager : MonoBehaviour
    {
        public event Action EventTap;
        public event Action EventHoldStarted;
        public event Action EventHoldEnded;

        /// <summary>
        /// Parameter : Swipe Direction to told event that what direction player had swipe the screen
        /// </summary>
        public event Action<SwipeDirectionEnum> EventSwipe;

        [Header("Settings")]
        [SerializeField]
        private float swipeThreshold = 50f;

        [SerializeField]
        private float holdThreshold = 0.1f;

        private Vector2 startTouchPosition;
        private float swipeThresholdSquare;
        private float touchDuration;
        private bool isTouchingScreen;
        private bool isSwiping;
        private bool isHoldTriggered;

        public bool IsTouchingScreen => isTouchingScreen;

        public void Initialize()
        {
            swipeThresholdSquare = swipeThreshold * swipeThreshold;
        }

        public void Dispose()
        {
            
        }

        public void UpdateLogic(float deltaTime)
        {
            if (Input.touchCount <= 0)
            {
                if (isTouchingScreen)
                {
                    HandleTouchEnded();
                }

                return;
            }

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                {
                    HandleTouchBegan(touch);
                    break;
                }
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                {
                    touchDuration += deltaTime;
                    HandleTouchMoved(touch);
                    break;
                }
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                { 
                    HandleTouchEnded(touch);
                    break;
                }
            }
        }

        private void HandleTouchMoved(Touch touch)
        {
            if (isSwiping) return;

            Vector2 moveDelta = touch.position - startTouchPosition;
            if (moveDelta.sqrMagnitude > swipeThresholdSquare)
            {
                isSwiping = true;

                if (isHoldTriggered)
                {
                    EventHoldEnded?.Invoke();
                }

                HandleSwipe(moveDelta);
                return;
            }

            if (!isHoldTriggered && !isSwiping && touchDuration > holdThreshold)
            {
                isHoldTriggered = true;
                EventHoldStarted?.Invoke();
            }
        }

        private void HandleTouchBegan(Touch touch)
        {
            isTouchingScreen = true;
            isSwiping = false;
            isHoldTriggered = false;
            startTouchPosition = touch.position;
            touchDuration = 0f;
        }

        public void HandleTouchEnded(Touch? touch = null)
        {
            if (!isTouchingScreen) return;

            if (isHoldTriggered)
            {
                EventHoldEnded?.Invoke();
            }

            if (!isSwiping && touch.HasValue)
            {
                Vector2 finalDelta = touch.Value.position - startTouchPosition;

                if (finalDelta.sqrMagnitude < swipeThresholdSquare && touchDuration < holdThreshold)
                {
                    EventTap?.Invoke();
                }
            }

            isTouchingScreen = false;
            isHoldTriggered = false;
            isSwiping = false;
        }
        public void HandleSwipe(Vector2 swipedVector)
        {
            if (Mathf.Abs(swipedVector.x) < Mathf.Abs(swipedVector.y)) return;

            if (swipedVector.x > 0)
            {
                EventSwipe?.Invoke(SwipeDirectionEnum.Right);
            }
            else
            {
                EventSwipe?.Invoke(SwipeDirectionEnum.Left);
            }
        }
    }
}
