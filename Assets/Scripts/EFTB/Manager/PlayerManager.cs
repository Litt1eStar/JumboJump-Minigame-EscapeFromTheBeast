using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class PlayerManager
    {
        public Transform PlayerTransform { get; private set;}
        public float InitialPlayerY { get; private set; }

        public PlayerStateController StateController => stateController;
        public event Action EventIdleLimitExceeded;
        public event Action<Vector3> EventPlayerMoved;
        private PlayerStateController stateController;
        private PlayerVisualizer visualizer => stateController.Visualizer;

        public void Initialize()
        {
            Debug.Log($"{this.GetType().Name} was Initialize");
            stateController = new PlayerStateController();
            stateController.Initialize();
            stateController.EventIdleLimitExceeded += OnIdleLimitExceeded;
            stateController.StartStateController();

            stateController.EventPlayerMoved += OnPlayerMoved;

            GIPlayer giPlayer = SceneObjectContext.Instance.Get<GIPlayer>();
            if (giPlayer != null)
            {
                PlayerTransform = giPlayer.transform;
                InitialPlayerY = PlayerTransform.position.y;
            }
            else
            {
                DebugLogHelper.LogError($"[{GetType().Name}] GIPlayer not found in SceneObjectContext");
            }

            SetPlayerToMiddleLane();
            
            float startX = stateController.LaneXPositions[stateController.CurrentLaneIndex];
            if (PlayerTransform != null)
            {
                PlayerTransform.position = new Vector3(startX, PlayerTransform.position.y, PlayerTransform.position.z);
            }

            GameContext.Instance.Add(this);
        }

        private void OnPlayerMoved(Vector3 position)
        {
            EventPlayerMoved?.Invoke(position);
        }

        private void SetPlayerToMiddleLane()
        {
            visualizer.SetPlayerOnMiddleLane();
        }

        public void Dispose()
        {
            if (stateController != null)
            {
                stateController.EventIdleLimitExceeded -= OnIdleLimitExceeded;
                stateController.Dispose();
                stateController = null;
            }

            GameContext.Instance.Remove(this);
        }

        private void OnIdleLimitExceeded()
        {
            EventIdleLimitExceeded?.Invoke();
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController.UpdateLogic(deltaTime);
        }

        public void TriggerPounceWarning(float duration, System.Action onComplete)
        {
            visualizer?.ShowPounceWarning(duration, onComplete);
        }
    }
}
