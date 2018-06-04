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
    /// File Slot type.
    /// </summary>
    public enum FileSlotType
    {
        /// <summary>
        /// Dictionary type, like the Unity PlayerPrefs.
        /// </summary>
        Dictionary,
        /// <summary>
        /// Custom data. Use this for store custom data structures.
        /// </summary>
        CustomData
    }

    /// <summary>
    /// Serialization modes for custom data.
    /// </summary>
    public enum FileSlotSerializationMode
    {
        /// <summary>
        /// Text data in JSON format.
        /// </summary>
        JSON,
        /// <summary>
        /// Binary format.
        /// </summary>
        Binary
    }

    /// <summary>
    /// Error codes.
    /// </summary>
    public enum FileSlotErrorCodes
    {
        NO_DATA_TO_LOAD,
        DATA_CORRUPTED,
        NOT_ENOUGH_SPACE,
        NOT_MOUNTED,
        USER_REQUIRED,
        CLOUD_STORAGE_UNAVAILABLE,
        GENERIC_ERROR
    }
    #endregion

    /// <summary>
    /// Base class for implementing file slots in each platform.
    /// </summary>
    public abstract class FileSlot : ScriptableObject
    {
        #region Internal vars
        FileDictionary _dictionary;
        byte[] _binaryBuffer;
        string _jsonBuffer;
        #endregion

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
        /// File Slot type.
        /// </summary>
        [SerializeField]
        FileSlotType _type = FileSlotType.Dictionary;

        /// <summary>
        /// Serialization mode.
        /// </summary>
        [SerializeField]
        FileSlotSerializationMode _serializeMode = FileSlotSerializationMode.JSON;
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
        /// File Slot type.
        /// </summary>
        public FileSlotType Type { get { return this._type; } }

        /// <summary>
        /// Serialization mode (only for custom data).
        /// </summary>
        public FileSlotSerializationMode SerializationMode { get { return this._serializeMode; } }

        /// <summary>
        /// File size.
        /// </summary>
        public int Size { get { return 0; } }

        /// <summary>
        /// Dictionary of values.
        /// </summary>
        public FileDictionary Dictionary
        {
            get
            {
                if (this._type == FileSlotType.Dictionary)
                {
                    return this._dictionary;
                }
                else
                {
                    throw new InvalidOperationException("The File Slot not is setup as dictionary.");
                }
            }
        }
        #endregion

        #region Events
        private void OnEnable()
        {
            if (this._dictionary == null)
            {
                this._dictionary = new FileDictionary();
            }
        } 
        #endregion

        #region Event delegates
        public Action OnSaveSuccess;
        public Action<FileSlotErrorCodes> OnSaveFailed;
        public Action OnLoadSuccess;
        public Action<FileSlotErrorCodes> OnLoadFailed;
        public Action OnDeleteSuccess;
        public Action<FileSlotErrorCodes> OnDeleteFailed;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Serialize custom data.
        /// </summary>
        /// <param name="data">Object to serialize.</param>
        public void Serialize(object data)
        {
            if (this._type == FileSlotType.CustomData)
            {
                if (this._serializeMode == FileSlotSerializationMode.JSON)
                {
                    this._jsonBuffer = JsonUtility.ToJson(data, true);
                }
                else
                {
                    this._binaryBuffer = BinarySerializer.Serialize(data);
                }
            }
            else
            {
                throw new InvalidOperationException("The File Slot not is setup as custom data.");
            }
        }

        /// <summary>
        /// Deserialize custom data.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize.</typeparam>
        /// <returns>Returns a copy of the serialized object.</returns>
        public T Deserialize<T>()
        {
            if (this._type == FileSlotType.CustomData)
            {
                if (this._serializeMode == FileSlotSerializationMode.JSON)
                {
                    return JsonUtility.FromJson<T>(this._jsonBuffer);
                }
                else
                {
                    return BinarySerializer.Deserialize<T>(this._binaryBuffer);
                }
            }
            else
            {
                throw new InvalidOperationException("The File Slot not is setup as custom data.");
            }
        }

        /// <summary>
        /// Save data to file.
        /// </summary>
        public virtual void Save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load data from file.
        /// </summary>
        public virtual void Load()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        public virtual void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Exists file.
        /// </summary>
        /// <returns>Return true if the file exists.</returns>
        public virtual bool Exists()
        {
            throw new NotImplementedException();
        }
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
        SerializedProperty _title, _details, _creationDateTime, _lastWriteDateTime, _lastReadDateTime, _type, _serializeMode;
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
            this._type = this.serializedObject.FindProperty("_type");
            this._serializeMode = this.serializedObject.FindProperty("_serializeMode");

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
                if (this._target.Type == FileSlotType.CustomData)
                {
                    EditorGUILayout.PropertyField(this._serializeMode);
                }
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