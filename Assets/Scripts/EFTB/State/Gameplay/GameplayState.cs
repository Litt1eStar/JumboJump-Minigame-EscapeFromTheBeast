using Assets.Scripts.EFTB.Visualizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EFTB.State.Gameplay
{
    public class GameplayState : BaseState
    {
        private CollectibleVisualizer collectibleVisualizer;
        public GameplayState(BaseStateController stateController) : base(stateController)
        {
        }

        public override void OnEnterState()
        {
            base.OnEnterState();

            collectibleVisualizer = new CollectibleVisualizer();
            collectibleVisualizer.Initialize();
        }

        public override void OnExitState()
        {
            base.OnExitState();

            collectibleVisualizer?.Dispose();
            collectibleVisualizer = null;
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }
    }
}
