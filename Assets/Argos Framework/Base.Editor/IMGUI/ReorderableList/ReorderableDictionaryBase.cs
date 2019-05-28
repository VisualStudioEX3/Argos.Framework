using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.IMGUI
{
    /// <summary>
    /// Base class to implement ReorderableLists with Dictionary behaviour.
    /// </summary>
    public abstract class ReorderableDictionaryBase : ReorderableListBase, IDisposable
    {
        #region Constructors
        public ReorderableDictionaryBase(SerializedProperty elements, bool isDraggable = true, bool displayHeader = true, ReorderableListAddButtonType displayAddButton = ReorderableListAddButtonType.Default, bool displayRemoveButton = true) :
            base(elements, isDraggable, displayHeader, displayAddButton, displayRemoveButton)
        {
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
                SerializedProperty element = this.Elements.GetArrayElementAtIndex(i);
                if (element.hasChildren)
                {
                    string elementName = element.GetArrayElementAtIndex(0).stringValue;

                    if (!string.IsNullOrEmpty(elementName))
                    {
                        if (name.Equals(elementName))
                        {
                            index = i;
                            return true;
                        }
                    }
                    else
                    {
                        // TODO: Error, the first element must be a string value.
                        throw new Exception();
                    }
                }
                else
                {
                    // TODO: Error, the element must be a structure or class value with public serialized properties.
                    throw new Exception();
                }
            }

            index = -1;

            return false;
        } 
        #endregion

        #region Event listeners
        public sealed override void OnAddElement()
        {
            string name = string.Empty; // TODO: Implement input box dialog for fill data like this.

            if (true)
            {
                int matchIndex;
                if (this.IsNameExists(name, out matchIndex))
                {
                    // TODO: Add new element.
                }
            }
        } 
        #endregion
    }
}
