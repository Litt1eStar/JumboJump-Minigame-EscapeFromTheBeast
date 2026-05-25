using System;
using System.Numerics;
using UnityEngine;

namespace Assets.Scripts.EFTB.Detection
{
    public class SightViewCone
    {
        public readonly LayerMask SIGHT_BLOCKER_LAYER_MASK;        
        public float FovAngle { get; private set; }
        public float Range {  get; private set; }

        public SightViewCone(float fovAngle, float range, LayerMask sightBlockerLayerMask)
        {
            FovAngle = fovAngle;
            Range = range;
            SIGHT_BLOCKER_LAYER_MASK = sightBlockerLayerMask;
        }
    }
}
