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

            var giAggressive = stateController.GiAggressiveCat;
            if (giAggressive != null)
            {
                // Slide hand from its current lane position back to the body position
                startPosition = giAggressive.CatHand != null ? giAggressive.CatHand.position : giAggressive.transform.position;
                targetPosition = giAggressive.transform.position;
            }
            else
            {
                startPosition = stateController.GiCat.transform.position;
                targetPosition = startPosition;
            }
        }

        public override void UpdateLogic(float deltaTime)
        {
            timer += deltaTime;
            float duration = stateController.Config.TimeToDisappear;
            float t = duration > 0f ? Mathf.Clamp01(timer / duration) : ConstGameplay.Cat.AggressiveCat.TransitionProgressComplete;

            var giAggressive = stateController.GiAggressiveCat;
            if (giAggressive != null && giAggressive.CatHand != null)
            {
                giAggressive.SetHandPosition(Vector3.Lerp(startPosition, targetPosition, t));
            }

            if (t >= 1f)
            {
                if (giAggressive != null)
                {
                    giAggressive.SetHandActive(false);
                }
                catManager?.ReturnCat(stateController.GiCat);
                return;
            }
        }
    }
}
