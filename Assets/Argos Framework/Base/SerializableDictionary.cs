using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Serializable dictionary base class, specifically designed to use with inspectors and read-only operations in runtime.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        #region Internal vars
        Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
        #endregion

        #region Properties
        string InstanceClassName => $"{nameof(SerializableDictionary<TKey, TValue>)}";

        Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                if (this.IsDirty)
                {
                    this._dictionary.Clear();
                    this.OnSerialize();
                    this.IsDirty = false;
                }

                return this._dictionary;
            }
        }

        /// <summary>
        /// Read-only access to dictiorary element.
        /// </summary>
        /// <param name="key">Key value to search in dictionary.</param>
        /// <returns>Returns the value paired with key, if exists.</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (this.TryGetValue(key, out value))
                {
                    return value;
                }
                throw new KeyNotFoundException($"{this.InstanceClassName}: Key not present in dictionary. ({key})");
            }
        }

        /// <summary>
        /// Returns a read-only key collection of this dictionary.
        /// </summary>
        public IEnumerable<TKey> Keys => this.Dictionary.Keys;

        /// <summary>
        /// Returns a read-only value colleciton of this dictionary.
        /// </summary>
        public IEnumerable<TValue> Values => this.Dictionary.Values;

        /// <summary>
        /// Number of elements in dictionary.
        /// </summary>
        public int Count => this.Dictionary.Count;

        /// <summary>
        /// Is the dictionary dirty? If is true, the next access to internal dictionary raise <see cref="OnSerialize"/> event to refresh any changes from serialized data to dictionary.
        /// </summary>
        public bool IsDirty { get; private set; }
        #endregion

        #region Constructors
        public SerializableDictionary()
        {
            this.IsDirty = true;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += (UnityEditor.PlayModeStateChange state) =>
            {
                if (state == UnityEditor.PlayModeStateChange.EnteredPlayMode ||
                    state == UnityEditor.PlayModeStateChange.EnteredEditMode)
                {
                    this.IsDirty = true;
                }
            };
#endif
        } 
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Add new value to dictionary.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <remarks>This method only can be called from <see cref="OnSerialize"/> event when is called by dictionary itself.</remarks>
        public void Add(TKey key, TValue value)
        {
            if (this.IsDirty)
            {
                try
                {
                    this._dictionary.Add(key, value);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"{this.InstanceClassName}: {ex.Message}");
                }
            }
            else
            {
                throw new InvalidOperationException($"{this.InstanceClassName}: This method is only available from {nameof(this.OnSerialize)}() event when the dictionary is dirty.");
            }
        }

        /// <summary>
        /// Add new value to dictionary.
        /// </summary>
        /// <param name="value">KeyValue-Pair.</param>
        /// <remarks>This method only can be called from <see cref="OnSerialize"/> event when is called by dictionary itself.</remarks>
        public void Add(KeyValuePair<TKey, TValue> value)
        {
            this.Add(value.Key, value.Value);
        }

        /// <summary>
        /// Mark dictionary as dirty to call <see cref="OnSerialize"/> event in the next access to dictionary, only in Edit mode.
        /// </summary>
        public void SetDirty()
        {
            this.IsDirty = !Application.isPlaying;
        }

        /// <summary>
        /// Search for a key in dictionary.
        /// </summary>
        /// <param name="key">Key value to search.</param>
        /// <returns>Returns true if the key exists in dictionary.</returns>
        public bool ContainsKey(TKey key)
        {
            return this.Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.Dictionary.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }
        #endregion

        #region Event listeners
        /// <summary>
        /// Use this event to add serialized data to the internal dictionary.
        /// </summary>
        public abstract void OnSerialize();
        #endregion
    }

}
