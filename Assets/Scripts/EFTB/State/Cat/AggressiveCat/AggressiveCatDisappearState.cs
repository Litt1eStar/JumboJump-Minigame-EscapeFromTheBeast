using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatDisappearState : BaseState
    {
        private AggressiveCatStateController stateController;
        private CatManager catManager;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float timer;

        public AggressiveCatDisappearState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (AggressiveCatStateController)stateController;
            
            catManager = GameContext.Instance.Get<CatManager>();
            if(catManager == null)
            {
                DebugLogHelper.LogError("CatManager is not found in GameContext.");
            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = 0f;

            startPosition = stateController.GiCat.transform.position;
            float direction = (stateController.GiCat.CurrentSightDirection == CatSightDirection.Right)
                ? ConstGameplay.Cat.AggressiveCat.SLIDE_DIRECTION_LEFT_MULTIPLIER
                : ConstGameplay.Cat.AggressiveCat.SLIDE_DIRECTION_RIGHT_MULTIPLIER;
            targetPosition = new Vector3(
                startPosition.x + direction * stateController.Config.SlideDistance,
                startPosition.y,
                startPosition.z
            );
        }

        public override void UpdateLogic(float deltaTime)
        {
            timer += deltaTime;
            float duration = stateController.Config.TimeToDisappear;
            float t = duration > 0f ? Mathf.Clamp01(timer / duration) : ConstGameplay.Cat.AggressiveCat.TRANSITION_PROGRESS_COMPLETE;

            Vector3 currentPos = stateController.GiCat.transform.position;
            float newX = Mathf.Lerp(startPosition.x, targetPosition.x, t);
            stateController.GiCat.transform.position = new Vector3(newX, currentPos.y, currentPos.z);

            if (t >= ConstGameplay.Cat.AggressiveCat.TRANSITION_PROGRESS_COMPLETE)
            {
                catManager?.ReturnCat(stateController.GiCat);
                return;
            }
        }
    }
}
