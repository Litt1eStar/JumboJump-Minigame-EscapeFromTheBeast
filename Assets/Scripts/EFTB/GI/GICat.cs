using Assets.Scripts.EFTB.Interface;
using System;
using UnityEngine;

namespace Assets.Scripts.EFTB.GI
{
    public class GICat : MonoBehaviour
    {
        [Header("Sight View Cone Configuration")]
        [SerializeField]
        private Transform sightOrigin;
        [SerializeField]
        private float fovAngle;
        [SerializeField]
        private float range;
        [SerializeField]
        private LayerMask sightBlockerLayerMask;

        public event Action OnTargetSpotted;
        public event Action OnTargetLost;
        public bool IsTargetInSight { get; private set; }
        private Transform target;

        [SerializeField]
        private BaseCatConfigSO config;

        [Header("Debug Visualization")]
        [SerializeField] private bool drawGizmo = true;
        [SerializeField, Range(8, 64)] private int arcSegments = 24;
        [SerializeField] private Color colorClear = new Color(0f, 1f, 0f, 0.6f);
        [SerializeField] private Color colorSpotted = new Color(1f, 0f, 0f, 0.8f);
       
        public ICatStateController BuildStateController(Transform target)
        {
            this.target = target;
            var controller = config.BuildStateController(this, target);
            return controller;
        }

        public void UpdateLogic(float deltaTime)
        {
            if (sightOrigin == null || target == null) return;

            bool wasVisible = IsTargetInSight;
            IsTargetInSight = ComputeVisible();

            if (!wasVisible && IsTargetInSight) OnTargetSpotted?.Invoke();
            else if (wasVisible && !IsTargetInSight) OnTargetLost?.Invoke();
        }

        private bool ComputeVisible()
        {
            Vector2 origin = sightOrigin.position;
            Vector2 facing = -sightOrigin.up;
            Vector2 toTarget = target.position - sightOrigin.position;

            if (toTarget.sqrMagnitude > range * range) return false;
            if (Vector2.Angle(facing, toTarget) > fovAngle * 0.5f) return false;

            var hit = Physics2D.Raycast(origin, toTarget.normalized, toTarget.magnitude, sightBlockerLayerMask);
            return hit.collider == null;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmo || sightOrigin == null) return;

            Vector2 origin = sightOrigin.position;
            Vector2 facing = -sightOrigin.up;             // matches ComputeVisible()
            if (facing.sqrMagnitude < 0.0001f) return;

            Gizmos.color = IsTargetInSight ? colorSpotted : colorClear;

            float half = fovAngle * 0.5f;

            // Edge lines
            Vector2 leftEnd = origin + Rotate(facing, +half) * range;
            Vector2 rightEnd = origin + Rotate(facing, -half) * range;
            Gizmos.DrawLine(origin, leftEnd);
            Gizmos.DrawLine(origin, rightEnd);

            // Arc
            Vector2 prev = rightEnd;
            for (int i = 1; i <= arcSegments; i++)
            {
                float t = (float)i / arcSegments;
                float angle = Mathf.Lerp(-half, +half, t);
                Vector2 curr = origin + Rotate(facing, angle) * range;
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

        private static Vector2 Rotate(Vector2 v, float degrees)
        {
            float rad = degrees * Mathf.Deg2Rad;
            float c = Mathf.Cos(rad), s = Mathf.Sin(rad);
            return new Vector2(c * v.x - s * v.y, s * v.x + c * v.y);
        }
    }
}
