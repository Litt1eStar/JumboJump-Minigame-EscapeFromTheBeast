namespace JumboJumps.EFTB.Model
{
    public struct ScoreData
    {
        public int TotalScore { get; set; }
        public int DistanceScore { get; set; }
        public int TreatScore { get; set; }
        public int MaxCellsClimbed { get; set; }
        public int TreatsCollected { get; set; }

        public ScoreData(int totalScore, int distanceScore, int treatScore, int maxCellsClimbed, int treatsCollected)
        {
            TotalScore = totalScore;
            DistanceScore = distanceScore;
            TreatScore = treatScore;
            MaxCellsClimbed = maxCellsClimbed;
            TreatsCollected = treatsCollected;
        }
    }
}
