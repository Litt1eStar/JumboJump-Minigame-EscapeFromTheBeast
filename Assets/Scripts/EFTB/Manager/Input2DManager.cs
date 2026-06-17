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
        private float touchStartTime;
        private bool isTouchingScreen;
        private bool isSwiping;
        private bool isHoldTriggered;

        public bool IsTouchingScreen => isTouchingScreen;

        public void Initialize()
        {
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
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
                    isTouchingScreen = true;
                    isSwiping = false;
                    isHoldTriggered = false;
                    startTouchPosition = touch.position;
                    touchStartTime = Time.time;
                    break;

                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    if (isSwiping) return;

                    Vector2 moveDelta = touch.position - startTouchPosition;
                    if (moveDelta.magnitude > swipeThreshold)
                    {
                        isSwiping = true;

                        if (isHoldTriggered)
                        {
                            EventHoldEnded?.Invoke();
                        }

                        EventHoldEnded?.Invoke();
                        SwipePerforming(moveDelta);
                        return;
                    }

                    if (!isHoldTriggered && !isSwiping && (Time.time - touchStartTime) > holdThreshold)
                    {
                        isHoldTriggered = true;
                        EventHoldStarted?.Invoke();
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    HandleTouchEnded(touch);
                    break;
            }
        }

        public void HandleTouchEnded(Touch? touch = null)
        {
            if (!isTouchingScreen) return;
            isTouchingScreen = false;

            if (isHoldTriggered)
            {
                EventHoldEnded?.Invoke();
            }

            EventHoldEnded?.Invoke();

            if (!isSwiping && touch.HasValue)
            {
                float duration = Time.time - touchStartTime;
                Vector2 finalDelta = touch.Value.position - startTouchPosition;

                if (finalDelta.magnitude < swipeThreshold && duration < holdThreshold)
                {
                    EventTap?.Invoke();
                }
            }

            isTouchingScreen = false;
            isHoldTriggered = false;
            isSwiping = false;
        }
        public void SwipePerforming(Vector2 swipedVector)
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
