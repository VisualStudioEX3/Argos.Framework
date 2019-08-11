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
        public string[] DropDownItems { get; set; }
        #endregion

        #region Events
        public event Action<string> OnSearchTextChange;
        public event Action<int> OnDropDownSelect;
        #endregion

        #region Static members
        static GUIStyle _toolbarSeachTextFieldPopupStyle;
        public static float Height => EditorGUIUtility.singleLineHeight;
        #endregion

        #region Initializers
        [InitializeOnLoadMethod]
        static void Init()
        {
            SearchField._toolbarSeachTextFieldPopupStyle = EditorSkinUtility.Skin.FindStyle("ToolbarSeachTextFieldPopup");
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
            
            Rect dropDownRect = position;
            {
                dropDownRect.width = 30f;
            }

            // TODO: Current solution not working. The second plan is recreate by scracth the SearchField control, divided in 3 parts:
            // The dropdown button (with the left-side SearchField style, the textBox (with the middle part of the SearchField style),
            // and the cancel button (with the cancel button SearchField style).

            if (Event.current.type == EventType.MouseDown)// && dropDownRect.Contains(Event.current.mousePosition))
            {
                Debug.LogWarning("SearchField dropdown button clicked!");
                Event.current.Use();
            }

            EditorGUI.Popup(dropDownRect, 0, this.DropDownItems, SearchField._toolbarSeachTextFieldPopupStyle);
            string searchField = this._searchField.OnToolbarGUI(position, searchString);

            return searchField;
        }

        public string DoLayout(string searchString, bool fullWidth = false)
        {
            Rect position = EditorGUILayout.GetControlRect(!fullWidth);
            return this.Do(position, searchString);
        }
        #endregion
    }
}
