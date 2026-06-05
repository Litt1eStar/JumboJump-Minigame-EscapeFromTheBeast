using JumboJumps.EFTB.State.Player;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class PlayerManager
    {
        public Transform PlayerTransform { get; private set;}
        private PlayerStateController stateController;
        public void Initialize()
        {
            Debug.Log($"{this.GetType().Name} was Initialize");
            stateController = new PlayerStateController();
            stateController.Initialize();
            stateController.StartStateController();

            PlayerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

            GameContext.Instance.Add(this);
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
