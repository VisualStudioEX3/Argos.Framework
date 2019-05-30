using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.IMGUI
{
    /// <summary>
    /// Base class to implement ReorderableLists with dictionary behaviour.
    /// </summary>
    public abstract class ReorderableDictionaryBase : ReorderableListBase, IDisposable
    {
        #region Constants
        const string POPUP_WINDOW_DEFAULT_FIELD_CAPTION_LABEL = "New element name";
        const float POPUP_WINDOW_POSITION_OFFSET_X = -401f;
        const float POPUP_WINDOW_POSITION_OFFSET_Y = -13f;
        #endregion

        #region Internal vars
        InputPopupWindow<string> _inputPopupWindow;
        #endregion

        #region Constructors
        public ReorderableDictionaryBase(SerializedProperty elements, bool isDraggable = true, bool displayHeader = true, bool displayAddButton = true, bool displayRemoveButton = true, string inputPopupWindowLabel = ReorderableDictionaryBase.POPUP_WINDOW_DEFAULT_FIELD_CAPTION_LABEL) :
            base(elements, isDraggable, displayHeader, displayAddButton ? ReorderableListAddButtonType.Dropdown : ReorderableListAddButtonType.None, displayRemoveButton)
        {
            this._inputPopupWindow = new InputPopupWindow<string>(inputPopupWindowLabel, this.OnInputPopupAccept);
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Check if the name exists in the serialized list.
        /// </summary>
        /// <param name="name">Name to check.</param>
        /// <param name="index">Return the index of the element with the same name. Return -1 when not element match the name parameter.</param>
        /// <returns>Returns true if the name exists in the list.</returns>
        /// <param name="skipIndexElement">Skip check on this index element. By default is -1 and no skip any index.</param>
        /// <remarks>Use this function when implement your custom add element behaviour.</remarks>
        public bool IsNameExists(string name, out int index, int skipIndexElement = -1)
        {
            for (int i = 0; i < this.Elements.arraySize; i++)
            {
                if (skipIndexElement < 0 || i != skipIndexElement)
                {
                    if (name.Equals(this.GetStringProperty(this.Elements.GetArrayElementAtIndex(i).Copy()).stringValue))
                    {
                        index = i;
                        return true;
                    } 
                }
            }

            index = -1;

            return false;
        }

        SerializedProperty GetStringProperty(SerializedProperty element)
        {
            if (element.propertyType != SerializedPropertyType.String && element.hasChildren)
            {
                element.Next(true);
            }

            if (element.propertyType == SerializedPropertyType.String)
            {
                return element.Copy();
            }
            else
            {
                throw new FieldAccessException("ReorderableDictionaryBase: The property or their first child must be a string type.");
            }
        }

        bool CheckForDuplicateName(string name, int skipIndex = -1)
        {
            int matchIndex;
            if (this.IsNameExists(name, out matchIndex, skipIndex))
            {
                EditorUtility.DisplayDialog("Duplicated element", $"The name value already exists in the list (index {matchIndex})", "Ok");
                return true;
            }

            return false;
        }

        bool CheckForChanges()
        {
            if (GUI.changed)
            {
                GUI.changed = false;

                return true;
            }

            return false;
        }
        #endregion

        #region Event listeners
        public override void OnAddDropdown(Rect buttonRect)
        {
            this._inputPopupWindow.Show(buttonRect, new Vector2(ReorderableDictionaryBase.POPUP_WINDOW_POSITION_OFFSET_X, ReorderableDictionaryBase.POPUP_WINDOW_POSITION_OFFSET_Y));
        }

        void OnInputPopupAccept(string value)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
            {
                if (!this.CheckForDuplicateName(value))
                {
                    this.OnAddNewElement(value);
                }
            }
        }

        public virtual void OnAddNewElement(string name)
        {
            SerializedProperty newElement = this.GetStringProperty(this.AddNewElement());
            newElement.stringValue = name;
            newElement.serializedObject.ApplyModifiedProperties();
        }

        public override void OnElementGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
        {
            base.OnElementGUI(rect, element, index, isActive, isFocused);

            if (this.CheckForChanges())
            {
                string newName = this.GetStringProperty(element).stringValue;
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    if (this.CheckForDuplicateName(newName, index))
                    {
                        Undo.PerformUndo();
                    }
                }
            }
        }
        #endregion
    }
}
