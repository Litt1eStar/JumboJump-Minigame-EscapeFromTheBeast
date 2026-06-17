using UnityEngine;

namespace JumboJumps.EFTB.UI.InitialLoading
{
    public class UILoadingCanvas : MonoBehaviour
    {
        [SerializeField]
        private UILoadingPanel uiLoadingPanel;

        public void Show()
        {
            if(uiLoadingPanel != null)
            {
                uiLoadingPanel.Show();
            }
        }

        public void Hide() 
        {
            if(uiLoadingPanel != null)
            {
                uiLoadingPanel.Hide();
            }
        }

        public void SetProgress(float value)
        {
            if(uiLoadingPanel != null)
            {
                uiLoadingPanel.SetProgress(value);
            }
        }
    }
}
