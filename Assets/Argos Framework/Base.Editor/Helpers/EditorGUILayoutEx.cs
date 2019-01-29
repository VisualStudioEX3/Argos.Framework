using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// EditorGUILayout extensions.
    /// </summary>
    public static class EditorGUILayoutEx
    {
        /// <summary>
        /// Draws a section with a header title and content.
        /// </summary>
        /// <param name="title">Section title.</param>
        /// <param name="content">Method that define the content o the section.</param>
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
                content?.Invoke();
                EditorGUI.indentLevel--;
            }

            return state;
        }

        /// <summary>
        /// Draws a labeled text field with a button to shows the system open file dialog.
        /// </summary>
        /// <param name="label">Field label.</param>
        /// <param name="path">Path for the file location shows in the text field.</param>
        /// <param name="dialogTitle">Title for the open file dialog.</param>
        /// <param name="dialogType">Dialog behaviour type.</param>
        /// <param name="fileExtension">File extension for the open file dialog.</param>
        /// <param name="directory">Initial directory to target the open file dialog. By default is empty.</param>
        /// <param name="defaultName">Default filename. Use only for SaveFile type. By default is empty.</param>
        /// <param name="message">Message displayed in dialog. Only for SaveFileInProject. By default is empty.</param>
        /// <returns>Returns the selected file path or the latest file path when the user cancels the open file dialog.</returns>
        public static string FileField(string label, string path, string dialogTitle, FileDialogTypes dialogType, string fileExtension, string directory = "", string defaultName = "", string message = "")
        {
            EditorGUILayout.BeginHorizontal();
            {
                path = EditorGUILayout.TextField(label, path);
                if (GUILayout.Button("...", EditorStyles.miniButton, GUILayout.Width(30f)))
                {
                    string newPath = string.Empty;

                    switch (dialogType)
                    {
                        case FileDialogTypes.OpenFile:
                            newPath = EditorUtility.OpenFilePanel(dialogTitle, directory, fileExtension);
                            break;
                        case FileDialogTypes.SaveFile:
                            newPath = EditorUtility.SaveFilePanel(dialogTitle, directory, defaultName, fileExtension);
                            break;
                        case FileDialogTypes.SaveFileInProject:
                            newPath = EditorUtility.SaveFilePanelInProject(dialogTitle, directory, fileExtension, message);
                            break;
                    }

                    if (!string.IsNullOrEmpty(newPath))
                    {
                        path = newPath;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return path;
        }

        /// <summary>
        /// Draws a labeled text field with a button to shows the system open folder dialog.
        /// </summary>
        /// <param name="label">Field label.</param>
        /// <param name="path">Path for the folder location in text field.</param>
        /// <param name="dialogTitle">Title for the open folder dialog.</param>
        /// <param name="dialogType">Dialog behaviour type.</param>
        /// <param name="folder">Initial directory to target the open folder dialog. By default is empty.</param>
        /// <param name="defaultName">Default folder name. By default empty.</param>
        /// <returns>Returns the selected folder path or the latest folder path when the user cancels the open folder dialog.</returns>
        public static string FolderField(string label, string path, string dialogTitle, FolderDialogTypes dialogType, string folder = "", string defaultName = "")
        {
            EditorGUILayout.BeginHorizontal();
            {
                path = EditorGUILayout.TextField(label, path);
                if (GUILayout.Button("...", EditorStyles.miniButton, GUILayout.Width(30f)))
                {
                    string newPath = string.Empty;
                    switch (dialogType)
                    {
                        case FolderDialogTypes.OpenFolder:
                            newPath = EditorUtility.OpenFolderPanel(label, folder, defaultName);
                            break;
                        case FolderDialogTypes.SaveFolder:
                            newPath = EditorUtility.SaveFolderPanel(label, folder, defaultName);
                            break;
                    }
                    
                    if (!string.IsNullOrEmpty(newPath))
                    {
                        path = newPath;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return path;
        }

        /// <summary>
        /// Draws a labeled multi-line text field, with the label field in the left, and the text area in the field region, like the standard fields.
        /// </summary>
        /// <param name="label">Field label.</param>
        /// <param name="text">Content of the text area.</param>
        /// <param name="lines">The lines to show in the field at same time.</param>
        /// <returns>Returns the current content of the text area.</returns>
        public static string LabeledTextArea(string label, string text, int lines)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(label);

                var currentIndent = EditorGUI.indentLevel;
                {
                    EditorGUI.indentLevel = 0;

                    GUISkin editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

                    Rect rect = EditorGUILayout.GetControlRect(true, (editorSkin.textArea.lineHeight * lines) + editorSkin.textArea.margin.vertical);
                    float delta = 7f; // editorSkin.textArea.padding.horizontal;
                    rect.x -= delta;
                    rect.width += delta;

                    text = EditorGUI.TextArea(rect, text);
                }
                EditorGUI.indentLevel = currentIndent;
            }
            EditorGUILayout.EndHorizontal();

            return text;
        }
    }
}