using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    /// <summary>
    /// View entity component attached to static furniture obstacle instances.
    /// Stores grid lane and world Y position metadata for cell collision queries.
    /// </summary>
    public class GIFurnitureObstacle : MonoBehaviour
    {
        public int LaneIndex { get; private set; }
        public float WorldY { get; private set; }

        public void Initialize(int laneIndex, float worldY)
        {
            LaneIndex = laneIndex;
            WorldY = worldY;
        }

        /// <summary>
        /// Returns true if this furniture obstacle occupies the specified lane and world Y coordinate.
        /// </summary>
        public bool BlocksCell(int targetLaneIndex, float targetWorldY, float tolerance = 1.5f)
        {
            if (targetLaneIndex != LaneIndex) return false;
            return Mathf.Abs(targetWorldY - WorldY) < tolerance;
        }
    }
}
