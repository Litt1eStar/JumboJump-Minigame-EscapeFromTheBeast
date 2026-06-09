using JumboJumps.EFTB.UI.MainMenu;
using JumboJumps.EFTB.Utilities;
using System;

namespace JumboJumps.EFTB.Visualizer.MainMenu
{
    public class MainMenuVisualizer
    {
        private UIMainMenuCanvas uiMainMenuCanvas;
        
        public void Initialize()
        {
            uiMainMenuCanvas = SceneObjectContext.Instance.Get<UIMainMenuCanvas>();

            if(uiMainMenuCanvas == null)
            {
                DebugLogHelper.LogError("Failed to initialize MainMenuVisualizer: UIMainMenuCanvas not found in scene.");
            }
        }

        public void Dispose() 
        {
            uiMainMenuCanvas = null;
        }

        public void Subscribe(Action playBtnCallback, Action exitBtnCallback)
        {
            uiMainMenuCanvas?.Subscribe(playBtnCallback, exitBtnCallback);
        }
    }
}
