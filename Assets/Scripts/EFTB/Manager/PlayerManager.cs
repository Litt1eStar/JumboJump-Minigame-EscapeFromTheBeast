using Assets.Scripts.EFTB.State.Player;
using Assets.Scripts.EFTB.Utilities;
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

        GameContext.Instance.Add(this);
    }

    public void Dispose()
    {
        stateController = null;
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
