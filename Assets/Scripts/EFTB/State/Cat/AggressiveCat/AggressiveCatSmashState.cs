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

                startPosition = giAggressive.transform.position;
                currentSightDirection = giAggressive.CurrentSightDirection;

                // Always target the second lane (middle lane at index 1)
                float[] lanePositions = ConstGameplay.LevelGenerator.LaneXPositions;
                float targetX = (lanePositions != null && lanePositions.Length > 1)
                    ? lanePositions[1]
                    : 0f;

                float rotation = (currentSightDirection == CatSightDirection.Right) 
                    ? ConstGameplay.Cat.AggressiveCat.CatLeftHandYRotation 
                    : ConstGameplay.Cat.AggressiveCat.CatRightHandYRotation;

                targetPosition = new Vector3(targetX, startPosition.y, startPosition.z);
                
                Quaternion catHandRotation = Quaternion.Euler(0f, rotation, 0f);
                giAggressive.SetHandRotation(catHandRotation);
                giAggressive.SetHandPosition(startPosition);
            }
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

                // Wait for the specified stay duration after the smash
                stayTimer += deltaTime;
                if (stayTimer >= stateController.Config.SmashStayDuration)
                {
                    stateController.ChangeState(typeof(AggressiveCatDisappearState));
                }
            }
        }
    }
}
