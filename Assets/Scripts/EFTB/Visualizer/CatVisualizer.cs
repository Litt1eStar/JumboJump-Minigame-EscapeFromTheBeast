using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.Visualizer
{
    public class CatVisualizer    {
        public GICatSight giSleepyCatSight { get; private set; }
        public void Initialize(GICatSight sight)
        {
            giSleepyCatSight = sight;
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

        #region Event Handler
        /// <summary>
        /// Subscribe and Unsubscribe only when cat have to use GICatsight
        /// </summary>
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

        #endregion
        public bool IsTargetInSght() => giSleepyCatSight != null && giSleepyCatSight.IsTargetInSight;
        
    }
}
