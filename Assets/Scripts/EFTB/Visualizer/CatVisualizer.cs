using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.Visualizer
{
    public class CatVisualizer    {
        public GICat giCat { get; private set; }
        public void Initialize(GICat giCat)
        {
            this.giCat = giCat;
        }

        public void UpdateLogic(float deltaTime)
        {
            giCat?.UpdateLogic(deltaTime);
        }

        public void Dispose()
        {
            Unsubscribe();
            giCat = null;
        }

        #region Event Handler
        /// <summary>
        /// Subscribe and Unsubscribe only when cat have to use GICatsight
        /// </summary>
        public void Subscribe()
        {
            giCat.OnTargetSpotted += OnSpotted;
            giCat.OnTargetLost += OnLost;
        }

        public void Unsubscribe()
        {
            giCat.OnTargetSpotted -= OnSpotted;
            giCat.OnTargetLost -= OnLost;
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
        public bool IsTargetInSght() => giCat != null && giCat.IsTargetInSight;
        
    }
}
