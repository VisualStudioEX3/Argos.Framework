using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using Argos.Framework.Helpers;

namespace Argos.Framework.Localization
{
    /// <summary>
    /// Localization text string manager.
    /// </summary>
    [AddComponentMenu("Argos.Framework/Localization/Localization Manager")]
    public sealed class LocalizationManager : MonoBehaviour
    {
        #region Singleton
        public static LocalizationManager Instance { get; private set; }
        #endregion

        #region Constants
        const string LOCALIZED_STRING_NOT_FOUND = "'{0}' localized string not found.";
        static readonly SystemLanguage[] SPANISH_LANGUAGES = { SystemLanguage.Spanish, SystemLanguage.Basque, SystemLanguage.Catalan };
        #endregion

        #region Structs
        [Serializable]
        public struct LocalizedStringData
        {
            public string English;
            public string Spanish;
        }
        #endregion

        #region Inspector fields
        [SerializeField]
        char _fieldSeparatorChar = '|';

        [SerializeField]
        TextAsset _localizationFile;

        [SerializeField]
        SystemLanguage _currentLanguage = SystemLanguage.English;
        #endregion

        #region Public vars
        /// <summary>
        /// Event raise when the language is changed.
        /// </summary>
        public event Action<SystemLanguage> OnLanguageChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Get or set the current language.
        /// </summary>
        /// <remarks>Only supported spanish language and variants, and english as default.</remarks>
        public SystemLanguage CurrentLanguage
        {
            get
            {
                return this._currentLanguage;
            }
            set
            {
                var selection = value;
                selection = LocalizationManager.CheckAllowedLanguages(selection);

                if (this._currentLanguage != selection)
                {
                    this._currentLanguage = selection;
                    this.OnLanguageChanged?.Invoke(this._currentLanguage);
                }
            }
        }

        /// <summary>
        /// Read only dictionary with all localized strings.
        /// </summary>
        public ReadOnlyDictionary<string, LocalizedStringData> Strings { get; private set; }
        #endregion

        #region Initializers
        private void Awake()
        {
            this.ImportFile();
            LocalizationManager.Instance = this;
        }
        #endregion

        #region Events
        void Reset()
        {
            this.Strings = new ReadOnlyDictionary<string, LocalizedStringData>(new Dictionary<string, LocalizedStringData>());
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Check if the language is an allowed game language.
        /// </summary>
        /// <param name="language">Language to check.</param>
        /// <returns>Return English as default language it this not match any spanish language.</returns>
        public static SystemLanguage CheckAllowedLanguages(SystemLanguage language)
        {
            return !CollectionsHelper.Exists<SystemLanguage>(language, LocalizationManager.SPANISH_LANGUAGES) ? SystemLanguage.English : language;
        }

        /// <summary>
        /// Check if the language is a spanish language.
        /// </summary>
        /// <param name="language">Language to check.</param>
        /// <returns>Return true if is a spanish language or variant.</returns>
        public static bool IsSpanishLanguage(SystemLanguage language)
        {
            return CollectionsHelper.Exists<SystemLanguage>(language, LocalizationManager.SPANISH_LANGUAGES);
        }

        /// <summary>
        /// Import and parse the localization file.
        /// </summary>
        public void ImportFile()
        {
            // Separate in single lines (remove first \r chars on Windows text formats):
            string[] lines = this._localizationFile.text.Replace('\r', '\0').Split('\n').Where(e => e.Length > 1).ToArray();
            var values = new Dictionary<string, LocalizedStringData>();

            foreach (string line in lines)
            {
                string[] fields = line.Split(this._fieldSeparatorChar);
                values.Add(fields[0], new LocalizedStringData() { English = fields[1], Spanish = fields[2] });
            }

            this.Strings = new ReadOnlyDictionary<string, LocalizedStringData>(values);
        }

        /// <summary>
        /// Return the localized string by key.
        /// </summary>
        /// <param name="key">Key of the localized string.</param>
        /// <returns>Return the localized key for current system language.</returns>
        public string GetLocalizedString(string key)
        {
            if (this.Strings.ContainsKey(key))
            {
                var localizedString = this.Strings[key];
                if (CollectionsHelper.Exists<SystemLanguage>(this._currentLanguage, LocalizationManager.SPANISH_LANGUAGES))
                {
                    return localizedString.Spanish;
                }
                else
                {
                    return localizedString.English;
                }
            }
            else
            {
                return string.Format(LocalizationManager.LOCALIZED_STRING_NOT_FOUND, key);
            }
        }
        #endregion
    }
}