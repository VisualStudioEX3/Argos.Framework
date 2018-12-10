using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace Argos.Framework.FileSystem
{
    /// <summary>
    /// Base editor class implementation for FileSlots.
    /// </summary>
    [CustomEditor(typeof(FileSlotAsset))]
    public sealed class FileSlotAssetEditor : ArgosCustomEditorBase
    {
        #region Internal vars
        FileSlotAsset _baseTarget;
        SerializedProperty _title, _details, _creationDateTime, _lastWriteDateTime, _lastReadDateTime, _type, _serializeMode;
        bool _previewFoldOut;
        ReorderableList _modules;
        #endregion

        #region Events
        public void OnEnable()
        {
            this._baseTarget = (FileSlotAsset)target;

            this._title = this.serializedObject.FindProperty("_title");
            this._details = this.serializedObject.FindProperty("_description");
            this._creationDateTime = this.serializedObject.FindProperty("_creationDateTime");
            this._lastReadDateTime = this.serializedObject.FindProperty("_lastReadDateTime");
            this._lastWriteDateTime = this.serializedObject.FindProperty("_lastWriteDateTime");
            this._type = this.serializedObject.FindProperty("_type");
            this._serializeMode = this.serializedObject.FindProperty("_serializeMode");

            this._modules = EditorHelper.CreateSimpleReorderableList(this, this._modules, string.Empty, "_modules");

            this.HeaderTitle = "File Slot";
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.LabelField("Main settings:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.PropertyField(this._title);
                EditorGUILayout.PropertyField(this._details);
                EditorGUILayout.PropertyField(this._type);
                EditorGUILayout.PropertyField(this._serializeMode);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.LabelField("File information:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                this.DrawTextField("Size", $"{this._baseTarget.Size} Bytes");
                this.DrawTextField("Creation", this.FormatDateTime(this._baseTarget.CreationDateTime));
                this.DrawTextField("Last read access", this.FormatDateTime(this._baseTarget.LastReadDateTime));
                this.DrawTextField("Last write access", this.FormatDateTime(this._baseTarget.LastWriteDateTime));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.LabelField("Platform modules:", EditorStyles.boldLabel);
            //EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            //{
            //    EditorGUILayout.Space();
            //}
            //EditorGUILayout.EndVertical();
            this._modules.DoLayoutList();

            this.serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Methods & Functions
        string FormatDateTime(DateTime dateTime)
        {
            return dateTime == DateTime.MinValue ? "None" : $"{dateTime.ToLongDateString()} - {dateTime.ToLongTimeString()}";
        }

        void DrawTextField(string label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(label);
                EditorGUILayout.SelectableLabel(value, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    } 
}