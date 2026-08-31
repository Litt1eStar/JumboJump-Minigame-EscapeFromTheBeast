using JumboJumps.EFTB.Sound;
using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.ErrorPopup
{
    public class UIErrorPopupPanel : MonoBehaviour
    {
        [SerializeField]
        private Button confirmButton;

        private Action confirmCallback;

        public void Initialize()
        {
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
            confirmCallback = null;
        }

        public void Show(Action onConfirmed = null)
        {
            confirmCallback = onConfirmed;
            gameObject.SetActive(true);
            DebugLogHelper.Log("Show");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            confirmCallback = null;
        }

        private void Subscribe()
        {
            if (confirmButton == null) return;

            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private void Unsubscribe()
        {
            if (confirmButton == null) return;

            confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
        }

        private void OnConfirmButtonClicked()
        {
            var callback = confirmCallback;

            EFTBSound.PlayUIClick();
            Hide();
            callback?.Invoke();
        }
    }
}
