using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatSmashState : BaseState
    {
        private AggressiveCatStateController stateController;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private CatSightDirection currentSightDirection;
        private float timer;
        private float stayTimer;
        private bool hasSmashed;

        public AggressiveCatSmashState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(AggressiveCatCatchState), null);
            StateTransitionMap.Add(typeof(AggressiveCatDisappearState), null);
            this.stateController = (AggressiveCatStateController)stateController;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = 0f;
            stayTimer = 0f;
            hasSmashed = false;

            var giAggressive = stateController.GiAggressiveCat;
            if (giAggressive != null)
            {
                giAggressive.SetHandActive(true);

                currentSightDirection = giAggressive.CurrentSightDirection;

                if (giAggressive.TargetSmashPosition.HasValue)
                {
                    Vector3 cachedPos = giAggressive.TargetSmashPosition.Value;
                    startPosition = new Vector3(giAggressive.transform.position.x, cachedPos.y, giAggressive.transform.position.z);
                    targetPosition = new Vector3(cachedPos.x, cachedPos.y, startPosition.z);
                }
                else
                {
                    InitializeSmashParameters(giAggressive);
                    return;
                }

                float yRotation = (currentSightDirection == CatSightDirection.Right) 
                    ? ConstGameplay.Cat.AggressiveCat.CAT_LEFT_HAND_Y_ROTATION 
                    : ConstGameplay.Cat.AggressiveCat.CAT_RIGHT_HAND_Y_ROTATION;

                Quaternion catHandRotation = Quaternion.Euler(0f, yRotation, 0f);

                giAggressive.SetHandRotation(catHandRotation);
                giAggressive.SetHandPosition(startPosition);
            }
        }

        private void InitializeSmashParameters(GIAggressiveCat giAggressive)
        {
            giAggressive.SetHandActive(true);

            startPosition = giAggressive.transform.position;
            currentSightDirection = giAggressive.CurrentSightDirection;
            
            float[] lanePositions = ConstGameplay.LevelGenerator.LANE_X_POSITIONS;
            float targetX = (currentSightDirection == CatSightDirection.Right)
                ? (lanePositions != null && lanePositions.Length > 0 ? lanePositions[0] : -2.0f)
                : (lanePositions != null && lanePositions.Length > 1 ? lanePositions[lanePositions.Length - 1] : 2.0f);

            float rotation = (currentSightDirection == CatSightDirection.Right) 
                ? ConstGameplay.Cat.AggressiveCat.CAT_LEFT_HAND_Y_ROTATION 
                : ConstGameplay.Cat.AggressiveCat.CAT_RIGHT_HAND_Y_ROTATION;

            targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);
            
            Quaternion catHandRotation = Quaternion.Euler(0f, rotation, 0f);
            giAggressive.SetHandRotation(catHandRotation);
            giAggressive.SetHandPosition(startPosition);
        }

        public override void UpdateLogic(float deltaTime)
        {
            var giAggressive = stateController.GiAggressiveCat;
            if (giAggressive == null) return;

            if (!hasSmashed)
            {
                timer += deltaTime;
                float duration = stateController.Config.TimeToSmash;
                float t = duration > 0f ? Mathf.Clamp01(timer / duration) : 1f;

                Vector3 currentHandPos = Vector3.Lerp(startPosition, targetPosition, t);
                giAggressive.SetHandPosition(currentHandPos);

                if (t >= 1f)
                {
                    hasSmashed = true;
                    giAggressive.SetHandPosition(targetPosition);
                }
            }

            if (hasSmashed)
            {
                if (giAggressive.CheckPlayerCollision())
                {
                    stateController.ChangeState(typeof(AggressiveCatCatchState));
                    return;
                }

                stayTimer += deltaTime;
                if (stayTimer >= stateController.Config.SmashStayDuration)
                {
                    stateController.ChangeState(typeof(AggressiveCatDisappearState));
                }
            }
        }
    }
}
