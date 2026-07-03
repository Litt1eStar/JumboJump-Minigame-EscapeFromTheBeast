using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatDissappearState : BaseState
    {
        private AggressiveCatStateController stateController;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float timer;

        public AggressiveCatDissappearState(BaseStateController stateController) : base(stateController)
        {
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = 0f;

            startPosition = stateController.GiCat.transform.position;
            float direction = startPosition.x < 0 ? -1f : 1f;
            targetPosition = new Vector3(
                startPosition.x + direction * stateController.Config.SlideDistance,
                startPosition.y,
                startPosition.z
            );
        }

        public override void UpdateLogic(float deltaTime)
        {
            timer += deltaTime;
            float duration = stateController.Config.TimeToDissappear;
            float t = duration > 0f ? Mathf.Clamp01(timer / duration) : 1f;

            Vector3 currentPos = stateController.GiCat.transform.position;
            float newX = Mathf.Lerp(startPosition.x, targetPosition.x, t);
            stateController.GiCat.transform.position = new Vector3(newX, currentPos.y, currentPos.z);

            if (t >= 1f)
            {
                GameObject giCatGo = stateController.GiCat.gameObject;

                GISegment giSegment = giCatGo.transform.parent?.GetComponent<GISegment>();
                giSegment?.DeregisterSpawnedObject(giCatGo);

                SceneObjectContext.Instance.Deregister(stateController.GiCat);
                CatManager catManager = GameContext.Instance.Get<CatManager>();
                catManager?.DeregisterCat(stateController.GiCat);

                ObjectPoolManager poolManager = GameContext.Instance.Get<ObjectPoolManager>();
                poolManager?.Recycle(giCatGo);

                return;
            }
        }
    }
}
