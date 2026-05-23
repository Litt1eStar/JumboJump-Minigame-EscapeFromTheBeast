using Assets.Scripts.EFTB.State.Player;
using UnityEngine;

public class PlayerManager
{
    private PlayerStateController stateController;

    public void Initialize()
    {
        stateController = new PlayerStateController();
        stateController.Initialize();
    }

    public void Dispose()
    {
        stateController.Dispose();
        stateController = null;
    }
}
