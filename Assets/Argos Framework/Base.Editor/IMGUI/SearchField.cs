using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework.Utils;

namespace Argos.Framework.IMGUI
{
    public sealed class SearchField
    {
        #region Internal vars
        UnityEditor.IMGUI.Controls.SearchField _searchField;
        int _dropDownSelection;
        #endregion

        #region Properties
        /// <summary>
        /// Current selection index on dropdown filter.
        /// </summary>
        public int DropDownSelection
        {
            get { return Mathf.Clamp(this._dropDownSelection, 0, this.DropDownItems.Length - 1); }
            set { this._dropDownSelection = Mathf.Clamp(value, 0, this.DropDownItems.Length - 1); }
        }

        /// <summary>
        /// Filter values.
        /// </summary>
        public string[] DropDownItems { get; set; }
        #endregion

        #region Events
        public event Action<string> OnSearchTextChange;
        public event Action<int> OnDropDownSelect;
        #endregion

        #region Static members
        public static float Height => EditorGUIUtility.singleLineHeight;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dropDownItems">Filter values.</param>
        public SearchField(params string[] dropDownItems)
        {
            this.DropDownItems = dropDownItems;
            this._searchField = new UnityEditor.IMGUI.Controls.SearchField();
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="position">Position to draw the control. The height value is fixed and setup by control.</param>
        /// <param name="searchString">Current search string value.</param>
        /// <returns>Returns the search string value with changes (if there where changes).</returns>
        public string Do(Rect position, string searchString)
        {
            position.height = SearchField.Height;

            Rect popupButtonRect = position;
            {
                popupButtonRect.width = 16f;
            }

            int lastSelection = this.DropDownSelection;
            this.DropDownSelection = EditorGUI.Popup(popupButtonRect, this.DropDownSelection, this.DropDownItems, EditorSkinUtility.Styles.ArgosCustomVariants.invisibleButtonWithTransparentText);

            if (lastSelection != this.DropDownSelection)
            {
                this.OnDropDownSelect?.Invoke(this.DropDownSelection);
                lastSelection = this.DropDownSelection;
            }

            string searchFieldText = this._searchField.OnGUI(position, searchString, 
                                                             EditorSkinUtility.Styles.Custom.ToolbarSearch.textFieldPopup, 
                                                             EditorSkinUtility.Styles.Custom.ToolbarSearch.cancelButton, 
                                                             EditorSkinUtility.Styles.Custom.ToolbarSearch.cancelButtonEmpty);

            if (string.IsNullOrEmpty(searchFieldText) && !this._searchField.HasFocus())
            {
                Rect labelRect = position;
                {
                    labelRect.xMin += 14f;
                    labelRect.y--;
                }

                EditorGUI.LabelField(labelRect, this.DropDownItems[this.DropDownSelection], EditorSkinUtility.Styles.ArgosCustomVariants.disabledMiniLabel);
            }

            if (!string.IsNullOrEmpty(searchFieldText) && !string.IsNullOrEmpty(searchString) && !searchFieldText.Equals(searchString))
            {
                this.OnSearchTextChange?.Invoke(searchFieldText);
            }

            return searchFieldText;
        }

        /// <summary>
        /// Draws control using the current inspector layout.
        /// </summary>
        /// <param name="searchString">Current search string value.</param>
        /// <param name="fullWidth">Using full width? If is false, using field width by default.</param>
        /// <returns>Returns the search string value with changes (if there where changes).</returns>
        public string DoLayout(string searchString, bool fullWidth = false)
        {
            Rect position = EditorGUILayout.GetControlRect(!fullWidth);
            return this.Do(position, searchString);
        }
        #endregion
    }
}
