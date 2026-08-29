using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.Visualizer;
using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.State.Player
{
    public class PlayerStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(PlayerIdleState);
        public PlayerVisualizer Visualizer { get; private set; }
        public Input2DManager Input2DManager { get; private set; }
        public int CurrentLaneIndex { get; set; }
        private float[] laneXPositions;
        public float[] LaneXPositions
        {
            get
            {
                if (laneXPositions == null)
                {
                    LevelGeneratorManager levelGeneratorManager = GameContext.Instance.Get<LevelGeneratorManager>();
                    if (levelGeneratorManager != null)
                    {
                        laneXPositions = levelGeneratorManager.LaneXPositions;
                    }
                    else
                    {
                        laneXPositions = ConstGameplay.LevelGenerator.LANE_X_POSITIONS;
                    }
                }
                return laneXPositions;
            }
        }
        public SwipeDirectionEnum LastSwipeDirection { get; set; }
        public bool IsStepUpRequested { get; set; }
        public float IdleTimer { get; set; }
        public event Action EventIdleLimitExceeded;

        public void ResetIdleTimer()
        {
            IdleTimer = 0f;
        }

        public void InvokeIdleLimitExceeded()
        {
            EventIdleLimitExceeded?.Invoke();
        }

        public PlayerStateController()
        {
            Visualizer = new PlayerVisualizer();
            Visualizer.Initialize();

            CurrentLaneIndex = ConstGameplay.LevelGenerator.INITIAL_LANE_INDEX;

            Input2DManager = SceneObjectContext.Instance.Get<Input2DManager>(); 
            if (Input2DManager == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] {nameof(PlayerStateController)}| Failed to get {typeof(Input2DManager).AssemblyQualifiedName} from SceneObjectContext");
            }


            States = new Dictionary<Type, BaseState>
            {
                { typeof(PlayerIdleState), new PlayerIdleState(this) },
                { typeof(PlayerMovingState), new PlayerMovingState(this) },
                { typeof(PlayerSwitchLaneState), new PlayerSwitchLaneState(this) }
            };
        }

        public bool IsTargetCellBlocked(int targetLaneIndex, float targetWorldY)
        {
            var levelGen = GameContext.Instance.Get<LevelGeneratorManager>();
            return levelGen != null && levelGen.IsCellBlockedByFurniture(targetLaneIndex, targetWorldY);
        }

        public override void StartStateController(Type initialStateType = null)
        {
            if (CurrentState?.IsStateActive == true)
            {
                CurrentState.OnExitState();
            }

            base.StartStateController(initialStateType);
        }

        public void ResetStateController(Type targetStateType = null)
        {
            StartStateController(targetStateType);
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }
    }
}
