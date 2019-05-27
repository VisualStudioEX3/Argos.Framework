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
        /// <returns>Returns true if the name exists in the list.</returns>
        /// <remarks>Use this functions when implement your custom add element behaviour.</remarks>
        public bool IsNameExists(string name)
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

            return false;
        } 
        #endregion

        #region Event listeners
        public sealed override void OnAddElement()
        {
            string name = string.Empty; // TODO: Implement input box dialog for fill data like this.

            if (true)
            {
                if (this.IsNameExists(name))
                {
                    // TODO: Add new element.
                }
            }
        } 
        #endregion
    }
}
