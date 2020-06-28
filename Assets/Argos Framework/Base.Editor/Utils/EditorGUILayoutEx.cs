using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework.Utils;

namespace Argos.Framework
{
    /// <summary>
    /// EditorGUILayout extensions.
    /// </summary>
    public static class EditorGUILayoutEx
    {
        #region Methods & Functions
        /// <summary>
        /// Draws a section with a header title and content.
        /// </summary>
        /// <param name="title">Section title.</param>
        /// <param name="content">Method that define the content of the section.</param>
        public static void Section(string title, Action content)
        {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            {
                content?.Invoke();
            }
            EditorGUILayout.Space();
        }

        /// <summary>
        /// Shows or hide a group of controls.
        /// </summary>
        /// <param name="label">Field label.</param>
        /// <param name="condition">Condition to show or hide the content.</param>
        /// <param name="content">Method that define the content of the group.</param>
        /// <param name="showWhenConditionIsFalse">Shows content when the condition is false.</param>
        /// <returns>Returns the current condition state.</returns>
        public static bool ShowFieldGroup(string label, bool condition, Action content, bool showWhenConditionIsFalse = false)
        {
            bool state = EditorGUILayout.Toggle(label, condition);
            if (state && !showWhenConditionIsFalse || !state && showWhenConditionIsFalse)
            {
                EditorGUI.indentLevel++;
                {
                    content?.Invoke();
                }
                EditorGUI.indentLevel--;
            }

            return state;
        }

        /// <summary>
        /// Draws a section with a header title and content with window style.
        /// </summary>
        /// <param name="title">Section title.</param>
        /// <param name="content">Method that define the content of the section.</param>
        public static void WindowSection(string title, Action content)
        {
            Rect headerRect = EditorGUILayout.BeginVertical(EditorSkinUtility.Styles.window, GUILayout.Height(10f));
            {
                headerRect.x += 8f;
                EditorGUI.LabelField(headerRect, title, EditorStyles.boldLabel);

                EditorGUI.indentLevel++;
                {
                    content?.Invoke();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        } 
        #endregion
    }
}