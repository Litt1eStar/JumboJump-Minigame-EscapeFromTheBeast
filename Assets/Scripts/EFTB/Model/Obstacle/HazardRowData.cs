using UnityEngine;

namespace JumboJumps.EFTB.Model.Obstacle
{
    public enum HazardDirectionEnum
    {
        LeftToRight = 1,
        RightToLeft = -1
    }

    public class HazardRowData
    {
        public float RowWorldY { get; }
        public HazardDirectionEnum Direction { get; }
        public float Speed { get; }
        public float SpawnInterval { get; set; }
        public float NextSpawnTimer { get; set; }

        public HazardRowData(float rowWorldY, HazardDirectionEnum direction, float speed, float spawnInterval, bool immediateFirstSpawn = true)
        {
            RowWorldY = rowWorldY;
            Direction = direction;
            Speed = speed;
            SpawnInterval = spawnInterval;
            NextSpawnTimer = immediateFirstSpawn ? spawnInterval : 0f;
        }
    }
}
