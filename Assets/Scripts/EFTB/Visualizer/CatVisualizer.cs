using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.State;
using JumboJumps.EFTB.UI;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer
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
                this.controller.EventTimerChanged += OnTransitionTimerCountdown;
            }
        }

        public void UpdateLogic(float deltaTime)
        {
            if (giCat)
            {
                giCat.UpdateLogic(deltaTime);
            }
        }

        public void Dispose()
        {
            if (controller != null)
            {
                controller.EventStateChanged -= OnStateChange;
                controller.EventTimerChanged -= OnTransitionTimerCountdown;
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
            if(giCat)
            {
                giCat.EventTargetSpotted += OnSpotted;
                giCat.EventTargetLost += OnLost;
            }
        }

        public void Unsubscribe()
        {
            if (giCat)
            {
                giCat.EventTargetSpotted -= OnSpotted;
                giCat.EventTargetLost -= OnLost;
            }

        }

        public void OnStateChange(BaseState prev, BaseState next)
        {
            if(label == null || next == null) return;
            label.SetText(next.GetType().Name);
            label.SetTimerCountdown("");
        }
        public void OnTransitionTimerCountdown(float seconds) 
        {
            label.SetTimerCountdown($"{seconds.ToString("F2")}s");
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
