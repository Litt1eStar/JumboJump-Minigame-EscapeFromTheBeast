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
        private Transform playerTransform;

        private float initialPlayerY;

        public int MaxCellsClimbed { get; private set; }
        public int TreatsCollected => (collectibleManager != null && ConstGameplay.Score.TREAT_POINT_VALUE > 0) 
            ? (collectibleManager.TotalCollectibleValue / ConstGameplay.Score.TREAT_POINT_VALUE) 
            : 0;
        public int DistanceScore => MaxCellsClimbed * ConstGameplay.Score.DISTANCE_POINT_PER_CELL;
        public int TreatScore => collectibleManager != null ? collectibleManager.TotalCollectibleValue : 0;
        public int TotalScore => DistanceScore + TreatScore;

        public ScoreData CurrentScoreData => new ScoreData(TotalScore, DistanceScore, TreatScore, MaxCellsClimbed, TreatsCollected);

        public void Initialize(Transform playerTransform)
        {
            if (playerTransform != null)
            {
                this.playerTransform = playerTransform;
                initialPlayerY = this.playerTransform.position.y;
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
            if (collectibleManager != null)
            {
                collectibleManager.EventTotalCollectibleValueChanged -= OnTreatsCollectedChanged;
                collectibleManager = null;
            }

            playerTransform = null;
            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if (playerTransform == null) return;

            float deltaY = playerTransform.position.y - initialPlayerY;
            float stepDistance = ConstGameplay.Player.STEP_DISTANCE_Y;
            int currentCells = Mathf.Max(0, Mathf.FloorToInt((deltaY + (stepDistance * 0.5f)) / stepDistance));

            if (currentCells > MaxCellsClimbed)
            {
                MaxCellsClimbed = currentCells;
                EventScoreChanged?.Invoke(CurrentScoreData);
            }
        }

        private void OnTreatsCollectedChanged(int totalValue)
        {
            EventScoreChanged?.Invoke(CurrentScoreData);
        }
    }
}
