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

        public static float SnapToCellCenter(float rawY, float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT)
        {
            int rowIdx = Mathf.RoundToInt((rawY - 1.0f) / cellHeight);
            return (rowIdx * cellHeight) + 1.0f;
        }

        public void Initialize(int laneIndex, float worldY)
        {
            levelGeneratorManager = GameContext.Instance?.Get<LevelGeneratorManager>();

            LaneIndex = laneIndex;
            WorldY = SnapToCellCenter(worldY);
            transform.position = new Vector3(transform.position.x, WorldY, transform.position.z);

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
            if (levelGeneratorManager == null)
            {
                levelGeneratorManager = GameContext.Instance?.Get<LevelGeneratorManager>();
            }

            WorldY = SnapToCellCenter(transform.position.y);
            transform.position = new Vector3(transform.position.x, WorldY, transform.position.z);
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
            float[] laneX = ConstGameplay.LevelGenerator.LANE_X_POSITIONS;
            float targetX = (laneX != null && targetLaneIndex >= 0 && targetLaneIndex < laneX.Length) 
                ? laneX[targetLaneIndex] 
                : 0f;

            int actualLaneIndex = GetClosestLaneIndex(transform.position.x);
            bool laneMatches = (targetLaneIndex == actualLaneIndex) || 
                               (targetLaneIndex == LaneIndex) || 
                               (Mathf.Abs(targetX - transform.position.x) < 1.2f);

            if (!laneMatches) return false;

            float cellHeight = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int furnitureRow = Mathf.FloorToInt(WorldY / cellHeight);
            int transformRow = Mathf.FloorToInt(transform.position.y / cellHeight);
            int targetRow = Mathf.FloorToInt(targetWorldY / cellHeight);

            if (targetRow == furnitureRow || targetRow == transformRow)
            {
                return true;
            }

            float yPos = transform.position.y;
            return Mathf.Abs(targetWorldY - yPos) < tolerance || Mathf.Abs(targetWorldY - WorldY) < tolerance;
        }

        private int GetClosestLaneIndex(float worldX)
        {
            float[] laneX = ConstGameplay.LevelGenerator.LANE_X_POSITIONS;
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
