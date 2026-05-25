using System;
using UnityEngine;

namespace Assets.Scripts.EFTB.GI
{
    [ExecuteAlways]
    public class GICamera : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        [Header("Target")]
        [SerializeField]
        private Transform target;

        [Header("Smooth Follow")]
        [SerializeField]
        [Range(0.01f, 1f)]
        private float smoothSpeed = 0.08f;

        [SerializeField]
        private Vector3 offset;

        [Header("Bound Min, Bound Max")]
        [SerializeField]
        private Transform boundMin;
        [SerializeField]
        private Transform boundMax;
        [SerializeField]
        private float boundGap = 5f;

        public void Initialize()
        {
            
        }

        public void Dispose()
        {

        }

        public void UpdateLogic(float deltaTime)
        {
            if(target == null)
            {
                Debug.LogWarning("GICamera target is not assigned.");
                return;
            }

            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.Lerp(
                                                transform.position,
                                                desiredPosition,
                                                smoothSpeed
                                                );

            smoothPosition = ClampToBounds(smoothPosition);
            transform.position = smoothPosition;
        }

        private Vector3 ClampToBounds(Vector3 position)
        {
            if(boundMin == null || boundMax == null)
            {
                Debug.LogWarning("GICamera bounds are not assigned.");
                return position;
            }
        
            float clampedX = Mathf.Clamp(
                                        position.x,
                                        boundMin.position.x + boundGap,
                                        boundMax.position.x - boundGap
                                        );

            return new Vector3(clampedX, position.y, position.z);
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return; 

            if (target == null) return;

            
            Vector3 snappedPosition = ClampToBounds(target.position + offset);
            transform.position = snappedPosition;
        }
#endif
    }
}
