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
        public ReorderableDictionaryBase(SerializedProperty elements, bool isDraggable = true, bool displayHeader = true, bool displayAddButton = true, bool displayRemoveButton = true, string inputPopupWindowLabel = "") :
            base(elements, isDraggable, displayHeader, displayAddButton ? ReorderableListAddButtonType.Dropdown : ReorderableListAddButtonType.None, displayRemoveButton)
        {
            this._inputPopupWindow = new InputPopupWindow<string>(string.IsNullOrEmpty(inputPopupWindowLabel) ? ReorderableDictionaryBase.POPUP_WINDOW_DEFAULT_FIELD_CAPTION_LABEL : inputPopupWindowLabel, this.OnInputPopupAccept);
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Check if the name exists in the serialized list.
        /// </summary>
        /// <param name="name">Name to check.</param>
        /// <param name="index">Return the index of the element with the same name. Return -1 when not element match the name parameter.</param>
        /// <param name="skipIndexElement">Skip check on this index element. By default is -1 and no skip any index.</param>
        /// <returns>Returns true if the name exists in the list.</returns>
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

        /// <summary>
        /// Get the first string property of the Serialized Property.
        /// </summary>
        /// <param name="element">Serialized Property to check.</param>
        /// <returns>Return the copy of the string property.</returns>
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
                throw new FieldAccessException($"{this.GetType().Name}: The property or their first child must be a string type.");
            }
        }

        /// <summary>
        /// Check for changes on current GUI element.
        /// </summary>
        /// <returns>Return true when any property of the GUI element has changed.</returns>
        bool CheckForChanges()
        {
            if (GUI.changed)
            {
                GUI.changed = false; // At first positive, disabled the state to avoid detect a chain of changes in entire property hierarchy.
                return true;
            }

            return false;
        }

        void ShowDuplicatedErrorMessageBox(string duplicatedName, int index)
        {
            EditorUtility.DisplayDialog("Duplicated element", $"An element with name \"{duplicatedName}\" already exists in the list (Element index {index})", "Ok");
        }

        /// <summary>
        /// Check if element key value is duplicated.
        /// </summary>
        /// <param name="element"><see cref="SerializedProperty"/> element procesed on <see cref="OnElementGUI(Rect, SerializedProperty, int, bool, bool)"/> event.</param>
        /// <param name="index">Index element procesed on <see cref="OnElementGUI(Rect, SerializedProperty, int, bool, bool)"/> event.</param>
        /// <remarks>If you implement a custom overload for <see cref="OnElementGUI(Rect, SerializedProperty, int, bool, bool)"/> event you must call this method after render all GUI element for check the Key value changes (to avoid duplicates).</remarks>
        public void CheckElementKeyValue(SerializedProperty element, int index)
        {
            if (this.CheckForChanges())
            {
                string newName = this.GetStringProperty(element).stringValue;
                bool isEmptyName = newName.IsNullOrEmptyOrWhiteSpace();
                int matchIndex = -1;

                if ((!isEmptyName && this.IsNameExists(newName, out matchIndex, index)) || isEmptyName)
                {
                    if (!isEmptyName)
                    {
                        this.ShowDuplicatedErrorMessageBox(newName, matchIndex);
                    }

                    EditorGUIUtility.ExitGUI();
                }
            }
        }
        #endregion

        #region Event listeners
        public override void OnAddDropdown(Rect buttonRect)
        {
            this._inputPopupWindow.Show(buttonRect, new Vector2(ReorderableDictionaryBase.POPUP_WINDOW_POSITION_OFFSET_X, ReorderableDictionaryBase.POPUP_WINDOW_POSITION_OFFSET_Y));
        }

        void OnInputPopupAccept(string value)
        {
            if (!value.IsNullOrEmptyOrWhiteSpace())
            {
                int matchIndex;
                if (!this.IsNameExists(value, out matchIndex))
                {
                    this.OnAddNewElement(value);
                }
                else
                {
                    this.ShowDuplicatedErrorMessageBox(value, matchIndex);
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
            this.CheckElementKeyValue(element, index);
        }
        #endregion
    }
}
