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

        public PlayerStateController()
        {
            Visualizer = new PlayerVisualizer();
            Visualizer.Initialize();

            Input2DManager = GameContext.Instance.Get<Input2DManager>();
            if(Input2DManager == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] {nameof(PlayerStateController)}| Failed to get {typeof(Input2DManager).AssemblyQualifiedName} from GameContext");
            }

            States = new Dictionary<Type, BaseState>
            {
                { typeof(PlayerIdleState), new PlayerIdleState(this) },
                { typeof(PlayerWalkingState), new PlayerWalkingState(this) }
            };
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            Visualizer.giCamera.UpdateLogic(deltaTime);
        }
    }
}
