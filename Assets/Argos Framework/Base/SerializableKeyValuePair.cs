using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Serializable key value pair structure.
    /// </summary>
    /// <typeparam name="TKey">Type of the key field.</typeparam>
    /// <typeparam name="TValue">Type of the value field.</typeparam>
    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        #region Public vars
        public TKey key;
        public TValue value;
        #endregion

        #region Operators
        public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> data)
        {
            return new SerializableKeyValuePair<TKey, TValue>() { key = data.Key, value = data.Value };
        }

        public static explicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> data)
        {
            return new KeyValuePair<TKey, TValue>(data.key, data.value);
        } 
        #endregion
    } 
}
