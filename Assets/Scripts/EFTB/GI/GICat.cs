using JumboJumps.EFTB.GameData.Cat;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.UI;
using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{

    public enum CatSightDirection
    {
        Left = 1,
        Right =2 
    }

    public class GICat : MonoBehaviour
    {
        public event Action EventTargetSpotted;
        public event Action EventTargetLost;
        public event Action EventStateChanged;

        [Header("Sight View Cone Configuration")]
        [SerializeField]
        private Transform sightOrigin;
        
        [SerializeField]
        private float fovAngle;
        
        [SerializeField]
        private float range;

        [SerializeField]
        private Vector3 direction;
        
        [SerializeField]
        private LayerMask sightBlockerLayerMask;

        [Header("Reference")]
        [SerializeField] 
        private UICatStateLabel uiCatStateLabel;

        [SerializeField]
        private BaseCatConfigSO config;

        [Header("Debug Visualization")]
        [SerializeField]
        private bool drawGizmo = true;
        
        [SerializeField, Range(8, 64)] 
        private int arcSegments = 24;
        
        [SerializeField] 
        private Color colorClear = new Color(0f, 1f, 0f, 0.6f);
        
        [SerializeField] 
        private Color colorSpotted = new Color(1f, 0f, 0f, 0.8f);
        public bool IsTargetInSight { get; private set; }
        private Transform target;

        public ICatStateController BuildStateController(Transform target)
        {
            this.target = target;
            var controller = config.BuildStateController(this, target, uiCatStateLabel);
            return controller;
        }

        public void UpdateLogic(float deltaTime)
        {
            if (sightOrigin == null || target == null)
            {
                DebugLogHelper.LogError("Sight origin or target not set. Cannot compute visibility.");
                return;
            }

            bool wasVisible = IsTargetInSight;
            IsTargetInSight = ComputeVisible();

            if (wasVisible == IsTargetInSight) return;

            if (IsTargetInSight)
            {
                EventTargetSpotted?.Invoke();
            }
            else
            {
                EventTargetLost?.Invoke();
            }
        }

        private bool ComputeVisible()
        {
            Vector2 origin = sightOrigin.position;
            Vector2 facing = direction;
            Vector2 toTarget = target.position - sightOrigin.position;

            if (toTarget.sqrMagnitude > range * range) return false;
            if (Vector2.Angle(facing, toTarget) > fovAngle * 0.5f) return false;

            var hit = Physics2D.Raycast(origin, toTarget.normalized, toTarget.magnitude, sightBlockerLayerMask);
            return hit.collider == null;
        }

        public CatSightDirection CurrentSightDirection { get; private set; } = CatSightDirection.Left;

        public void SetDirection(CatSightDirection catSightDirection)
        {
            CurrentSightDirection = catSightDirection;
            if (catSightDirection == CatSightDirection.Left)
            {
                direction = -sightOrigin.right;
            }
            else if (catSightDirection == CatSightDirection.Right)
            {
                direction = sightOrigin.right;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!drawGizmo || sightOrigin == null) return;

            Vector2 origin = sightOrigin.position;
            Vector2 facing = direction;
            if (facing.sqrMagnitude < 0.0001f)
            {
                facing = -sightOrigin.right;
            }

            Gizmos.color = IsTargetInSight ? colorSpotted : colorClear;

            float half = fovAngle * 0.5f;

            // Edge lines
            Vector2 leftEnd = origin + (Vector2)(Quaternion.Euler(0, 0, +half) * facing) * range;
            Vector2 rightEnd = origin + (Vector2)(Quaternion.Euler(0, 0, -half) * facing) * range;
            Gizmos.DrawLine(origin, leftEnd);
            Gizmos.DrawLine(origin, rightEnd);

            // Arc
            Vector2 prev = rightEnd;
            for (int i = 1; i <= arcSegments; i++)
            {
                float t = (float)i / arcSegments;
                float angle = Mathf.Lerp(-half, +half, t);
                Vector2 curr = origin + (Vector2)(Quaternion.Euler(0, 0, angle) * facing) * range;
                Gizmos.DrawLine(prev, curr);
                prev = curr;
            }

            // Ray to target (only useful in Play mode when target is assigned)
            if (target != null)
            {
                Gizmos.color = IsTargetInSight ? Color.red : new Color(1f, 1f, 1f, 0.3f);
                Gizmos.DrawLine(origin, target.position);
            }
        }
#endif
    }
}
