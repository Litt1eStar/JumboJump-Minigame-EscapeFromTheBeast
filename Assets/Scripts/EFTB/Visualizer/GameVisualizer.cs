using JumboJumps.EFTB.UI;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer
{
    public class GameVisualizer
    {
        private UIGameStateDebugLabel uiGameStateDebugLabel;

        public void Initialize()
        {
            uiGameStateDebugLabel = SceneObjectContext.Instance.Get<UIGameStateDebugLabel>();
            
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
        }

        public void UpdateOuterStateLabel(string text)
        {
            if(uiGameStateDebugLabel == null)
            {
                DebugLogHelper.LogError("UIGameStateDebugLabel not found in SceneObjectContext. Cannot update outer state label.");
                return;
            }

            uiGameStateDebugLabel.SetOuterLabel(text);
        }

        public void UpdateInnerStateLabel(string text)
        {
            if (uiGameStateDebugLabel == null)
            {
                DebugLogHelper.LogError("UIGameStateDebugLabel not found in SceneObjectContext. Cannot update inner state label.");
                return;
            }

            uiGameStateDebugLabel.SetInnerLabel(text);
        }
    }
}
