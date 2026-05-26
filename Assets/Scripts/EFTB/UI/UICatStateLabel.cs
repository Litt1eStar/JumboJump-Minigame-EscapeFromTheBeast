using TMPro;
using UnityEngine;

namespace Assets.Scripts.EFTB.UI
{
    public class UICatStateLabel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label;

        public void SetText(string text)
        {
            if (label != null)
            {
                label.text = text;
            }
        }
    }
}
