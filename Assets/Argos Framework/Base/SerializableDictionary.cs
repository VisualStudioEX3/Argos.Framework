using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Argos.Framework
{
    #region Structs
    /// <summary>
    /// Serializable key value pair base class, specifically designed to use with inspectors and read-only operations in runtime.
    /// </summary>
    /// <typeparam name="TKey">Type of the key field.</typeparam>
    /// <typeparam name="TValue">Type of the value field.</typeparam>
    [Serializable]
    public abstract class SerializableKeyValuePair<TKey, TValue>
    {
        #region Public vars
        public TKey key;
        public TValue value;
        #endregion

        #region Operators
        public static explicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> data)
        {
            return new KeyValuePair<TKey, TValue>(data.key, data.value);
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// Serializable dictionary base class, specifically designed to use with inspectors and read-only operations in runtime.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue> : IEnumerable<TValue>
    {
        #region Internal vars
        IReadOnlyDictionary<TKey, TValue> _internalDictionary;
        #endregion

        #region Classes
        [Serializable]
        public sealed class KeyValuePair : SerializableKeyValuePair<TKey, TValue>
        {
        } 
        #endregion

        #region Inspector fields
#pragma warning disable 649
        [SerializeField]
        protected List<KeyValuePair> _elements;
#pragma warning restore
        #endregion

        #region Constructors
        public SerializableDictionary()
        {
            this._elements = new List<KeyValuePair>();
        } 
        #endregion

        #region Properties
        string InstanceClassName => $"{nameof(SerializableDictionary<TKey, TValue>)}";

        /// <summary>
        /// Number of elements in dictionary.
        /// </summary>
        public int Count { get { return Application.isPlaying ? this._internalDictionary.Count : this._elements.Count; } }

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

    [Serializable]
    public abstract class SerializableDictionary : IReadOnlyDictionary<string, UnityEngine.Object>
    {
        [Serializable]
        public struct SerializableKeyValuePair
        {
            public string key;
            public UnityEngine.Object value;
        }

        [SerializeField]
        SerializableKeyValuePair[] _elements;

        public UnityEngine.Object this[string key] => throw new NotImplementedException();

        public IEnumerable<string> Keys => throw new NotImplementedException();

        public IEnumerable<UnityEngine.Object> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, UnityEngine.Object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out UnityEngine.Object value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
