using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class ScoreManager
    {
        public event Action<ScoreData> EventScoreChanged;

        private CollectibleManager collectibleManager;
        private PlayerManager playerManager;

        public int MaxCellsClimbed { get; private set; }
        public int TreatsCollected { get; private set; }
        public int DistanceScore => MaxCellsClimbed * ConstGameplay.Score.DISTANCE_POINT_PER_CELL;
        public int TreatScore => TreatsCollected * ConstGameplay.Score.TREAT_POINT_VALUE;
        public int TotalScore => DistanceScore + TreatScore;

        public ScoreData CurrentScoreData => new ScoreData(TotalScore, DistanceScore, TreatScore, MaxCellsClimbed, TreatsCollected);

        public void Initialize(PlayerManager playerManager)
        {
            this.playerManager = playerManager;
            if (this.playerManager != null)
            {
                this.playerManager.EventPlayerMoved += OnPlayerMoved;
            }

            collectibleManager = GameContext.Instance.Get<CollectibleManager>();
            if (collectibleManager != null)
            {
                collectibleManager.EventTotalCollectibleValueChanged += OnTreatsCollectedChanged;
            }

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            if (playerManager != null)
            {
                playerManager.EventPlayerMoved -= OnPlayerMoved;
                playerManager = null;
            }

            if (collectibleManager != null)
            {
                collectibleManager.EventTotalCollectibleValueChanged -= OnTreatsCollectedChanged;
                collectibleManager = null;
            }

            GameContext.Instance.Remove(this);
        }

        private void OnPlayerMoved(Vector3 position)
        {
            if (playerManager == null) return;

            float deltaY = position.y - playerManager.InitialPlayerY;
            float stepDistance = ConstGameplay.Obstacle.Furniture.CELL_HEIGHT;
            int currentCells = Mathf.Max(0, Mathf.FloorToInt((deltaY + (stepDistance * 0.5f)) / stepDistance));

            if (currentCells > MaxCellsClimbed)
            {
                MaxCellsClimbed = currentCells;
                EventScoreChanged?.Invoke(CurrentScoreData);
            }
        }

        public void ResetScore()
        {
            MaxCellsClimbed = 0;
            TreatsCollected = 0;
            EventScoreChanged?.Invoke(CurrentScoreData);
        }

        private void OnTreatsCollectedChanged(int totalValue)
        {
            TreatsCollected = totalValue;
            EventScoreChanged?.Invoke(CurrentScoreData);
        }
    }
}
