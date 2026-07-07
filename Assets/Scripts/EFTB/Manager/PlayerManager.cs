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
        private PlayerStateController stateController;
        private PlayerVisualizer visualizer => stateController.Visualizer;
        public void Initialize()
        {
            Debug.Log($"{this.GetType().Name} was Initialize");
            stateController = new PlayerStateController();
            stateController.Initialize();
            stateController.StartStateController();

            PlayerTransform = SceneObjectContext.Instance.Get<GIPlayer>().transform;

            SetPlayerToMiddleLane();

            GameContext.Instance.Add(this);
        }

        private void SetPlayerToMiddleLane()
        {
            visualizer.SetPlayerOnMiddleLane();
        }

        public void Dispose()
        {
            stateController.Dispose();
            stateController = null;

            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            stateController.UpdateLogic(deltaTime);
        }
    }
}
