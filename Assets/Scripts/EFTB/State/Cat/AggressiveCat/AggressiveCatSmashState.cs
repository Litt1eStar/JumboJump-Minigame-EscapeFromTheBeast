using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using UnityEngine;

namespace JumboJumps.EFTB.State.Cat.AggressiveCat
{
    public class AggressiveCatSmashState : BaseState
    {
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private CatSightDirection currentSightDirection;
        private float timer;
        private float stayTimer;
        private bool hasSmashed;
        private float yRotation;

        private AggressiveCatStateController stateController => (AggressiveCatStateController)StateController;

        public AggressiveCatSmashState(BaseStateController stateController) : base(stateController)
        {
            StateTransitionMap.Add(typeof(AggressiveCatCatchState), null);
            StateTransitionMap.Add(typeof(AggressiveCatDisappearState), null);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            timer = 0f;
            stayTimer = 0f;
            hasSmashed = false;

            var giAggressive = stateController.GiAggressiveCat;
            if (giAggressive == null) return;

            giAggressive.SetHandActive(true);
            giAggressive.SetSmashColliderActive(false);
            currentSightDirection = giAggressive.CurrentSightDirection;

            CalculateTargetPositions(giAggressive);

            float initialZ = ConstGameplay.Cat.AggressiveCat.INITIAL_Z_ROTATION;
            Quaternion initialRotation = Quaternion.Euler(0f, yRotation, initialZ);

            giAggressive.SetHandPosition(startPosition);
            giAggressive.SetRotation(initialRotation);
        }

        /// <summary>
        /// Calculates offscreen start position and smash cell target position based on player's lane and sight direction.
        /// </summary>
        private void CalculateTargetPositions(GIAggressiveCat giAggressive)
        {
            Vector3 cachedPos;
            if (giAggressive.TargetSmashPosition.HasValue)
            {
                cachedPos = giAggressive.TargetSmashPosition.Value;
            }
            else
            {
                float[] lanePositions = ConstGameplay.LevelGenerator.LANE_X_POSITIONS;
                float defaultTargetX = (lanePositions != null && lanePositions.Length > 1) ? lanePositions[1] : 0f;
                cachedPos = new Vector3(defaultTargetX, giAggressive.transform.position.y, giAggressive.transform.position.z);
            }

            int playerLaneIndex = GetPlayerLaneIndex(cachedPos.x);
            float targetX;
            float offScreenX;

            if (currentSightDirection == CatSightDirection.Right)
            {
                float[] leftPositions = ConstGameplay.Cat.AggressiveCat.SPAWN_X_LEFT_POSITION;
                if (leftPositions != null && leftPositions.Length > 0)
                {
                    int spawnIndex = (playerLaneIndex <= 0) ? 0 : 1;
                    spawnIndex = Mathf.Clamp(spawnIndex, 0, leftPositions.Length - 1);
                    targetX = leftPositions[spawnIndex];
                }
                else
                {
                    targetX = cachedPos.x;
                }

                offScreenX = ConstGameplay.Cat.AggressiveCat.OFFSCREEN_X_LEFT_POSITION;
                yRotation = ConstGameplay.Cat.AggressiveCat.CAT_LEFT_HAND_Y_ROTATION;
            }
            else
            {
                float[] rightPositions = ConstGameplay.Cat.AggressiveCat.SPAWN_X_RIGHT_POSITION;
                if (rightPositions != null && rightPositions.Length > 0)
                {
                    int spawnIndex = (playerLaneIndex >= 2) ? 1 : 0;
                    spawnIndex = Mathf.Clamp(spawnIndex, 0, rightPositions.Length - 1);
                    targetX = rightPositions[spawnIndex];
                }
                else
                {
                    targetX = cachedPos.x;
                }

                offScreenX = ConstGameplay.Cat.AggressiveCat.OFFSCREEN_X_RIGHT_POSITION;
                yRotation = ConstGameplay.Cat.AggressiveCat.CAT_RIGHT_HAND_Y_ROTATION;
            }

            startPosition = new Vector3(offScreenX, cachedPos.y, giAggressive.transform.position.z);
            targetPosition = new Vector3(targetX, cachedPos.y, startPosition.z);
        }

