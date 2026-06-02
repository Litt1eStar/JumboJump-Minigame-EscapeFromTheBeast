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
            uiGameStateDebugLabel?.SetOuterLabel(text);
        }

        public void UpdateInnerStateLabel(string text)
        {
            uiGameStateDebugLabel?.SetInnerLabel(text);
        }
    }
}
