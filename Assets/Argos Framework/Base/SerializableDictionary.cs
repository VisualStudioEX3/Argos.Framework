using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Serializable dictionary, specifically designed to use with inspectors and read-only operations in runtime.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    [Serializable]
    public sealed class SerializableDictionary<TKey, TValue> : IEnumerable<TValue>
    {
        #region Internal vars
        IReadOnlyDictionary<TKey, TValue> _internalDictionary;
        #endregion

        #region Inspector fields
        [SerializeField]
        SerializableKeyValuePair<TKey, TValue>[] _elements;
        #endregion

        #region Properties
        string InstanceClassName => $"{nameof(SerializableDictionary<TKey, TValue>)}";

        /// <summary>
        /// Number of elements in dictionary.
        /// </summary>
        public int Count { get { return Application.isPlaying ? this._internalDictionary.Count : this._elements.Length; } }

        /// <summary>
        /// Read-only access to dictiorary element.
        /// </summary>
        /// <param name="key">Key value to search in dictionary.</param>
        /// <returns>Returns the value paired with key, if exists.</returns>
        public TValue this[TKey key]
        {
            get
            {
                if (this.Count > 0)
                {
                    if (this.ContainsKey(key))
                    {
                        return Application.isPlaying ? this._internalDictionary[key] : this._elements.Where(e => e.key.Equals(key)).FirstOrDefault().value;
                    }
                    else
                    {
                        throw new KeyNotFoundException($"{this.InstanceClassName}: The key \"{key}\" was not found!");
                    }
                }
                else
                {
                    throw new Exception($"{this.InstanceClassName}: The map list is empty!");
                }
            }
        }

        /// <summary>
        /// Returns a read-only key collection of this dictionary.
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                if (Application.isPlaying)
                {
                    return this._internalDictionary.Keys;
                }
                else
                {
                    return this._elements.Select(e => e.key);
                }
            }
        }

        /// <summary>
        /// Returns a read-only value colleciton of this dictionary.
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                if (Application.isPlaying)
                {
                    return this._internalDictionary.Values;
                }
                else
                {
                    return this._elements.Select(e => e.value);
                }
            }
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Search for a key in dictionary.
        /// </summary>
        /// <param name="key">Key value to search.</param>
        /// <returns>Returns true if the key exists in dictionary.</returns>
        public bool ContainsKey(TKey key)
        {
            if (Application.isPlaying)
            {
                return this._internalDictionary.ContainsKey(key); 
            }
            else
            {
                return this.Keys.Contains(key);
            }
        }

        /// <summary>
        /// Generate the <see cref="Dictionary{TKey, TValue}"/> representation of this <see cref="SerializableDictionary{TKey, TValue}"/> for works in runtime.
        /// </summary>
        public void GenerateRuntimeDictionary()
        {
            if (this._elements.Length > 0)
            {
                var dic = new Dictionary<TKey, TValue>();

                foreach (var map in this._elements)
                {
                    if (this._internalDictionary.ContainsKey(map.key))
                    {
                        Logger.Log($"{this.InstanceClassName}: The key \"{map.key}\" already exists in the dictionary! Skip to add to dictionary.", LogLevel.Error);
                    }
                    else
                    {
                        dic.Add(map.key, map.value);
                    }
                }

                this._internalDictionary = dic;
            }
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return _internalDictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalDictionary.Values.GetEnumerator();
        }
        #endregion
    }
}
