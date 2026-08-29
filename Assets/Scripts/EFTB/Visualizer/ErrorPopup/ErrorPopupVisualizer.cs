using JumboJumps.EFTB.UI.ErrorPopup;
using JumboJumps.EFTB.Utilities;
using System;
using UnityEngine;

namespace JumboJumps.EFTB.Visualizer.ErrorPopup
{
    public class ErrorPopupVisualizer
    {
        private UIErrorPopupCanvas uiErrorPopupCanvas;

        public void Initialize(UIErrorPopupCanvas canvas = null)
        {
            uiErrorPopupCanvas = canvas;
            
            if (uiErrorPopupCanvas != null)
            {
                uiErrorPopupCanvas.Initialize();
            }
            else
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Failed to resolve UIErrorPopupCanvas in SceneObjectContext or Scene.");
            }

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance?.Remove(this);

            uiErrorPopupCanvas?.Dispose();
            uiErrorPopupCanvas = null;
        }

        public void Show(Action onConfirmed = null)
        {
            if (uiErrorPopupCanvas == null)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] Cannot show Error Popup: UIErrorPopupCanvas is missing from scene.");
                return;
            }

            uiErrorPopupCanvas.Show(onConfirmed);
        }

        public void Hide()
        {
            uiErrorPopupCanvas?.Hide();
        }
    }
}
