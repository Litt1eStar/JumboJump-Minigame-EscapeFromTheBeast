using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.Visualizer
{
    public class CatVisualizer
    {
        public GICatSight giSleepyCatSight { get; private set; }
        public void Initialize()
        {
            giSleepyCatSight = SceneObjectContext.Instance.Get<GICatSight>();
            if (giSleepyCatSight == null)
            {
                DebugLogHelper.LogError("GICatSight not found in SceneObjectContext. CatVisualizer initialization failed.");
            }

            DebugLogHelper.Log($"{GetType().Name} got Initialized");
        }

        public void Dispose()
        {
            giSleepyCatSight = null;
        }
    }
}
