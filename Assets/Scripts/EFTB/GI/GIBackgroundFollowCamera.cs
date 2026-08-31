using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    /// <summary>
    /// View component attached to background GameObjects to follow the Main Camera position smoothly.
    /// Executes positioning updates in LateUpdate to avoid visual jitter after camera tracking updates.
    /// </summary>
    public class GIBackgroundFollowCamera : MonoBehaviour
    {
        [Header("Target Camera")]
        [Tooltip("Target camera to follow. If null, automatically resolves Camera.main or SceneObjectContext Camera.")]
        [SerializeField] private Camera targetCamera;

        [Header("Follow Axes Settings")]
        [Tooltip("Follow the camera's X position (horizontal)")]
        [SerializeField] private bool followX = false;

        [Tooltip("Follow the camera's Y position (vertical)")]
        [SerializeField] private bool followY = true;

        [Tooltip("Follow the camera's Z position (depth)")]
        [SerializeField] private bool followZ = false;

        [Header("Offset Settings")]
        [Tooltip("Custom position offset relative to target camera position")]
        [SerializeField] private Vector3 positionOffset = Vector3.zero;

        [Tooltip("Use the initial offset distance between this object and the camera at start")]
        [SerializeField] private bool useInitialOffset = true;

        private void Start()
        {
            EnsureTargetCamera();

            if (useInitialOffset && targetCamera != null)
            {
                positionOffset = transform.position - targetCamera.transform.position;
            }
        }

        private void LateUpdate()
        {
            if (targetCamera == null)
            {
                EnsureTargetCamera();
                if (targetCamera == null) return;
            }

            Vector3 camPos = targetCamera.transform.position;
            Vector3 currentPos = transform.position;

            float newX = followX ? camPos.x + positionOffset.x : currentPos.x;
            float newY = followY ? camPos.y + positionOffset.y : currentPos.y;
            float newZ = followZ ? camPos.z + positionOffset.z : currentPos.z;

            transform.position = new Vector3(newX, newY, newZ);
        }

        private void EnsureTargetCamera()
        {
            if (targetCamera != null) return;

            targetCamera = Camera.main;

            if (targetCamera == null && SceneObjectContext.Instance != null)
            {
                targetCamera = SceneObjectContext.Instance.Get<Camera>();
            }

            if (targetCamera == null)
            {
                targetCamera = Object.FindAnyObjectByType<Camera>();
            }

            if (targetCamera == null)
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Target Camera could not be resolved.");
            }
        }
    }
}
