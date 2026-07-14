using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class CameraFOVAdjuster : MonoBehaviour
    {
        [Header("Design Settings")]
        [Tooltip("The vertical FOV when the screen aspect ratio matches the design.")]
        [SerializeField] private float designFOV = 100f;
        
        [Tooltip("The reference width (e.g., 9 for 9:16 aspect ratio)")]
        [SerializeField] private float designAspectWidth = 9f;
        
        [Tooltip("The reference height (e.g., 16 for 9:16 aspect ratio)")]
        [SerializeField] private float designAspectHeight = 16f;

        [Header("Behavior")]
        [Tooltip("If true, only increases FOV on narrower screens to prevent cutting off the sides. On wider screens (like iPad), keeps Design FOV so you see extra width instead of cutting off top/bottom.")]
        [SerializeField] private bool onlyAdjustIfNarrower = true;

        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void Update()
        {
            if (cam == null) return;

            float currentAspect = (float)Screen.width / Screen.height;
            float designAspect = designAspectWidth / designAspectHeight;

            if (!onlyAdjustIfNarrower || currentAspect < designAspect)
            {
                // To maintain the exact same horizontal view width as the design,
                // we calculate the required vertical FOV for the current aspect ratio.
                float targetVFOVRad = designFOV * Mathf.Deg2Rad;
                float targetHFOVRad = 2f * Mathf.Atan(Mathf.Tan(targetVFOVRad / 2f) * designAspect);
                
                float newVFOVRad = 2f * Mathf.Atan(Mathf.Tan(targetHFOVRad / 2f) / currentAspect);
                cam.fieldOfView = newVFOVRad * Mathf.Rad2Deg;
            }
            else
            {
                // Screen is wider than design, keep the design FOV
                cam.fieldOfView = designFOV;
            }
        }
    }
}
