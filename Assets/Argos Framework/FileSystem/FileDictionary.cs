using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using System;
using System.Runtime.CompilerServices;

namespace Argos.Framework.FileSystem
{
    /// <summary>
    /// Dictionary of dinamic values used by File Slots.
    /// </summary>
    /// <remarks>This dictionary only allow the following types: boolean, int, float, string, TimeSpan and DateTime.</remarks>
    public sealed class FileDictionary : IDisposable
    {
        #region Internal vars
        Dictionary<string, dynamic> _dictionary;
        #endregion

        #region Properties
        /// <summary>
        /// The number of entries stored in this dictionary.
        /// </summary>
        public int Count
        {
            get
            {
                return this._dictionary.Count;
            }
        }

        public dynamic this[string key]
        {
            get
            {
                return this._dictionary[key];
            }

            set
            {
                this.SetValue(key, value);
            }
        }
        #endregion

        #region Constructor & destructors
        public FileDictionary()
        {
            this._dictionary = new Dictionary<string, dynamic>();
        }

        FileDictionary(byte[] buffer)
        {
            this._dictionary = BinarySerializer.Deserialize<Dictionary<string, dynamic>>(buffer);
        }

        public void Dispose()
        {
            this._dictionary.Clear();
            this._dictionary = null;
        }
        #endregion

        #region Methods & Functions
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void SetValue(string key, dynamic value)
        {
            if (this._dictionary.ContainsKey(key))
            {
                this._dictionary[key] = value;
            }
            else
            {
                this._dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Get value from dictionary.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>Return the desired value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public dynamic Get(string key)
        {
            if (this._dictionary.Count > 0)
            {
                return this._dictionary[key];
            }
            else
            {
                throw new Exception("The dictionary is empty.");
            }
        }

        /// <summary>
        /// Add or set boolean value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void Set(string key, bool value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set int value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void Set(string key, int value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set float value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void Set(string key, float value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set string value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void Set(string key, string value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set TimeSpan value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void Set(string key, TimeSpan value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set DateTime value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void Set(string key, DateTime value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Remove value from dictionary.
        /// </summary>
        /// <param name="key">Key value.</param>
        public void Remove(string key)
        {
            this._dictionary.Remove(key);
        }

        /// <summary>
        /// Clear entire dictionary.
        /// </summary>
        public void Clear()
        {
            this._dictionary.Clear();
        }

        /// <summary>
        /// Serialize to binary.
        /// </summary>
        /// <returns>Return the binary array with the all data.</returns>
        public byte[] Serialize()
        {
            return BinarySerializer.Serialize(this._dictionary);
        }
        #endregion
    }
}