using Assets.Scripts.EFTB.State.Cat.SleepyCat;
using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.Manager
{
    public class SleepyCatBehaviourConfig
    {
        public float TIME_TILL_AWAKE;
        public float TIME_TO_ALERT;
        public float TIME_TO_CATCH;

        public SleepyCatBehaviourConfig(
            float TIME_TILL_AWAKE,
            float TIME_TO_ALERT,
            float TIME_TO_CATCH
            )
        {
            this.TIME_TILL_AWAKE = TIME_TILL_AWAKE;
            this.TIME_TO_ALERT = TIME_TO_ALERT;
            this.TIME_TO_CATCH = TIME_TO_CATCH;
        }
    }

    public class CatManager
    {
        private SleepyCatStateController sleepyCatStateController;
        public void Intialize(SleepyCatBehaviourConfig config)
        {
            sleepyCatStateController = new SleepyCatStateController(config);
            sleepyCatStateController.Initialize();
            sleepyCatStateController.StartStateController();

            GameContext.Instance.Add(this);
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
