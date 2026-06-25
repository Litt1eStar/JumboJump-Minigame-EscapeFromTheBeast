using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Visualizer
{
    public class PlayerVisualizer
    {
        private GIPlayer giPlayer;
        public Vector3 PlayerPosition => giPlayer != null ? giPlayer.PlayerPosition : Vector3.zero;

        public void Initialize()
        {
            giPlayer = SceneObjectContext.Instance.Get<GIPlayer>();
            if(giPlayer == null)
            {
                DebugLogHelper.LogError("GIPlayer not found in SceneObjectContext. PlayerVisualizer initialization failed.");
            }

            OnInitialize();
        }

        public void OnInitialize()
        {
            giPlayer.SetPlayerOnMiddleLane();
        }

        public void Dispose()
        {
            giPlayer = null;
        }
        public void MoveForward(float deltaTime)
        {
            giPlayer.MoveForward(deltaTime);
        }

        public void SetXPosition(float x) 
        {
            giPlayer.SetXPosition(x);
        }
    }
}
