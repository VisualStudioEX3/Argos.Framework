using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [CreateAssetMenu(fileName = "File Slot", menuName = "Argos.Framework/File System/File Slot definition")]
    public sealed class FileSlotAsset : ScriptableObject
    {
        #region Internal vars
        Storage _dictionary;
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

        //[SerializeField]
        //List<MonoBehaviour> _modules;
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
        public int Size { get; private set; }

        /// <summary>
        /// Dictionary of values.
        /// </summary>
        public Storage Data
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
        /// <summary>
        /// Event executed after the custom data is serialized in binary mode.
        /// </summary>
        /// <param name="buffer">Byte array with the serialized data (in/out param).</param>
        /// <remarks>Usefull for encrypt the binary result.</remarks>
        public BinarySerializer.BinarySerializationHandler OnBinaryDataSerialized;

        /// <summary>
        /// Event executed before the custom data is deserialized in binary mode.
        /// </summary>
        /// <param name="buffer">Byte array with the serialized data (in/out param)</param>
        public BinarySerializer.BinarySerializationHandler OnBinaryDataDeserializing;

        private void OnEnable()
        {
            if (this._dictionary == null)
            {
                this._dictionary = new Storage();
            }

            //if (this._modules == null)
            //{
            //    this._modules = new List<SceneAsset>();
            //}
        }
        #endregion

        #region Event delegates
        /// <summary>
        /// Event for notify the save success operation.
        /// </summary>
        public event Action OnSaveSuccess;

        /// <summary>
        /// Event for notify the save failed operation.
        /// </summary>
        public event Action<FileSlotErrorCodes> OnSaveFailed;

        /// <summary>
        /// Event for notify the load success operation.
        /// </summary>
        public event Action OnLoadSuccess;

        /// <summary>
        /// Event for notify the load failed operation.
        /// </summary>
        public event Action<FileSlotErrorCodes> OnLoadFailed;

        /// <summary>
        /// Event for notify the delete success operation.
        /// </summary>
        public event Action OnDeleteSuccess;

        /// <summary>
        /// Event for notify the delete failed operation.
        /// </summary>
        public Action<FileSlotErrorCodes> OnDeleteFailed;
        #endregion

        #region Methods & Functions
        void Serialize(object data)
        {
            if (this._serializeMode == FileSlotSerializationMode.JSON)
            {
                this._jsonBuffer = JsonUtility.ToJson(data, true);
            }
            else
            {
                this._binaryBuffer = BinarySerializer.Serialize(data, this.OnBinaryDataSerialized);
            }
        }

        T Deserialize<T>()
        {
            if (this._serializeMode == FileSlotSerializationMode.JSON)
            {
                return JsonUtility.FromJson<T>(this._jsonBuffer);
            }
            else
            {
                return BinarySerializer.Deserialize<T>(this._binaryBuffer, this.OnBinaryDataDeserializing);
            }
        }

        /// <summary>
        /// Save data to file.
        /// </summary>
        public void Save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load data from file.
        /// </summary>
        public void Load()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        public void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Exists file.
        /// </summary>
        /// <returns>Return true if the file exists.</returns>
        public bool Exists()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}