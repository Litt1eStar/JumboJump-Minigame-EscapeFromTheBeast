using System;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GICollectible : MonoBehaviour
    {
        public event Action<GICollectible> EventCollected;
        public event Action<GICollectible> EventRecycleRequested;

        public int PointValue { get; private set; } = 100;
        public int LaneIndex { get; private set; }
        public float WorldY { get; private set; }

        private bool isCollected = false;

        public void Initialize(int pointValue = 100, int laneIndex = 0, float worldY = 0f)
        {
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
            EventCollected?.Invoke(this);

            GISegment parentSegment = GetComponentInParent<GISegment>();
            if (parentSegment != null)
            {
                parentSegment.DeregisterSpawnedObject(gameObject);
            }

            EventRecycleRequested?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isCollected) return;

            if (collision.GetComponent<GIPlayer>() != null)
            {
                Collect();
            }
        }
    }
}
