using UnityEngine;
using Unity.Cinemachine;
using JumboJumps.EFTB.GI;

namespace JumboJumps.EFTB.Utilities
{
    /// <summary>
    /// A custom Cinemachine v3 extension to lock the camera's position on specific axes.
    /// This is especially useful for vertical or horizontal scroll games to lock the camera 
    /// from moving sideways (horizontal / X-axis) when following a target.
    /// </summary>
    [ExecuteAlways]
    [AddComponentMenu("Cinemachine/User Extensions/Cinemachine Lock Axis")]
    public class CinemachineLockAxis : CinemachineExtension
    {
        [Header("Lock X Axis (Horizontal)")]
        [Tooltip("Lock the camera's X position")]
        [SerializeField] private bool lockX = true;
        [Tooltip("If true, locks the camera to the X position it starts with in the editor")]
        [SerializeField] private bool useInitialX = true;
        [Tooltip("Custom X position to lock to (if Use Initial X is false)")]
        [SerializeField] private float lockedX = 0f;

        [Header("Lock Y Axis (Vertical)")]
        [Tooltip("Lock the camera's Y position")]
        [SerializeField] private bool lockY = false;
        [Tooltip("If true, locks the camera to the Y position it starts with in the editor")]
        [SerializeField] private bool useInitialY = true;
        [Tooltip("Custom Y position to lock to (if Use Initial Y is false)")]
        [SerializeField] private float lockedY = 0f;

        [Header("Lock Z Axis (Depth)")]
        [Tooltip("Lock the camera's Z position")]
        [SerializeField] private bool lockZ = false;
        [Tooltip("If true, locks the camera to the Z position it starts with in the editor")]
        [SerializeField] private bool useInitialZ = true;
        [Tooltip("Custom Z position to lock to (if Use Initial Z is false)")]
        [SerializeField] private float lockedZ = -10f;

        [Header("Lock Rotation")]
        [Tooltip("Lock the camera's rotation to its starting rotation")]
        [SerializeField] private bool lockRotation = true;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        protected override void Awake()
        {
            base.Awake();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                var player = SceneObjectContext.Instance?.Get<GIPlayer>();
                if (player != null)
                {
                    initialPosition = player.PlayerPosition;
                }
            }
        }

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Finalize)
            {
                Vector3 pos = state.RawPosition;

                if (lockX)
                {
                    pos.x = useInitialX ? initialPosition.x : lockedX;
                }

                if (lockY)
                {
                    pos.y = useInitialY ? initialPosition.y : lockedY;
                }

                if (lockZ)
                {
                    pos.z = useInitialZ ? initialPosition.z : lockedZ;
                }

                state.RawPosition = pos;

                if (lockRotation)
                {
                    state.RawOrientation = initialRotation;
                }
            }
        }
    }
}
