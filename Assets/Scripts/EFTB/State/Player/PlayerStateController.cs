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
        public PlayerMovementController movementController { get; private set; }
        public PlayerStateController()
        {
            movementController = new PlayerMovementController();
            movementController.Initialize();

            States = new Dictionary<Type, BaseState>
            {
                { typeof(PlayerIdleState), new PlayerIdleState(this) },
                { typeof(PlayerWalkingState), new PlayerWalkingState(this) }
            };
        }
    }
}
