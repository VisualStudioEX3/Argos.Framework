#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Argos.Framework
{
    /// <summary>
    /// Helper functions to used in Editor scripts.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// Initialize and setup a ReorderableList with non unique named elements.
        /// </summary>
        /// <param name="editorInstance">Instance of the Editor script hosting the ReorderableList.</param>
        /// <param name="list">ReorderableList instance.</param>
        /// <param name="headerName">Name to shows on the header.</param>
        /// <param name="property">Name of the property that contain the generic list to used in this ReorderableList.</param>
        /// <param name="prefixName">Optional. Prefix name for the data field.</param>
        /// <returns>Return the ready ReorderableList.</returns>
        /// <remarks>The list type must be an struct with a string field name 'Name', to store and manage the name ids, and a custom type field named 'Data'. The Data field is the content of each element in the list.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ReorderableList CreateNamedList(Editor editorInstance, ReorderableList list, string headerName, string property, string prefixName = "")
        {
            const string PROPERTY_NAME = "Name";
            const string PROPERTY_DATA = "Data";

            var ret = new ReorderableList(editorInstance.serializedObject, editorInstance.serializedObject.FindProperty(property), true, true, true, true);

            ret.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, headerName);
            };

            ret.elementHeightCallback = (int index) =>
            {
                var element = ret.serializedProperty.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element.FindPropertyRelative(PROPERTY_NAME), GUIContent.none) + EditorGUI.GetPropertyHeight(element.FindPropertyRelative(PROPERTY_DATA), GUIContent.none, true) + EditorGUIUtility.singleLineHeight;
            };

            ret.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2f;
                rect.height -= 4f;
                EditorGUI.HelpBox(rect, string.Empty, MessageType.None);

                rect.x += 14f;
                rect.y += 4f;
                rect.width -= 18f;
                rect.height = EditorGUIUtility.singleLineHeight;

                var element = ret.serializedProperty.GetArrayElementAtIndex(index);

                var nameField = element.FindPropertyRelative(PROPERTY_NAME);
                EditorGUI.PropertyField(rect, nameField, new GUIContent(nameField.name));

                var dataField = element.FindPropertyRelative(PROPERTY_DATA);
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(rect, dataField, new GUIContent(string.IsNullOrEmpty(prefixName) ? dataField.name : prefixName), true);
            };

            return ret;
        }
    } 
}
#endif