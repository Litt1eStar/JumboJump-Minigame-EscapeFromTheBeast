using JumboJumps.EFTB.Constant.Sound;
using JumboJumps.EFTB.Sound;
using JumboJumps.EFTB.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Utilities
{
    [RequireComponent(typeof(Button))]
    public class UIButtonSoundPlayer : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private string soundKey;

        private void Awake()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            if (button == null)
            {
                DebugLogHelper.LogError($"[UIButtonSoundPlayer] Missing Button component on {gameObject.name}.");
                return;
            }

            if (string.IsNullOrEmpty(soundKey))
            {
                soundKey = ConstSound.Keys.UI.BUTTON_CLICK;
            }

            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(OnClick);
            }
        }

        private void OnClick()
        {
            EFTBSound.PlaySFX(soundKey);
        }
    }
}
