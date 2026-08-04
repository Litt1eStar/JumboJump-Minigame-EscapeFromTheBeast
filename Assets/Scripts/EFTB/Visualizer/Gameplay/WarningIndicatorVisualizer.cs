using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer.Gameplay
{
    public class WarningIndicatorVisualizer
    {
        private UIGameplayCanvas gameplayCanvas;

        public void Initialize()
        {
            gameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
        }

        public void SetWarningIndicatorActive(int laneIndex, bool active)
        {
            if (gameplayCanvas == null)
            {
                DebugLogHelper.LogError("[WarningIndicatorVisualizer] gameplayCanvas is null");    
                return;
            }

            gameplayCanvas.SetWarningIndicatorActive(laneIndex, active);
        }
    }
}
