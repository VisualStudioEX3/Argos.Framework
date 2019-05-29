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
        #region Internal vars
        string _inputPopupWindowLabel;
        #endregion

        #region Constructors
        public ReorderableDictionaryBase(SerializedProperty elements, bool isDraggable = true, bool displayHeader = true, bool displayAddButton = true, bool displayRemoveButton = true, string inputPopupWindowLabel = "New item name") :
            base(elements, isDraggable, displayHeader, displayAddButton ? ReorderableListAddButtonType.Dropdown : ReorderableListAddButtonType.None, displayRemoveButton)
        {
            this._inputPopupWindowLabel = inputPopupWindowLabel;
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Check if the name exists in the serialized list.
        /// </summary>
        /// <param name="name">Name to check.</param>
        /// <param name="index">Return the index of the element with the same name. Return -1 when not element match the name parameter.</param>
        /// <returns>Returns true if the name exists in the list.</returns>
        /// <remarks>Use this function when implement your custom add element behaviour.</remarks>
        public bool IsNameExists(string name, out int index)
        {
            for (int i = 0; i < this.Elements.arraySize; i++)
            {
                if (name.Equals(this.GetStringProperty(this.Elements.GetArrayElementAtIndex(i).Copy()).stringValue))
                {
                    index = i;
                    return true;
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
        #endregion

        #region Event listeners
        public override void OnAddDropdown(Rect buttonRect)
        {
            InputPopupWindow.ShowInputStringPopup(buttonRect, this._inputPopupWindowLabel, this.OnInputPopupAccept);
        }

        void OnInputPopupAccept(string value)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
            {
                int matchIndex;
                if (!this.IsNameExists(value, out matchIndex))
                {
                    this.OnAddNewElement(value);
                }
                else
                {
                    EditorUtility.DisplayDialog("Duplicated element", $"The name value already exists in the list (index {matchIndex})", "Ok");
                }
            }
        }

        public virtual void OnAddNewElement(string name)
        {
            SerializedProperty newElement = this.GetStringProperty(this.AddNewElement());
            newElement.stringValue = name;
            newElement.serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
