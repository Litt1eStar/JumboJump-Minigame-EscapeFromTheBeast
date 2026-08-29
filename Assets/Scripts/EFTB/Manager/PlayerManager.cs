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
        public PlayerVisualizer Visualizer => stateController.Visualizer;
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
            Visualizer.SetXPosition(startX);

            GameContext.Instance.Add(this);
        }

        private void SetPlayerToMiddleLane()
        {
            Visualizer.SetPlayerOnMiddleLane();
        }

        public void ResetPlayer()
        {
            if (stateController != null)
            {
                stateController.CurrentLaneIndex = JumboJumps.EFTB.Constant.Gameplay.ConstGameplay.LevelGenerator.INITIAL_LANE_INDEX;
                stateController.IsStepUpRequested = false;
                stateController.ResetIdleTimer();
                stateController.ResetStateController(typeof(PlayerIdleState));
            }

            SetPlayerToMiddleLane();

            if (stateController != null)
            {
                float startX = stateController.LaneXPositions[stateController.CurrentLaneIndex];
                Visualizer?.SetXPosition(startX);
            }
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
            Visualizer?.ShowPounceWarning(duration, onComplete);
        }

        public void TriggerPounceWarning(float duration, float shakeSpeed, float maxZAngle, System.Action onComplete)
        {
            Visualizer?.ShowPounceWarning(duration, shakeSpeed, maxZAngle, onComplete);
        }
    }
}
