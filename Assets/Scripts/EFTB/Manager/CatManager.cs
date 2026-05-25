using Assets.Scripts.EFTB.State.Cat.SleepyCat;

namespace Assets.Scripts.EFTB.Manager
{
    public class CatManager
    {
        private SleepyCatStateController sleepyCatStateController;

        public void Intialize()
        {
            sleepyCatStateController = new SleepyCatStateController();
        }

        public void Dispose()
        {
            sleepyCatStateController = null;
        }

        public void UpdateLogic(float deltaTime)
        {
            sleepyCatStateController.UpdateLogic(deltaTime);
        }
    }
}
