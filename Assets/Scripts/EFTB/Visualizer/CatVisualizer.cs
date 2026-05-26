using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.UI;
using Assets.Scripts.EFTB.Utilities;

namespace Assets.Scripts.EFTB.Visualizer
{
    public class CatVisualizer    {
        private GICat giCat;
        private UICatStateLabel label;
        private BaseStateController controller;
        public void Initialize(
            GICat giCat,
            UICatStateLabel label,
            BaseStateController controller
            )
        {
            this.giCat = giCat;
            this.label = label;
            this.controller = controller;

            if(controller != null)
            {
                this.controller.EventStateChanged += OnStateChange;
            }
        }

        public void UpdateLogic(float deltaTime)
        {
            giCat?.UpdateLogic(deltaTime);
        }

        public void Dispose()
        {
            if(controller != null)
            {
                controller.EventStateChanged -= OnStateChange; 
            }

            Unsubscribe();
            giCat = null;
            label = null;
            controller = null;
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

        public void OnStateChange(BaseState prev, BaseState next)
        {
            if(label == null || next == null) return;
            label.SetText(next.GetType().Name);
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
