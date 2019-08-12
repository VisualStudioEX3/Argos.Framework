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
        #endregion

        #region Properties
        public int DropDownSelection { get; private set; }
        public string[] DropDownItems { get; set; }
        #endregion

        #region Events
        public event Action<string> OnSearchTextChange;
        public event Action<int> OnDropDownSelect;
        #endregion

        #region Static members
        static GUIStyle _toolbarSeachTextFieldPopupStyle;
        static GUIStyle _cancelButtonStyle;
        static GUIStyle _cancelButtonEmptyStyle;
        static GUIStyle _invisibleButtonStyle;
        static GUIStyle _miniLabelDisabled;

        public static float Height => EditorGUIUtility.singleLineHeight;
        #endregion

        #region Initializers
        [InitializeOnLoadMethod]
        static void Init()
        {
            SearchField._toolbarSeachTextFieldPopupStyle = new GUIStyle(EditorSkinUtility.Skin.FindStyle("ToolbarSeachTextFieldPopup"));
            SearchField._cancelButtonStyle = new GUIStyle(EditorSkinUtility.Skin.FindStyle("ToolbarSeachCancelButton"));
            SearchField._cancelButtonEmptyStyle = new GUIStyle(EditorSkinUtility.Skin.FindStyle("ToolbarSeachCancelButtonEmpty"));

            SearchField._invisibleButtonStyle = new GUIStyle(EditorSkinUtility.Skin.FindStyle("InvisibleButton"));

            SearchField._miniLabelDisabled = new GUIStyle(EditorSkinUtility.Skin.FindStyle("MiniLabel"));
            {
                Color color = SearchField._miniLabelDisabled.normal.textColor;
                color.a = 0.5f;
                SearchField._miniLabelDisabled.normal.textColor = color;
            }
        }
        #endregion

        #region Constructors
        public SearchField(params string[] dropDownItems)
        {
            this.DropDownItems = dropDownItems;
            this._searchField = new UnityEditor.IMGUI.Controls.SearchField();
        }
        #endregion

        #region Methods & Functions
        public string Do(Rect position, string searchString)
        {
            position.height = SearchField.Height;

            Rect popupButtonRect = position;
            {
                popupButtonRect.width = 16f;
            }

            int lastSelection = this.DropDownSelection;
            this.DropDownSelection = EditorGUI.Popup(popupButtonRect, this.DropDownSelection, this.DropDownItems, SearchField._invisibleButtonStyle);

            if (lastSelection != this.DropDownSelection)
            {
                this.OnDropDownSelect?.Invoke(this.DropDownSelection);
                lastSelection = this.DropDownSelection;
            }

            string searchFieldText = this._searchField.OnGUI(position, searchString, SearchField._toolbarSeachTextFieldPopupStyle, SearchField._cancelButtonStyle, SearchField._cancelButtonEmptyStyle);

            if (string.IsNullOrEmpty(searchFieldText) && !this._searchField.HasFocus())
            {
                Rect labelRect = position;
                {
                    labelRect.xMin += 14f;
                    labelRect.y--;
                }

                EditorGUI.LabelField(labelRect, this.DropDownItems[this.DropDownSelection], SearchField._miniLabelDisabled);
            }

            if (!string.IsNullOrEmpty(searchFieldText) && !string.IsNullOrEmpty(searchString) && !searchFieldText.Equals(searchString))
            {
                this.OnSearchTextChange?.Invoke(searchFieldText);
            }

            return searchFieldText;
        }

        public string DoLayout(string searchString, bool fullWidth = false)
        {
            Rect position = EditorGUILayout.GetControlRect(!fullWidth);
            return this.Do(position, searchString);
        }
        #endregion
    }
}
