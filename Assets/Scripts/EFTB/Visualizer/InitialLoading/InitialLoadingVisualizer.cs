using JumboJumps.EFTB.UI.InitialLoading;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer.InitialLoading
{
    public class InitialLoadingVisualizer
    {
        private UILoadingCanvas uiLoadingCanvas;

        public void Initialize()
        {
            uiLoadingCanvas = SceneObjectContext.Instance.Get<UILoadingCanvas>();
        }

        public void Dispose()
        {
            uiLoadingCanvas = null;
        }

        public void SetProgress(float value)
        {
            uiLoadingCanvas?.SetProgress(value);
        }

        public void Show()
        {
            uiLoadingCanvas?.Show();
        }

        public void Hide()
        {
            uiLoadingCanvas?.Hide();
        }
    }
}