        /// <summary>
        /// Determines the closest lane index (0 = Lane 1, 1 = Lane 2, 2 = Lane 3) for the given X coordinate.
        /// </summary>
        private int GetPlayerLaneIndex(float playerX)
        {
            float[] lanePositions = ConstGameplay.LevelGenerator.LANE_X_POSITIONS;
            if (lanePositions == null || lanePositions.Length == 0) return 1;

            int closestLane = 0;
            float minDistance = Mathf.Abs(playerX - lanePositions[0]);

            for (int i = 1; i < lanePositions.Length; i++)
            {
                float distance = Mathf.Abs(playerX - lanePositions[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestLane = i;
                }
            }

            return closestLane;
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
                float duration = stateController.Config != null ? stateController.Config.TimeToSmash : 0.3f;
                float t = duration > 0f ? Mathf.Clamp01(timer / duration) : 1f;

                var config = stateController.Config;
                float moveInThreshold = (config != null) ? config.SmashMoveInPercentage : ConstGameplay.Cat.AggressiveCat.CAT_SMASH_MOVE_IN_PERCENTAGE;
                float waitThreshold = (config != null) ? config.SmashWaitPercentage : ConstGameplay.Cat.AggressiveCat.CAT_SMASH_WAIT_PERCENTAGE;

                AnimationCurve moveCurve = config != null ? config.SmashMovementCurve : null;
                AnimationCurve rotCurve = config != null ? config.SmashRotationCurve : null;

                float initialZ = ConstGameplay.Cat.AggressiveCat.INITIAL_Z_ROTATION;
                float finalZ = ConstGameplay.Cat.AggressiveCat.FINAL_Z_ROTATION;

                if (t < moveInThreshold)
                {
                    // Phase 1: Move from startPosition to targetPosition with INITIAL_Z_ROTATION
                    float progress = t / moveInThreshold;
                    float moveProgress = (moveCurve != null && moveCurve.length > 0) ? moveCurve.Evaluate(progress) : Mathf.SmoothStep(0f, 1f, progress);

                    Vector3 currentHandPos = Vector3.Lerp(startPosition, targetPosition, moveProgress);
                    giAggressive.SetHandPosition(currentHandPos);
                    giAggressive.SetRotation(Quaternion.Euler(0f, yRotation, initialZ));
                }
                else if (t < waitThreshold)
                {
                    // Phase 2: Hold at targetPosition with INITIAL_Z_ROTATION
                    giAggressive.SetHandPosition(targetPosition);
                    giAggressive.SetRotation(Quaternion.Euler(0f, yRotation, initialZ));
                }
                else
                {
                    // Phase 3: Hit / Swing rotation from INITIAL_Z_ROTATION to FINAL_Z_ROTATION
                    float progress = (t - waitThreshold) / (1f - waitThreshold);
                    float rotProgress = (rotCurve != null && rotCurve.length > 0) ? rotCurve.Evaluate(progress) : Mathf.SmoothStep(0f, 1f, progress);

                    float currentZ = Mathf.Lerp(initialZ, finalZ, rotProgress);

                    giAggressive.SetHandPosition(targetPosition);
                    giAggressive.SetRotation(Quaternion.Euler(0f, yRotation, currentZ));
                }

                if (t >= 1f)
                {
                    hasSmashed = true;
                    giAggressive.SetHandPosition(targetPosition);
                    giAggressive.SetRotation(Quaternion.Euler(0f, yRotation, finalZ));
                    giAggressive.SetSmashColliderActive(true);
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
                float stayDuration = stateController.Config != null ? stateController.Config.SmashStayDuration : 3f;
                if (stayTimer >= stayDuration)
                {
                    stateController.ChangeState(typeof(AggressiveCatDisappearState));
                }
            }
        }
    }
}
