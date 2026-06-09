using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.InitialLoading
{
    public class UILoadingPanel : UIBasePanel
    {
        [SerializeField]
        private Slider loadingProgressBar;

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void SetProgress(float value)
        {
            if(loadingProgressBar != null)
            {
                loadingProgressBar.value = value;
            }
        }
    }
}
