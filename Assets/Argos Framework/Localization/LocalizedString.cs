using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Argos.Framework;

namespace Argos.Framework.Localization
{
    [AddComponentMenu("Argos.Framework/Localization/Localized String")]
    public class LocalizedString : MonoBehaviour
    {
        #region Internal vars
        Text _text;
        #endregion

        #region Inspector fields
        [SerializeField]
        string _localizedStringKey;
        #endregion

        #region Initializers
        private void Awake()
        {
            this._text = GetComponentInChildren<Text>();
            LocalizationManager.Instance.OnLanguageChange += this.OnLanguageChange;
        }

        private void Start()
        {
            this.OnLanguageChange(SystemLanguage.Unknown);
        }
        #endregion

        #region Events
        private void OnDestroy()
        {
            LocalizationManager.Instance.OnLanguageChange -= this.OnLanguageChange;
        }

        void OnLanguageChange(SystemLanguage language)
        {
            this._text.text = LocalizationManager.Instance.GetLocalizedString(this._localizedStringKey);
        } 
        #endregion
    } 
}
