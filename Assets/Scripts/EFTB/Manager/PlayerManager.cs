using Assets.Scripts.EFTB.State.Player;
using UnityEngine;

public class PlayerManager
{
    private PlayerStateController stateController;

    public void Initialize()
    {
        Debug.Log($"{this.GetType().Name} was Initialize");
        stateController = new PlayerStateController();
        stateController.Initialize();
        stateController.StartStateController();
    }

    public void UpdateLogic(float deltaTime)
    {
        stateController.UpdateLogic(deltaTime);
    }

    public void Dispose()
    {
        stateController.Dispose();
        stateController = null;
    }
}
