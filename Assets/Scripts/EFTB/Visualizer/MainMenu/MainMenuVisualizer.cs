using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Sound;
using JumboJumps.EFTB.UI.MainMenu;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace JumboJumps.EFTB.Visualizer.MainMenu
{
    public class MainMenuVisualizer
    {
        public event Action EventPlayUIButtonClicked;

        private UIMainMenuCanvas uiMainMenuCanvas;
        
        private UIMainMenuPanel uiMainMenuPanel;

        private CoroutineHelper coroutineHelper;
        private Coroutine fadeInWorldObjectsCoroutine;
        
        public void Initialize()
        {
            coroutineHelper = GameContext.Instance?.Get<CoroutineHelper>();
            uiMainMenuCanvas = SceneObjectContext.Instance.Get<UIMainMenuCanvas>();

            uiMainMenuCanvas?.Initialize();
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
            if (uiMainMenuPanel != null)
            {
                uiMainMenuPanel.EventPlayUIButtonClicked += OnPlayButtonClicked;
            }
        }

        public void UnSubscribe()
        {
            if (uiMainMenuPanel != null)
            {
                uiMainMenuPanel.EventPlayUIButtonClicked -= OnPlayButtonClicked;
            }
        }

        public void Show()
        {
            uiMainMenuCanvas?.Show();
        }

        public void Hide()
        {
            uiMainMenuCanvas?.Hide();
        }

        public void StartLogoIdleAnimation()
        {
            uiMainMenuCanvas?.StartLogoIdleAnimation();
        }

        public void PlayStartSequence(Action onComplete)
        {
            uiMainMenuCanvas?.PlayStartSequence(onComplete);
        }

        public void SetWorldObjectsAlpha(float alpha)
        {
            GIPlayer giPlayer = SceneObjectContext.Instance?.Get<GIPlayer>();

            if (giPlayer != null)
            {
                giPlayer.SetAlpha(alpha);
            }

            GIFurnitureObstacle[] furniture = UnityEngine.Object.FindObjectsOfType<GIFurnitureObstacle>(true);

            if (furniture != null)
            {
                foreach (var f in furniture)
                {
                    if (f == null) continue;
                    SetObjectRenderersAlpha(f.gameObject, alpha);
                }
            }

            GIHazardObstacle[] hazards = UnityEngine.Object.FindObjectsOfType<GIHazardObstacle>(true);

            if (hazards != null)
            {
                foreach (var h in hazards)
                {
                    if (h == null) continue;
                    SetObjectRenderersAlpha(h.gameObject, alpha);
                }
            }

            GICollectible[] collectibles = UnityEngine.Object.FindObjectsOfType<GICollectible>(true);
          
            if (collectibles != null)
            {
                foreach (var c in collectibles)
                {
                    if (c == null) continue;
                    SetObjectRenderersAlpha(c.gameObject, alpha);
                }
            }
        }

        private void SetObjectRenderersAlpha(GameObject obj, float alpha)
        {
            if (obj == null) return;

            SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>();

            foreach (var r in renderers)
            {
                if (r != null)
                {
                    Color c = r.color;
                    c.a = alpha;
                    r.color = c;
                }
            }
        }


        public void FadeInWorldObjects(float duration, Action onComplete)
        {
            fadeInWorldObjectsCoroutine = coroutineHelper.Play(FadeInWorldObjectsRoutine(duration, onComplete));
        }

        private IEnumerator FadeInWorldObjectsRoutine(float duration, Action onComplete)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = duration > 0f ? Mathf.Clamp01(elapsed / duration) : 1f;
                SetWorldObjectsAlpha(alpha);
                yield return null;
            }

            SetWorldObjectsAlpha(1f);
            onComplete?.Invoke();
        }

        public void OnPlayButtonClicked()
        {
            EFTBSound.PlayUIClick();
            EventPlayUIButtonClicked?.Invoke();
        }
    }
}
