using TMPro;
using UnityEngine;

namespace Assets.Scripts.EFTB.UI
{
    public class UICatStateLabel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label;
        [SerializeField]
        private TextMeshProUGUI timerCountdown;

        public void SetText(string text)
        {
            if (label != null)
            {
                label.text = text;
            }
        }

        public void SetTimerCountdown(string text)
        {
            if(timerCountdown != null)
            {
                timerCountdown.text = text;
            }
        }
    }
}
