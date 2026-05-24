using Assets.Scripts.EFTB.Manager;
using Assets.Scripts.EFTB.Utilities;
using Assets.Scripts.EFTB.Visualizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EFTB.State.Player
{
    public class PlayerStateController : BaseStateController
    {
        protected override Type DefaultTypeState => typeof(PlayerIdleState);
        public PlayerVisualizer visualizer { get; private set; }
        public Input2DManager input2DManager { get; private set; }
        public PlayerStateController()
        {
            visualizer = new PlayerVisualizer();
            visualizer.Initialize();

            input2DManager = GameContext.Instance.Get<Input2DManager>();
            if(input2DManager == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] {nameof(PlayerStateController)}| Failed to get {typeof(Input2DManager).AssemblyQualifiedName} from GameContext");
            }

            States = new Dictionary<Type, BaseState>
            {
                { typeof(PlayerIdleState), new PlayerIdleState(this) },
                { typeof(PlayerWalkingState), new PlayerWalkingState(this) }
            };
        }
    }
}
