using JumboJump.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class Input2DManager : MonoBehaviour
    {
        public event Action EventTap;
        public event Action EventHold;

        /// <summary>
        /// Parameter : Swipe Direction to told event that what direction player had swipe the screen
        /// </summary>
        public event Action<SwipeDirectionEnum> EventSwipe;

        [Header("Settings")]
        [SerializeField]
        private float swipeThreshold = 50f;

        [SerializeField]
        private float holdThreshold = 0.5f;

        private Vector2 startTouchPosition;
        private float touchStartTime;
        private bool isHolding;

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
            if (Input.touchCount <= 0) return;

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    touchStartTime = Time.time;
                    isHolding = false;

                    break;

                case TouchPhase.Stationary:
                    float currentDuration = Time.time - touchStartTime;
                    
                    if(!isHolding && currentDuration >= holdThreshold)
                    {
                        isHolding = true;
                        EventHold?.Invoke();
                    }

                    break;

                case TouchPhase.Ended:
                    float duration = Time.time - touchStartTime;
                    Vector2 endTouchPosition = touch.position;
                    Vector2 swipedVector = endTouchPosition - startTouchPosition;

                    if (swipedVector.magnitude > swipeThreshold)
                    {
                        SwipePerforming(swipedVector);
                    }
                    else if (duration < holdThreshold)
                    {
                        EventTap?.Invoke();
                    }

                    break;
            }
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
