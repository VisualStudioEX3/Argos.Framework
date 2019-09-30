using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Argos.Framework.Utils;

namespace Argos.Framework.IMGUI
{
    /// <summary>
    /// Toolbar SearchField control with popup support.
    /// </summary>
    public sealed class ToolbarSearchField
    {
        #region Internal vars
        SearchField _searchField;
        int _dropDownSelection;
        string _controlName;
        bool _showPopup;
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

        bool HasFocus
        {
            get
            {
#if UNITY_2019_3_OR_NEWER
                return GUI.GetNameOfFocusedControl().Equals(this._controlName);
#else
                return this._searchField.HasFocus();
#endif
            }
        }
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
        public ToolbarSearchField() : this(false)
        {
        }

        ToolbarSearchField(bool showPopup)
        {
            this._showPopup = showPopup;

#if UNITY_2019_3_OR_NEWER
            if (showPopup)
            {
                this._controlName = GUID.Generate().ToString();
            }
#else
            this._searchField = new UnityEditor.IMGUI.Controls.SearchField();
#endif
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dropDownItems">Filter values.</param>
        public ToolbarSearchField(params string[] dropDownItems) : this(true)
        {
            this.DropDownItems = dropDownItems;
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
            position.height = ToolbarSearchField.Height;

            if (this._showPopup)
            {
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
            }

            string searchFieldText;
#if UNITY_2019_3_OR_NEWER
            searchFieldText = this.DoToolbarSearchField(position, searchString, this._showPopup);
#else
            if (this._showPopup)
            {
                searchFieldText = this._searchField.OnGUI(position, searchString,
                                                          EditorSkinUtility.Styles.Custom.ToolbarSearch.textSearchFieldPopup,
                                                          EditorSkinUtility.Styles.Custom.ToolbarSearch.cancelButton,
                                                          EditorSkinUtility.Styles.Custom.ToolbarSearch.cancelButtonEmpty); 
            }
            else
            {
                searchFieldText = this._searchField.OnToolbarGUI(position, searchString);
            }
#endif

            if (this._showPopup && string.IsNullOrEmpty(searchFieldText) && !this.HasFocus)
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
            Rect position = EditorGUILayout.GetControlRect();
            {
                position.xMin += EditorGUIUtility.labelWidth + 4f;
            }
            return this.Do(position, searchString);
        }

        string DoToolbarSearchField(Rect position, string text, bool showWithPopupArrow)
        {
            position.width++;

            Rect position1 = position;
            Rect position2 = position;

            position2.x += position.width - 14f;
            position2.width = 14f;

            if (!string.IsNullOrEmpty(text))
            {
                EditorGUIUtility.AddCursorRect(position2, MouseCursor.Arrow);
            }

            if (Event.current.type == EventType.MouseUp && position2.Contains(Event.current.mousePosition))
            {
                text = string.Empty;
                GUIUtility.keyboardControl = 0;
                GUI.changed = true;
                Event.current.Use();
            }

            GUI.SetNextControlName(this._controlName);
            text = EditorGUI.TextField(position1, text, showWithPopupArrow ? EditorSkinUtility.Styles.Custom.ToolbarSearch.textFieldPopup : EditorSkinUtility.Styles.Custom.ToolbarSearch.textField);

            GUI.Button(position2, GUIContent.none, text != string.Empty ? EditorSkinUtility.Styles.Custom.ToolbarSearch.cancelButton : EditorSkinUtility.Styles.Custom.ToolbarSearch.cancelButtonEmpty);

            return text;
        }
        #endregion
    }
}
