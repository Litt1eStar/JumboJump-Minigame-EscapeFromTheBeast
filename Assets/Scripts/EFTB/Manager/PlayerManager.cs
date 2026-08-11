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
        public PlayerStateController StateController => stateController;
        public event System.Action EventIdleLimitExceeded;
        private PlayerStateController stateController;
        private PlayerVisualizer visualizer => stateController.Visualizer;
        public void Initialize()
        {
            Debug.Log($"{this.GetType().Name} was Initialize");
            stateController = new PlayerStateController();
            stateController.Initialize();
            stateController.EventIdleLimitExceeded += OnIdleLimitExceeded;
            stateController.StartStateController();

            PlayerTransform = SceneObjectContext.Instance.Get<GIPlayer>().transform;
            SetPlayerToMiddleLane();
            
            float startX = stateController.LaneXPositions[stateController.CurrentLaneIndex];
            visualizer.SetXPosition(startX);

            GameContext.Instance.Add(this);
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
