using System;
using UnityEngine;

namespace Assets.Scripts.EFTB.GI
{
    public class GICatSight : MonoBehaviour
    {
        [Header("Sight View Cone Configuration")]
        [SerializeField]
        private Transform sightOrigin;

        [SerializeField]
        private Transform target;

        [SerializeField]
        private float fovAngle;
        
        [SerializeField]
        private float range;

        [SerializeField]
        private LayerMask sightBlockerLayerMask;

        public event Action OnTargetSpotted;
        public event Action OnTargetLost;
        public bool IsTargetInSight { get; private set; }
        public void UpdateLogic(float deltaTime)
        {
            //Placeholder logic for Sight View Logic Calculation
        }
    }
}
