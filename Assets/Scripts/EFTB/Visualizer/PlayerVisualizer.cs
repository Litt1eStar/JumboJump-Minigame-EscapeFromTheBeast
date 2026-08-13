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
            if (giPlayer == null)
            {
                DebugLogHelper.LogError("GIPlayer not found in SceneObjectContext. PlayerVisualizer initialization failed.");
            }

        }

        public void SetPlayerOnMiddleLane()
        {
            giPlayer.SetInitialStartPosition();
        }

        public void Dispose()
        {
            giPlayer = null;
        }
        public void MoveForward(float deltaTime)
        {
            giPlayer.MoveForward(deltaTime);
        }

        public void SetPosition(Vector3 position)
        {
            giPlayer?.SetPosition(position);
        }

        public void SetXPosition(float x) 
        {
            giPlayer?.SetXPosition(x);
        }

        public void ShowPounceWarning(float duration, System.Action onComplete)
        {
            giPlayer?.ShowPounceWarning(duration, onComplete);
        }

        public void ShowPounceWarning(float duration, float shakeSpeed, float maxZAngle, System.Action onComplete)
        {
            giPlayer?.ShowPounceWarning(duration, shakeSpeed, maxZAngle, onComplete);
        }

        public void StopPounceWarning()
        {
            giPlayer?.StopPounceWarning();
        }

        public void SetMovingAnimation(bool isMoving)
        {
            giPlayer?.SetMovingAnimation(isMoving);
        }
    }
}
