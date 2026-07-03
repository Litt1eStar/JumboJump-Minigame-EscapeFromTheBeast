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
            StateTransitionMap.Add(typeof(AggressiveCatAwakeState), null);
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = 0f;

            targetPosition = stateController.GiCat.transform.position;
            float direction = targetPosition.x < 0 ? -1f : 1f;
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
            float t = duration > 0f ? Mathf.Clamp01(timer / duration) : 1f;

            Vector3 currentPos = stateController.GiCat.transform.position;
            float newX = Mathf.Lerp(startPosition.x, targetPosition.x, t);
            stateController.GiCat.transform.position = new Vector3(newX, currentPos.y, currentPos.z);

            if (t >= 1f)
            {
                stateController.ChangeState(typeof(AggressiveCatAwakeState));
            }
        }
    }
}
