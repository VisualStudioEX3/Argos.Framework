using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;
using Argos.Framework.IMGUI;

namespace Argos.Framework.Input
{
    public sealed class InputMapDictionaryControl : ReorderableDictionaryBase
    {
        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="elements">Serialized Property reference to element list.</param>
        /// <param name="headerCaption">Optional header caption title. Leave empty for avoid draw header.</param>
        /// <param name="boldHeaderCaption">Draw the header caption with bold style?</param>
        /// <param name="isDraggable">Allow to reorder the elements by mouse dragging?</param>
        /// <param name="displayAddButton">Show the add element button?</param>
        /// <param name="displayRemoveButton">Show the remove element button?</param>
        public InputMapDictionaryControl(SerializedProperty elements) :
            base(elements, true, false, true, true, "New Input Map name")
        {
        }
        #endregion

        #region Event listeners
        public override void OnElementGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
        {
            Rect controlRect = EditorGUI.PrefixLabel(rect, new GUIContent("Name"));

            Rect nameFieldRect = controlRect;
            nameFieldRect.width *= 0.5f;
            EditorGUI.PropertyField(nameFieldRect, element.FindPropertyRelative("_key"));

            Rect inputMapRect = nameFieldRect;
            inputMapRect.x += nameFieldRect.width;
            EditorGUI.PropertyField(inputMapRect, element.FindPropertyRelative("_value"));
        }
        #endregion
    }
}