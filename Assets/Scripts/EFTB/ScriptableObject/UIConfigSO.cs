using JumboJumps.EFTB.Constant.Gameplay;
using UnityEngine;

namespace JumboJumps.EFTB.Config
{
    [CreateAssetMenu(fileName = "UIConfigSO", menuName = "EFTB/Config/UIConfig")]
    public class UIConfigSO : ScriptableObject
    {
        [Header("Main Menu Logo Animation")]
        [Tooltip("Speed multiplier for Logo idle scaling sine wave (default: 3.0)")]
        [SerializeField] private float logoIdleScaleSpeed = ConstGameplay.UI.MainMenu.LOGO_IDLE_SCALE_SPEED;

        [Tooltip("Minimum scale factor for Logo idle animation (default: 0.95)")]
        [SerializeField] private float logoIdleScaleMin = ConstGameplay.UI.MainMenu.LOGO_IDLE_SCALE_MIN;

        [Tooltip("Maximum scale factor for Logo idle animation (default: 1.05)")]
        [SerializeField] private float logoIdleScaleMax = ConstGameplay.UI.MainMenu.LOGO_IDLE_SCALE_MAX;

        [Header("Ready / Go Transition Animation")]
        [Tooltip("Fade in/out duration in seconds for Logo, StartBtn, Ready, and Go elements (default: 0.3s)")]
        [SerializeField] private float fadeDuration = ConstGameplay.UI.MainMenu.FADE_DURATION;

        [Tooltip("Hold duration in seconds at peak scale for Ready/Go elements (default: 0.4s)")]
        [SerializeField] private float readyGoHoldDuration = ConstGameplay.UI.MainMenu.READY_GO_HOLD_DURATION;

        [Tooltip("Initial scale factor for Ready/Go elements before pop-in (default: 0.5)")]
        [SerializeField] private float readyGoScaleStart = ConstGameplay.UI.MainMenu.READY_GO_SCALE_START;

        [Tooltip("Target peak scale factor for Ready/Go pop-in animation (default: 1.1)")]
        [SerializeField] private float readyGoScaleTarget = ConstGameplay.UI.MainMenu.READY_GO_SCALE_TARGET;

        [Tooltip("Max Z rotation angle in degrees for Ready swing impact (default: 20.0)")]
        [SerializeField] private float readySwingMaxZAngle = ConstGameplay.UI.MainMenu.READY_SWING_MAX_Z_ANGLE;

        [Tooltip("Swing oscillation speed multiplier for Ready element (default: 15.0)")]
        [SerializeField] private float readySwingSpeed = ConstGameplay.UI.MainMenu.READY_SWING_SPEED;

        [Tooltip("Impact scale out target multiplier for Go element (default: 2.2)")]
        [SerializeField] private float goScaleOutTarget = ConstGameplay.UI.MainMenu.GO_SCALE_OUT_TARGET;

        public float LogoIdleScaleSpeed => logoIdleScaleSpeed;
        public float LogoIdleScaleMin => logoIdleScaleMin;
        public float LogoIdleScaleMax => logoIdleScaleMax;
        public float FadeDuration => fadeDuration;
        public float ReadyGoHoldDuration => readyGoHoldDuration;
        public float ReadyGoScaleStart => readyGoScaleStart;
        public float ReadyGoScaleTarget => readyGoScaleTarget;
        public float ReadySwingMaxZAngle => readySwingMaxZAngle;
        public float ReadySwingSpeed => readySwingSpeed;
        public float GoScaleOutTarget => goScaleOutTarget;
    }
}
