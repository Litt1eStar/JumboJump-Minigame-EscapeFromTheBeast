using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatAppearState : BaseState
    {
        private AggressiveCatStateController stateController;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float timer;

        public AggressiveCatAppearState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(AggressiveCatSmashState), null);
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = 0f;

            stateController.GiAggressiveCat?.SetHandActive(false);

            targetPosition = stateController.GiCat.transform.position;
 
            float direction = (stateController.GiCat.CurrentSightDirection == CatSightDirection.Right)
                ? ConstGameplay.Cat.AggressiveCat.SlideDirectionLeftMultiplier
                : ConstGameplay.Cat.AggressiveCat.SlideDirectionRightMultiplier;
            
            startPosition = new Vector3(
                targetPosition.x + direction * stateController.Config.SlideDistance,
                targetPosition.y,
                targetPosition.z
            );

            stateController.GiCat.transform.position = startPosition;
        }

        public override void UpdateLogic(float deltaTime)
        {
            timer += deltaTime;
            float duration = stateController.Config.TimeToAppear;
            float t = duration > 0f ? Mathf.Clamp01(timer / duration) : ConstGameplay.Cat.AggressiveCat.TransitionProgressComplete;

            Vector3 currentPos = stateController.GiCat.transform.position;
            float newX;

            float sneakInDurationPercentage = ConstGameplay.Cat.AggressiveCat.CatAppearSneakInDurationPercentage;
            float stayDurationPercentage = ConstGameplay.Cat.AggressiveCat.CatAppearStayDurationPercentage;

            if (t < sneakInDurationPercentage)
            {
                // Sneak In (0% to SneakInDurationPercentage duration) - Fast
                float progress = t / ConstGameplay.Cat.AggressiveCat.CatAppearSneakInDurationPercentage;
                newX = Mathf.Lerp(startPosition.x, targetPosition.x, progress);
            }
            else if (t < stayDurationPercentage)
            {
                // Stay (SneakInDurationPercentage to StayDurationPercentage duration) - Hold at target position
                newX = targetPosition.x;
            }
            else
            {
                // Sneak Out (StayDurationPercentage to 100% duration)
                float progress = (t - stayDurationPercentage) 
                    / (ConstGameplay.Cat.AggressiveCat.TransitionProgressComplete - stayDurationPercentage);
                newX = Mathf.Lerp(targetPosition.x, startPosition.x, progress);
            }

            stateController.GiCat.transform.position = new Vector3(newX, currentPos.y, currentPos.z);

            if (t >= ConstGameplay.Cat.AggressiveCat.TransitionProgressComplete)
            {
                stateController.ChangeState(typeof(AggressiveCatSmashState));
            }
        }
    }
}
