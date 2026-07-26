using JumboJumps.EFTB.Model;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class Input2DManager : MonoBehaviour
    {
        public event Action EventTap;
        public event Action<SwipeDirectionEnum> EventSwipe;

        [Header("Settings")]
        [SerializeField]
        private float swipeThreshold = 50f;

        private Vector2 startTouchPosition;
        private float swipeThresholdSquare;
        private float touchDuration;
        private bool isTouchingScreen;
        private bool isSwiping;

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

        private void HandleTouchBegan(Touch touch)
        {
            isTouchingScreen = true;
            isSwiping = false;
            startTouchPosition = touch.position;
            touchDuration = 0f;
        }

        private void HandleTouchMoved(Touch touch)
        {
            if (isSwiping) return;

            Vector2 moveDelta = touch.position - startTouchPosition;
            if (moveDelta.sqrMagnitude > swipeThresholdSquare)
            {
                isSwiping = true;
                HandleSwipe(moveDelta);
            }
        }

        public void HandleTouchEnded(Touch? touch = null)
        {
            if (!isTouchingScreen) return;

            if (!isSwiping)
            {
                EventTap?.Invoke();
            }

            isTouchingScreen = false;
            isSwiping = false;
        }

        public void HandleSwipe(Vector2 swipedVector)
        {
            SwipeDirectionEnum dir = swipedVector.x > 0 ? SwipeDirectionEnum.Right : SwipeDirectionEnum.Left;

            if (Mathf.Abs(swipedVector.x) >= Mathf.Abs(swipedVector.y))
            {
                EventSwipe?.Invoke(dir);
            }
            else if (swipedVector.y > 0)
            {
                EventTap?.Invoke();
            }
        }
    }
}
