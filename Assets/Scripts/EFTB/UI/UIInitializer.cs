using JumboJumps.EFTB.UI.ErrorPopup;
using JumboJumps.EFTB.Visualizer.ErrorPopup;
using UnityEngine;

namespace JumboJumps.EFTB.UI
{
    public class UIInitializer : MonoBehaviour
    {
        [SerializeField]
        private UIErrorPopupCanvas uiErrorPopupCanvas;

        private ErrorPopupVisualizer errorPopupVisualizer;

        public void Initialize()
        {
            errorPopupVisualizer = new ErrorPopupVisualizer();
            errorPopupVisualizer.Initialize(uiErrorPopupCanvas);
        }

        public void Dispose()
        {
            errorPopupVisualizer?.Dispose();
            errorPopupVisualizer = null;
        }
    }
}
