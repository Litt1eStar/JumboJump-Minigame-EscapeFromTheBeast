using Assets.Scripts.EFTB.State.Cat.SleepyCat;
using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.Manager
{
    public class CatManager
    {
        private SleepyCatStateController sleepyCatStateController;
        public void Intialize()
        {
            sleepyCatStateController = new SleepyCatStateController();
            sleepyCatStateController.Initialize();
            sleepyCatStateController.StartStateController();
        }

        public void Dispose()
        {
            sleepyCatStateController = null;
            sleepyCatStateController.Dispose();
        }

        public void UpdateLogic(float deltaTime)
        {
            sleepyCatStateController.UpdateLogic(deltaTime);
        }
    }
}
