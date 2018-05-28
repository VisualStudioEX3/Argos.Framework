/// Based on the original code of John Saunders "One Way to Serialize Dictionaries"
/// http://johnwsaundersiii.spaces.live.com/blog/cns!600A2BE4A82EA0A6!699.entry

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace Argos.Framework.FileSystem.Serializers
{
    /// <summary>
    /// Serializable Dictionary.
    /// </summary>
    /// <typeparam name="K">Key value type.</typeparam>
    /// <typeparam name="V">Data value type.</typeparam>
    /// <remarks>Use this dictionary implementation when you need to save a dictionary to XML or binary file.</remarks>
    public class SerializableDictionary<K, V>
    {
        #region Constructors
        public SerializableDictionary(IDictionary<K, V> original)
        {
            this.Dictionary = original;
        }

        public SerializableDictionary()
        {
            this.Dictionary = new Dictionary<K, V>();
        }
        #endregion

        #region Interface like as IDictionary
        public void Add(K Key, V Value)
        {
            this.Dictionary.Add(Key, Value);
        }

        public void Add(SerializableKeyValuePair item)
        {
            this.Dictionary.Add(item.ToKeyValuePair());
        }

        public void Remove(K Key)
        {
            this.Dictionary.Remove(Key);
        }

        public void Remove(SerializableKeyValuePair item)
        {
            this.Dictionary.Remove(item.ToKeyValuePair());
        }

        [XmlIgnore]
        public ReadOnlyCollection<V> Values { get { RebuildInternalDictionary(); return this.Dictionary.Values.ToList<V>().AsReadOnly(); } }

        [XmlIgnore]
        public ReadOnlyCollection<K> Keys { get { RebuildInternalDictionary(); return this.Dictionary.Keys.ToList<K>().AsReadOnly(); } }

        [XmlIgnore]
        public V this[K key] { get { RebuildInternalDictionary(); return this.Dictionary[key]; } }

        [XmlIgnore]
        public int Count { get { RebuildInternalDictionary(); return this.Dictionary.Count; } }
        #endregion

        private void RebuildInternalDictionary()
        {
            if (this.Dictionary.Count == 0 && this._list != null && this._list.Count > 0)
            {
                foreach (SerializableKeyValuePair kvp in this._list)
                    this.Dictionary.Add(kvp.ToKeyValuePair());
            }
        }

        #region The Proxy List
        [XmlIgnore]
        public IDictionary<K, V> Dictionary { get; internal set; }

        /// <summary>
        /// Holds the keys and values
        /// </summary>
        public class SerializableKeyValuePair
        {
            public K Key { get; set; }
            public V Value { get; set; }
            internal KeyValuePair<K, V> ToKeyValuePair()
            {
                return new KeyValuePair<K, V>(this.Key, this.Value);
            }
        }

        // This field will store the deserialized list
        private Collection<SerializableKeyValuePair> _list;

        /// <remarks>
        /// XmlElementAttribute is used to prevent extra nesting level. It's
        /// not necessary.
        /// </remarks>
        [XmlElement]
        public Collection<SerializableKeyValuePair> KeysAndValues
        {
            get
            {
                if (_list == null)
                {
                    _list = new Collection<SerializableKeyValuePair>();
                }

                // On deserialization, Original will be null, just return what we have
                if (Dictionary == null)
                {
                    return _list;
                }

                // If Original was present, add each of its elements to the list
                if (this.Dictionary.Count > 0) _list.Clear();
                foreach (var pair in Dictionary)
                {
                    _list.Add(new SerializableKeyValuePair { Key = pair.Key, Value = pair.Value });
                }

                return _list;
            }
        }
        #endregion

        /// <summary>
        /// Convenience method to return a dictionary from this proxy instance
        /// </summary>
        /// <returns></returns>
        public Dictionary<K, V> ToDictionary()
        {
            return KeysAndValues.ToDictionary(key => key.Key, value => value.Value);
        }
    }
}