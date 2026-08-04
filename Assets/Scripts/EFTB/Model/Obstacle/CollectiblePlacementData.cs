namespace JumboJumps.EFTB.Model.Obstacle
{
    public class CollectiblePlacementData
    {
        public int LaneIndex { get; }
        public float YOffset { get; }

        public CollectiblePlacementData(int laneIndex, float yOffset)
        {
            LaneIndex = laneIndex;
            YOffset = yOffset;
        }
    }
}
