using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework.FileSystem
{
    #region Enums
    /// <summary>
    /// Serialization modes.
    /// </summary>
    public enum SerializationModes
    {
        Json,
        Binary
    }

    /// <summary>
    /// File operation error codes.
    /// </summary>
    public enum FileOperationErrorCodes
    {
        NO_DATA_TO_LOAD,
        DATA_CORRUPTED,
        NOT_ENOUGH_SPACE,
        NOT_MOUNTED,
        CLOUD_STORAGE_UNAVAILABLE,
        GENERIC_ERROR
    }
    #endregion

    /// <summary>
    /// Base class for implementing file slots in each platform.
    /// </summary>
    public abstract class FileSlot : ScriptableObject
    {
        #region Serialized fields
        /// <summary>
        /// File title.
        /// </summary>
        [SerializeField]
        string _title = "New file slot";

        /// <summary>
        /// File description.
        /// </summary>
        [SerializeField]
        string _description;

        /// <summary>
        /// Creation date and time.
        /// </summary>
        [SerializeField]
        DateTime _creationDateTime = DateTime.MinValue;

        /// <summary>
        /// Last write access date and time.
        /// </summary>
        [SerializeField]
        DateTime _lastWriteDateTime = DateTime.MinValue;

        /// <summary>
        /// Last read access date and time.
        /// </summary>
        [SerializeField]
        DateTime _lastReadDateTime = DateTime.MinValue;

        /// <summary>
        /// Serialization mode.
        /// </summary>
        [SerializeField]
        SerializationModes _serializeMode = SerializationModes.Json;
        #endregion

        #region Properties
        /// <summary>
        /// File title.
        /// </summary>
        public string Title { get { return this._title; } }

        /// <summary>
        /// File description.
        /// </summary>
        public string Description { get { return this._description; } }

        /// <summary>
        /// Creation date and time.
        /// </summary>
        public DateTime CreationDateTime { get { return this._creationDateTime; } }

        /// <summary>
        /// Last write access date and time.
        /// </summary>
        public DateTime LastWriteDateTime { get { return this._creationDateTime; } }

        /// <summary>
        /// Last read access date and time.
        /// </summary>
        public DateTime LastReadDateTime { get { return this._creationDateTime; } }

        /// <summary>
        /// File size.
        /// </summary>
        public int Size { get { return 0; } }
        #endregion
    }

#if UNITY_EDITOR
    /// <summary>
    /// Base editor class implementation for FileSlots.
    /// </summary>
    [CustomEditor(typeof(FileSlot))]
    public abstract class FileSlotEditor : ArgosCustomEditorBase
    {
        #region Internal vars
        FileSlot _target;
        SerializedProperty _title, _details, _creationDateTime, _lastWriteDateTime, _lastReadDateTime, _serializeMode;
        bool _previewFoldOut; 
        #endregion
        
        private void OnEnable()
        {
            this._target = (FileSlot)target;

            this._title = this.serializedObject.FindProperty("_title");
            this._details = this.serializedObject.FindProperty("_description");
            this._creationDateTime = this.serializedObject.FindProperty("_creationDateTime");
            this._lastReadDateTime = this.serializedObject.FindProperty("_lastReadDateTime");
            this._lastWriteDateTime = this.serializedObject.FindProperty("_lastWriteDateTime");
            this._serializeMode = this.serializedObject.FindProperty("_serializeMode");

            this.HeaderTitle = "File Slot";
        }

        //protected override void OnHeaderGUI()
        //{
        //    base.OnHeaderGUI();
        //    var rect = new Rect(44f, 24f, EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);
        //    EditorGUI.LabelField(rect, target.GetType().Name, EditorStyles.miniLabel);
        //}

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.LabelField("Main settings:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.PropertyField(this._title);
                EditorGUILayout.PropertyField(this._details);
                EditorGUILayout.PropertyField(this._serializeMode);
            }
            EditorGUILayout.EndVertical();

            this.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("File information:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                this.DrawTextField("Size", $"{this._target.Size} Bytes");
                this.DrawTextField("Creation", this.FormatDateTime(this._target.CreationDateTime));
                this.DrawTextField("Last read access", this.FormatDateTime(this._target.LastReadDateTime));
                this.DrawTextField("Last write access", this.FormatDateTime(this._target.LastWriteDateTime));
            }
            EditorGUILayout.EndVertical();
        }

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
    }
#endif
}