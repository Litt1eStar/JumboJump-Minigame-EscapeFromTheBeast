using System;
using System.Collections;
using JumboJumps.EFTB.Utilities;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace JumboJumps.EFTB.Manager
{
    public class LocalizationManager
    {
        private const string DEFAULT_LANGUAGE_CODE = "th";

        public bool IsLocaleSet { get; private set; }
        public string CurrentLanguage { get; private set; }
        public event Action<string> EventLanguageChanged;

        private CoroutineHelper coroutineHelper;
        private Coroutine selectLocaleCoroutine;

        public void Initialize(CoroutineHelper coroutineHelper)
        {
            this.coroutineHelper = coroutineHelper;
            CurrentLanguage = DEFAULT_LANGUAGE_CODE;
            IsLocaleSet = false;

            GameContext.Instance.Add(this);
            DebugLogHelper.Log($"[{GetType().Name}] Initialized successfully.");
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);

            if (coroutineHelper != null && selectLocaleCoroutine != null)
            {
                coroutineHelper.Stop(selectLocaleCoroutine);
                selectLocaleCoroutine = null;
            }

            IsLocaleSet = false;
            CurrentLanguage = null;
            DebugLogHelper.Log($"[{GetType().Name}] Disposed.");
        }

        public void ApplyLanguageCode(string languageCode)
        {
            IsLocaleSet = false;
            CurrentLanguage = NormalizeLanguageCode(languageCode);
            selectLocaleCoroutine = coroutineHelper.Restart(selectLocaleCoroutine, SetLocaleRoutine(CurrentLanguage));
        }

        public void ToggleLanguage()
        {
            string nextLanguage = (CurrentLanguage == "th") ? "en" : "th";
            ApplyLanguageCode(nextLanguage);
        }

        private IEnumerator SetLocaleRoutine(string languageCode)
        {
            yield return LocalizationSettings.InitializationOperation;

            var locale = LocalizationSettings.AvailableLocales.Locales
                .Find(l => l.Identifier.Code.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

            if (locale != null)
            {
                LocalizationSettings.SelectedLocale = locale;
                DebugLogHelper.Log($"[{GetType().Name}] Set localization locale to <{languageCode}>.");
            }
            else
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Locale <{languageCode}> not found. Current: <{LocalizationSettings.SelectedLocale?.Identifier.Code}>.");
            }

            IsLocaleSet = true;
            EventLanguageChanged?.Invoke(CurrentLanguage);
        }

        private string NormalizeLanguageCode(string languageCode)
        {
            return string.IsNullOrWhiteSpace(languageCode) ? DEFAULT_LANGUAGE_CODE : languageCode.Trim().ToLowerInvariant();
        }
    }
}
