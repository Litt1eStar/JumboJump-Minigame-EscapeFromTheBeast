using TMPro;
using UnityEngine;

namespace JumboJumps.EFTB.UI
{
    public class UIGameStateDebugLabel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI outerStateLabel;

        [SerializeField]
        private TextMeshProUGUI innerStateLabel;

        public void SetOuterLabel(string text)
        {
            if(outerStateLabel == null)
            {
                Debug.LogError("UIGameStateDebugLabel: OuterStateLabel is not assigned.");
                return;
            }

            outerStateLabel.text = text;
        }

        public void SetInnerLabel(string text)
        {
            if (innerStateLabel == null)
            {
                Debug.LogError("UIGameStateDebugLabel: InnerStateLabel is not assigned.");
                return;
            }

            innerStateLabel.text = text;
        }
    }
}
