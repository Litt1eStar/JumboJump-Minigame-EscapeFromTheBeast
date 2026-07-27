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
        public event Action<Vector3> EventPlayerMoved;

        public Transform PlayerTransform { get; private set; }
        private PlayerStateController stateController;
        private PlayerVisualizer visualizer => stateController.Visualizer;

        public void Initialize()
        {
            Debug.Log($"{this.GetType().Name} was Initialize");
            stateController = new PlayerStateController();
            stateController.Initialize();
            stateController.StartStateController();

            stateController.EventPlayerMoved += OnPlayerMoved;

            PlayerTransform = SceneObjectContext.Instance.Get<GIPlayer>().transform;

            SetPlayerToMiddleLane();
            
            float startX = stateController.LaneXPositions[stateController.CurrentLaneIndex];
            PlayerTransform.position = new Vector3(startX, PlayerTransform.position.y, PlayerTransform.position.z);

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
                stateController.EventPlayerMoved -= OnPlayerMoved;
                stateController.Dispose();
                stateController = null;
            }

            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController.UpdateLogic(deltaTime);
        }
    }
}
