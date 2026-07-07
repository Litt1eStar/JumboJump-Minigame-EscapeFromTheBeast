using JumboJumps.EFTB.UI.MainMenu;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Visualizer.MainMenu
{
    public class MainMenuVisualizer
    {
        public event Action EventPlayUIButtonClicked;
        public event Action EventExitUIButtonClicked;

        private UIMainMenuCanvas uiMainMenuCanvas;
        
        private UIMainMenuPanel uiMainMenuPanel;
        
        public void Initialize()
        {
            uiMainMenuCanvas = SceneObjectContext.Instance.Get<UIMainMenuCanvas>();

            if (uiMainMenuCanvas == null)
            {
                DebugLogHelper.LogError("Failed to initialize MainMenuVisualizer: UIMainMenuCanvas not found in scene.");
            }

            uiMainMenuCanvas.Initialize();

            uiMainMenuPanel = uiMainMenuCanvas?.UIMainMenuPanel;

            Subscribe();
        }

        public void Dispose() 
        {
            UnSubscribe();
            uiMainMenuCanvas = null;
        }

        public void Subscribe()
        {
            uiMainMenuPanel.EventPlayUIButtonClicked += OnPlayButtonClicked;
            uiMainMenuPanel.EventExitUIButtonClicked += OnExitButtonClicked;
        }

        public void UnSubscribe()
        {
            uiMainMenuPanel.EventPlayUIButtonClicked -= OnPlayButtonClicked;
            uiMainMenuPanel.EventExitUIButtonClicked -= OnExitButtonClicked;
        }

        public void OnPlayButtonClicked()
        {
            EventPlayUIButtonClicked?.Invoke();
        }

        public void OnExitButtonClicked()
        {
            EventExitUIButtonClicked?.Invoke();
        }
    }
}
