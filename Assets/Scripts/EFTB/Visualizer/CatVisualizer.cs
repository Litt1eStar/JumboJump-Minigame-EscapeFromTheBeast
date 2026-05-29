using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer
{
    public class CatVisualizer    {
        public GICatSight giSleepyCatSight { get; private set; }
        public void Initialize()
        {
            giSleepyCatSight = SceneObjectContext.Instance.Get<GICatSight>();
            if (giSleepyCatSight == null)
            {
                DebugLogHelper.LogError("GICatSight not found in SceneObjectContext. CatVisualizer initialization failed.");
                return;
            }
        }

        public void UpdateLogic(float deltaTime)
        {
            giSleepyCatSight?.UpdateLogic(deltaTime);
        }

        public void Dispose()
        {
            Unsubscribe();
            giSleepyCatSight = null;
        }

        public void Subscribe()
        {
            giSleepyCatSight.OnTargetSpotted += OnSpotted;
            giSleepyCatSight.OnTargetLost += OnLost;
        }

        public void Unsubscribe()
        {
            giSleepyCatSight.OnTargetSpotted -= OnSpotted;
            giSleepyCatSight.OnTargetLost -= OnLost;
        }

        public void OnSpotted()
        {
            DebugLogHelper.Log("Cat spotted the target!");
        }

        public void OnLost()
        {
            DebugLogHelper.Log("Cat lost sight of the target.");
        }

        public bool IsTargetInSght() => giSleepyCatSight != null && giSleepyCatSight.IsTargetInSight;
        
    }
}
