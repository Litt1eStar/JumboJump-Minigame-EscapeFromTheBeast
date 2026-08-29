using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.UI.ErrorPopup
{
    public class UIErrorPopupCanvas : MonoBehaviour
    {
        [SerializeField]
        private UIErrorPopupPanel errorPopupPanel;

        public void Initialize()
        {
            //transform.localScale = Vector3.one;
            errorPopupPanel?.Initialize();
            Hide();
            DebugLogHelper.Log("UIErrorPopupPanel was Initialized");
        }

        public void Dispose()
        {
            errorPopupPanel?.Dispose();
        }

        public void Show(Action onConfirmed = null)
        {
            gameObject.SetActive(true);
            //transform.localScale = Vector3.one;
            errorPopupPanel?.Show(onConfirmed);
        }

        public void Hide()
        {
            errorPopupPanel?.Hide();
        }
    }
}
