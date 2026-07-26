using JumboJumps.EFTB.GameData;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GICollectible : MonoBehaviour
    {
        public int PointValue { get; private set; } = 100;
        public int LaneIndex { get; private set; }
        public float WorldY { get; private set; }

        private bool isCollected = false;
        private ObjectPoolManager poolManager;
        private CollectibleManager collectibleManager;

        public void Initialize(int pointValue = 100, int laneIndex = 0, float worldY = 0f)
        {
            poolManager = GameContext.Instance?.Get<ObjectPoolManager>();
            collectibleManager = GameContext.Instance?.Get<CollectibleManager>();

            PointValue = pointValue;
            LaneIndex = laneIndex;
            WorldY = worldY;
            isCollected = false;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
            }
        }

        public void Collect()
        {
            if (isCollected) return;

            isCollected = true;
            collectibleManager?.AddValue(PointValue);

            GISegment parentSegment = GetComponentInParent<GISegment>();
            if (parentSegment != null)
            {
                parentSegment.DeregisterSpawnedObject(gameObject);
            }

            RecycleSelf();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isCollected) return;

            if (collision.GetComponent<GIPlayer>() != null)
            {
                Collect();
            }
        }

        private void RecycleSelf()
        {
            poolManager?.Recycle(gameObject);
        }
    }
}
