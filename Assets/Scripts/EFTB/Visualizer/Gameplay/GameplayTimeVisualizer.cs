using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.UI.Gameplay;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer.Gameplay
{
    public class GameplayTimeVisualizer
    {
        private UIGameplayCanvas uiGameplayCanvas;
        private GameplayTimeManager gameplayTimeManager;
        public void Initialize(GameplayTimeManager gameplayTimeManager)
        {
            uiGameplayCanvas = SceneObjectContext.Instance.Get<UIGameplayCanvas>();
            
            if (uiGameplayCanvas == null)
            {
                DebugLogHelper.LogError("UIGameplayCanvas is not found in the scene. Please ensure it is present and properly initialized.");
            }

            this.gameplayTimeManager = gameplayTimeManager;

            Subscribe();
        }

        public void Dispose()
        {
            uiGameplayCanvas = null;

            UnSubscribe();
        }

        public void Subscribe()
        {
            gameplayTimeManager.EventGameplayTimerChanged += OnGameplayTimerChanged;
        }

        public void UnSubscribe()
        {
            gameplayTimeManager.EventGameplayTimerChanged -= OnGameplayTimerChanged;
        }

        public void OnGameplayTimerChanged(float value)
        {
            // Timer UI is disabled for endless gameplay
        }
    }
}
