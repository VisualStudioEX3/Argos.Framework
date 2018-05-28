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
        XML,
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

    public abstract class FileSlot : ScriptableObject
    {
        #region Serialized fields
        /// <summary>
        /// File title.
        /// </summary>
        [SerializeField]
        string _title = "New file slot";

        /// <summary>
        /// File details.
        /// </summary>
        [SerializeField]
        string _details;

        /// <summary>
        /// Creation date and time.
        /// </summary>
        [SerializeField]
        DateTime _creationDateTime;

        /// <summary>
        /// Last write access date and time.
        /// </summary>
        [SerializeField]
        DateTime _lastWriteDateTime;

        /// <summary>
        /// Last read access date and time.
        /// </summary>
        [SerializeField]
        DateTime _lastReadDateTime;

        /// <summary>
        /// Slot icon.
        /// </summary>
        [SerializeField]
        Texture2D _icon;

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
        /// File details.
        /// </summary>
        public string Details { get { return this._details; } }

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
        /// Slot icon.
        /// </summary>
        public Texture2D Icon { get { return this._icon; } }

        /// <summary>
        /// File size.
        /// </summary>
        public int Size { get { return 0; } }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FileSlot))]
    public abstract class FileSlotEditor : Editor
    {
        #region Internal vars
        FileSlot _target;
        SerializedProperty _title, _details, _creationDateTime, _lastWriteDateTime, _lastReadDateTime, _icon, _serializeMode;
        bool _previewFoldOut; 
        #endregion

        private void OnEnable()
        {
            this._target = (FileSlot)target;

            this._title = this.serializedObject.FindProperty("_title");
            this._details = this.serializedObject.FindProperty("_details");
            this._creationDateTime = this.serializedObject.FindProperty("_creationDateTime");
            this._lastReadDateTime = this.serializedObject.FindProperty("_lastReadDateTime");
            this._lastWriteDateTime = this.serializedObject.FindProperty("_lastWriteDateTime");
            this._icon = this.serializedObject.FindProperty("_icon");
            this._serializeMode = this.serializedObject.FindProperty("_serializeMode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("File Slot", EditorStyles.centeredGreyMiniLabel);

            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this._title);
            EditorGUILayout.PropertyField(this._details);
            EditorGUILayout.PropertyField(this._icon);

            if (this._target.Icon != null)
            {
                this._previewFoldOut = EditorGUILayout.Foldout(this._previewFoldOut, new GUIContent("Icon preview"));

                if (this._previewFoldOut)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical("helpbox", GUILayout.MaxWidth(108f));
                        {
                            Rect preview = EditorGUILayout.GetControlRect(false, 100f);
                            preview.width = 100f;

                            EditorGUI.DrawPreviewTexture(preview, this._target.Icon);
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical();
                        {
                            var texture = this._target.Icon;
                            EditorGUILayout.LabelField($"Dimensions: {texture.width} x {texture.height}\nType: {texture.dimension}\nFormat: {texture.format}\nFilter: {texture.filterMode}\nAnisotropic level: {texture.anisoLevel}", EditorStyles.wordWrappedLabel);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                } 
            }

            EditorGUILayout.PropertyField(this._serializeMode);

            this.serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}