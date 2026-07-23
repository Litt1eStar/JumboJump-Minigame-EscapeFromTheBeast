using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    /// <summary>
    /// View entity component attached to static furniture obstacle instances.
    /// Stores grid lane and world Y position metadata for cell collision queries.
    /// Automatically registers with LevelGeneratorManager on enable.
    /// </summary>
    public class GIFurnitureObstacle : MonoBehaviour
    {
        public int LaneIndex { get; private set; }
        public float WorldY { get; private set; }
        private LevelGeneratorManager levelGeneratorManager;

        public void Initialize(int laneIndex, float worldY)
        {
            levelGeneratorManager = GameContext.Instance?.Get<LevelGeneratorManager>();

            float cellHeight = ConstGameplay.Obstacle.Furniture.Cell_Height;
            LaneIndex = laneIndex;
            WorldY = Mathf.RoundToInt(worldY / cellHeight) * cellHeight;
            RegisterSelf();
        }

        private void OnEnable()
        {
            UpdateWorldPositionAndLane();
            RegisterSelf();
        }

        private void OnDisable()
        {
            UnregisterSelf();
        }

        public void UpdateWorldPositionAndLane()
        {
            float cellHeight = ConstGameplay.Obstacle.Furniture.Cell_Height;
            WorldY = Mathf.RoundToInt(transform.position.y / cellHeight) * cellHeight;
            LaneIndex = GetClosestLaneIndex(transform.position.x);
        }

        private void RegisterSelf()
        {
            if (levelGeneratorManager != null)
            {
                levelGeneratorManager.RegisterFurnitureObstacle(this);
            }
        }

        private void UnregisterSelf()
        {
            if (levelGeneratorManager != null)
            {
                levelGeneratorManager.UnregisterFurnitureObstacle(this);
            }
        }

        /// <summary>
        /// Returns true if this furniture obstacle occupies the specified lane and world Y coordinate.
        /// Evaluates grid cell row matching, lane position proximity, and world transform coordinates.
        /// </summary>
        public bool BlocksCell(int targetLaneIndex, float targetWorldY, float tolerance = 1.8f)
        {
            float[] laneX = ConstGameplay.LevelGenerator.Lane_X_Positions;
            float targetX = (laneX != null && targetLaneIndex >= 0 && targetLaneIndex < laneX.Length) 
                ? laneX[targetLaneIndex] 
                : 0f;

            int actualLaneIndex = GetClosestLaneIndex(transform.position.x);
            bool laneMatches = (targetLaneIndex == actualLaneIndex) || 
                               (targetLaneIndex == LaneIndex) || 
                               (Mathf.Abs(targetX - transform.position.x) < 1.2f);

            if (!laneMatches) return false;

            float cellHeight = ConstGameplay.Obstacle.Furniture.Cell_Height;
            int furnitureRow = Mathf.RoundToInt(WorldY / cellHeight);
            int transformRow = Mathf.RoundToInt(transform.position.y / cellHeight);
            int targetRow = Mathf.RoundToInt(targetWorldY / cellHeight);

            if (targetRow == furnitureRow || targetRow == transformRow)
            {
                return true;
            }

            float yPos = transform.position.y;
            return Mathf.Abs(targetWorldY - yPos) < tolerance || Mathf.Abs(targetWorldY - WorldY) < tolerance;
        }

        private int GetClosestLaneIndex(float worldX)
        {
            float[] laneX = ConstGameplay.LevelGenerator.Lane_X_Positions;
            if (laneX == null || laneX.Length == 0) return LaneIndex;

            int closest = 0;
            float minDiff = Mathf.Abs(worldX - laneX[0]);
            for (int i = 1; i < laneX.Length; i++)
            {
                float diff = Mathf.Abs(worldX - laneX[i]);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    closest = i;
                }
            }
            return closest;
        }
    }
}
