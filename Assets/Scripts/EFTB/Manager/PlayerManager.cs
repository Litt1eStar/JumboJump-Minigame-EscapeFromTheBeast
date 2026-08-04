using System;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class PlayerManager
    {
        public Transform PlayerTransform { get; private set;}
        public event Action EventIdleLimitExceeded;
        public PlayerStateController StateController => stateController;
        private PlayerStateController stateController;
        private PlayerVisualizer visualizer => stateController.Visualizer;
        public void Initialize()
        {
            Debug.Log($"{this.GetType().Name} was Initialize");
            stateController = new PlayerStateController();
            stateController.Initialize();
            Subscribe();
            stateController.StartStateController();

            PlayerTransform = SceneObjectContext.Instance.Get<GIPlayer>().transform;

            SetPlayerToMiddleLane();
            
            float startX = stateController.LaneXPositions[stateController.CurrentLaneIndex];
            PlayerTransform.position = new Vector3(startX, PlayerTransform.position.y, PlayerTransform.position.z);

            GameContext.Instance.Add(this);
        }

        public void Subscribe()
        {
            stateController.EventIdleLimitExceeded += OnIdleLimitExceeded;
        }

        public void Unsubscribe()
        {
            stateController.EventIdleLimitExceeded -= OnIdleLimitExceeded;
        }

        private void SetPlayerToMiddleLane()
        {
            visualizer.SetPlayerOnMiddleLane();
        }

        public void Dispose()
        {
            if (stateController != null)
            {
                Unsubscribe();
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
