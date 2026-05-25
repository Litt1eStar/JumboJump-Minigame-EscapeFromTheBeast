using Assets.Scripts.EFTB.Detection;
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
        private float fovAngle;
        
        [SerializeField]
        private float range;

        [SerializeField]
        private LayerMask sightBlockerLayerMask;

        private SightViewCone sightViewConeData;
        public void Initialize()
        {
            sightViewConeData = new SightViewCone(fovAngle, range, sightBlockerLayerMask);
        }

        public void Dispose()
        {
            sightViewConeData = null;
        }

        public void UpdateLogic(float deltaTime)
        {
            //Placeholder logic for Sight View Logic Calculation
        }
    }
}
