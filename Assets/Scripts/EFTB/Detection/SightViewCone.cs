using System;
using System.Numerics;

namespace Assets.Scripts.EFTB.Detection
{
    public class SightViewCone
    {
        public event Action EventTargetSpotted;
        public event Action EventTargetLost;
        private float fovAngle;
        private float range;
        public bool IsTargetVisible { get; private set; }
        
        public SightViewCone(float fovAngle, float range)
        {
            this.fovAngle = fovAngle;
            this.range = range;
        }

        public void Initialize()
        {
            IsTargetVisible = false;
        }
        public void UpdateLogic(float deltaTime, Vector2 sightOrigin)
        {
            //Placeholder for target detection logic
        }
        public void Dispose()
        {

        }
    }
}
