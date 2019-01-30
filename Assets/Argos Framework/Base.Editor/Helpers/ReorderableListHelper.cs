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
    /// Helper functions to create ReorderableList controls.
    /// </summary>
    public static class ReorderableListHelper
    {
        #region Constants
        const float HEADER_NONE_HEIGHT = 3f;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Initialize and setup a ReorderableList with non-unique named elements.
        /// </summary>
        /// <param name="editorInstance">Instance of the Editor script hosting the ReorderableList.</param>
        /// <param name="list">ReorderableList instance.</param>
        /// <param name="headerName">Name to shows on the header. If leave blank this field, the header is not drawed.</param>
        /// <param name="property">Name of the property that contain the generic list to used in this ReorderableList.</param>
        /// <param name="prefixName">Optional. Prefix name for the data field.</param>
        /// <param name="isReorderable">The list is reorderable by dragging elements? By default true.</param>
        /// <returns>Return the ready ReorderableList.</returns>
        /// <remarks>The list type must be a struct with a string field name 'Name', to store and manage the name ids, and a custom type field named 'Data'. The Data field is the content of each element in the list.</remarks>
        public static ReorderableList CreateNamedList(Editor editorInstance, ReorderableList list, string headerName, string property, string prefixName = "", bool isReorderable = true)
        {
            const string PROPERTY_NAME = "Name";
            const string PROPERTY_DATA = "Data";

            var ret = new ReorderableList(editorInstance.serializedObject, 
                                          editorInstance.serializedObject.FindProperty(property), 
                                          isReorderable, 
                                          !string.IsNullOrEmpty(headerName), 
                                          true, 
                                          true);

            if (!string.IsNullOrEmpty(headerName))
            {
                ret.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, headerName, EditorStyles.boldLabel);
                };
            }
            else
            {
                ret.headerHeight = ReorderableListHelper.HEADER_NONE_HEIGHT;
            }

            ret.elementHeightCallback = (int index) =>
            {
                SerializedProperty element = ret.serializedProperty.GetArrayElementAtIndex(index);

                return EditorGUI.GetPropertyHeight(element.FindPropertyRelative(PROPERTY_NAME), GUIContent.none) + 
                       EditorGUI.GetPropertyHeight(element.FindPropertyRelative(PROPERTY_DATA), GUIContent.none, true) + 
                       EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing;
            };

            ret.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = ret.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty nameField = element.FindPropertyRelative(PROPERTY_NAME);
                SerializedProperty dataField = element.FindPropertyRelative(PROPERTY_DATA);

                Rect contentBoxRect = rect;
                contentBoxRect.y += 2f;
                contentBoxRect.height -= 4f;

                GUI.Box(contentBoxRect, GUIContent.none, EditorStyles.helpBox);

                Rect nameRect = contentBoxRect;
                nameRect.x += 14f;
                nameRect.y += 4f;
                nameRect.width -= 18f;
                nameRect.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.PropertyField(nameRect, nameField, new GUIContent(nameField.name));

                Rect dataRect = contentBoxRect;
                dataRect.x += 14f;
                dataRect.y += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) + 4f;
                dataRect.width -= 18f;

                EditorGUI.PropertyField(dataRect, 
                                        dataField, 
                                        new GUIContent(string.IsNullOrEmpty(prefixName) ? 
                                                           dataField.name : 
                                                           prefixName), 
                                        true);
            };

            return ret;
        }

        public static ReorderableList CreateSimpleList(Editor editorInstance, ReorderableList list, string headerName, string property, string prefixName = "", bool isReorderable = true)
        {
            var ret = new ReorderableList(editorInstance.serializedObject, 
                                          editorInstance.serializedObject.FindProperty(property), 
                                          isReorderable, 
                                          !string.IsNullOrEmpty(headerName), 
                                          true, 
                                          true);

            if (!string.IsNullOrEmpty(headerName))
            {
                ret.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, headerName, EditorStyles.boldLabel);
                };
            }
            else
            {
                ret.headerHeight = ReorderableListHelper.HEADER_NONE_HEIGHT;
            }

            ret.elementHeightCallback = (int index) =>
            {
                SerializedProperty element = ret.serializedProperty.GetArrayElementAtIndex(index);

                return EditorGUI.GetPropertyHeight(element, GUIContent.none, true) + 
                       EditorGUIUtility.standardVerticalSpacing;
            };

            ret.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = ret.serializedProperty.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(rect, 
                                        element, 
                                        string.IsNullOrEmpty(prefixName) ? 
                                            GUIContent.none : 
                                            new GUIContent(element.name), 
                                        true);
            };

            return ret;
        }
        #endregion
    }
}