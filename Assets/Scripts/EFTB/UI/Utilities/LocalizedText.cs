using JumboJumps.EFTB.Constant.Localization;
using JumboJumps.EFTB.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace JumboJumps.EFTB.UI.Utilities
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [Header("Target Components")]
        [SerializeField] private TMP_Text targetText;

        [Header("Default Localization Settings")]
        [SerializeField] private LocalizedString localizedString = new LocalizedString();

        private void Awake()
        {
            if (targetText == null)
            {
                targetText = GetComponent<TMP_Text>();
            }
        }

        private void OnEnable()
        {
            if (localizedString != null)
            {
                localizedString.StringChanged += OnStringChanged;
                localizedString.RefreshString();
            }
        }

        private void OnDisable()
        {
            if (localizedString != null)
            {
                localizedString.StringChanged -= OnStringChanged;
            }
        }

        public void SetText(string text)
        {
            if (localizedString != null)
            {
                localizedString.StringChanged -= OnStringChanged;
            }
            if (targetText != null)
            {
                targetText.text = text;
            }
        }

        public void SetLocalizedKey(string entryKey, string table = ConstLocalization.DEFAULT_TABLE)
        {
            if (localizedString != null)
            {
                localizedString.StringChanged -= OnStringChanged;
            }

            localizedString = new LocalizedString
            {
                TableReference = table,
                TableEntryReference = entryKey
            };

            localizedString.StringChanged += OnStringChanged;
            localizedString.RefreshString();
        }

        private void OnStringChanged(string value)
        {
            if (targetText != null)
            {
                targetText.text = value;
            }
        }
    }
}
