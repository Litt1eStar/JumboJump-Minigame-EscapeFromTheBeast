using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EFTB.State.Cat.SleepyCat
{
    public class CatSleepState : BaseState
    {
        public CatSleepState(BaseStateController stateController) : base(stateController)
        {

        }

        public override void OnEnterState()
        {
            base.OnEnterState();
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }

        public override void UpdateLogic(float deltaTime)
        {

        }

        private IEnumerator SleepCoroutine(float timeTillAwake)
        {
            return null;
        }
    }
}
