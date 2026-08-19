namespace JumboJumps.EFTB.Model.Obstacle
{
    public class FurnitureBlockData
    {
        public int LaneIndex { get; }
        public float YOffset { get; }
        public string PrefabName { get; }

        public FurnitureBlockData(int laneIndex, float yOffset, string prefabName)
        {
            LaneIndex = laneIndex;
            YOffset = yOffset;
            PrefabName = prefabName;
        }
    }
}
