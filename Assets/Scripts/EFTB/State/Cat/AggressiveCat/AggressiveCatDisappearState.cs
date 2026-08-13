using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatDisappearState : BaseState
    {
        private CatManager catManager;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float timer;
        private float yRotation;

        private AggressiveCatStateController stateController => (AggressiveCatStateController)StateController;

        public AggressiveCatDisappearState(BaseStateController stateController) : base(stateController)
        {
            catManager = GameContext.Instance.Get<CatManager>();
            if (catManager == null)
            {
                DebugLogHelper.LogError("CatManager is not found in GameContext.");
            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = 0f;

            var giAggressive = stateController.GiAggressiveCat;
            if (giAggressive != null)
            {
                giAggressive.SetSmashColliderActive(false);

                float offScreenX;
                if (giAggressive.CurrentSightDirection == CatSightDirection.Right)
                {
                    offScreenX = ConstGameplay.Cat.AggressiveCat.OFFSCREEN_X_LEFT_POSITION;
                    yRotation = ConstGameplay.Cat.AggressiveCat.CAT_LEFT_HAND_Y_ROTATION;
                }
                else
                {
                    offScreenX = ConstGameplay.Cat.AggressiveCat.OFFSCREEN_X_RIGHT_POSITION;
                    yRotation = ConstGameplay.Cat.AggressiveCat.CAT_RIGHT_HAND_Y_ROTATION;
                }

                startPosition = giAggressive.transform.position;
                targetPosition = new Vector3(offScreenX, startPosition.y, startPosition.z);
            }
            else
            {
                startPosition = stateController.GiCat != null ? stateController.GiCat.transform.position : Vector3.zero;
                targetPosition = startPosition;
                yRotation = 0f;
            }
        }

        public override void UpdateLogic(float deltaTime)
        {
            timer += deltaTime;
            float duration = stateController.Config != null ? stateController.Config.TimeToDisappear : 1f;
            float t = duration > 0f ? Mathf.Clamp01(timer / duration) : 1f;
            float progress = Mathf.SmoothStep(0f, 1f, t);

            var giAggressive = stateController.GiAggressiveCat;
            if (giAggressive != null)
            {
                Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, progress);
                giAggressive.SetHandPosition(currentPos);

                float initialZ = ConstGameplay.Cat.AggressiveCat.INITIAL_Z_ROTATION;
                float finalZ = ConstGameplay.Cat.AggressiveCat.FINAL_Z_ROTATION;
                float currentZ = Mathf.Lerp(finalZ, initialZ, progress);

                giAggressive.SetRotation(Quaternion.Euler(0f, yRotation, currentZ));
            }

            if (t >= 1f)
            {
                if (giAggressive != null)
                {
                    giAggressive.SetHandActive(false);
                }
                catManager?.ReturnCat(stateController.GiCat);
            }
        }
    }
}
