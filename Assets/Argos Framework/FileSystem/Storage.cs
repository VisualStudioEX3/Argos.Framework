using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Argos.Framework.FileSystem
{
    /// <summary>
    /// Storage dictionary used by File Slots.
    /// </summary>
    /// <remarks>This storage dictionary only allow the following types: boolean, int, float, string, TimeSpan and DateTime.</remarks>
    public sealed class Storage : IDisposable
    {
        #region Constants
        // Cached characters for custom JSON serializer:
        const char CHAR_NULL = '\n';
        const char CHAR_DOUBLE_QUOTE = '\"';
        const char CHAR_DOT = '.';
        const char CHAR_COMMA = ',';
        const char CHAR_COLON = ':';
        const char CHAR_LEFT_CURLY_BRACKET = '{';
        const char CHAR_RIGHT_CURLY_BRACKET = '}';
        #endregion

        #region Internal vars
        Dictionary<string, object> _dictionary;
        #endregion

        #region Properties
        /// <summary>
        /// The number of values stored in this storage.
        /// </summary>
        public int Count
        {
            get
            {
                return this._dictionary.Count;
            }
        }
        #endregion

        #region Constructor & destructors
        public Storage()
        {
            this._dictionary = new Dictionary<string, object>();
        }

        /// <summary>
        /// Try to parse a JSON string.
        /// </summary>
        /// <param name="json">Keypar value list in JSON format (previously created by ToJSON() function).</param>
        /// <remarks>Load a complex JSON string may created corrupt dictionary or throw any exception.</remarks>
        public Storage(string json) : this()
        {
            string[] values = json.Replace(Storage.CHAR_DOUBLE_QUOTE, Storage.CHAR_NULL)
                                  .Split(Storage.CHAR_COMMA);

            bool boolValue; int intValue; float floatValue;

            for (int i = 1; i < values.Length - 1; i++) // Exclude first and last lines (JSON open and close block: '{' and '}').
            {
                string[] pieces = values[i].Substring(0, values[i].Length - 1).Split(Storage.CHAR_COLON);

                if (bool.TryParse(pieces[1], out boolValue))
                {
                    this._dictionary.Add(pieces[0], boolValue);
                }
                else if (int.TryParse(pieces[1], out intValue))
                {
                    this._dictionary.Add(pieces[0], intValue);
                }
                else if (float.TryParse(pieces[1], out floatValue))
                {
                    this._dictionary.Add(pieces[0], floatValue);
                }
                else
                {
                    this._dictionary.Add(pieces[0], pieces[1]);
                }
            }

            System.GC.Collect();
        }

        Storage(byte[] buffer)
        {
            this._dictionary = BinarySerializer.Deserialize<Dictionary<string, object>>(buffer);
        }

        public void Dispose()
        {
            this._dictionary.Clear();
            this._dictionary = null;
        }
        #endregion

        #region Methods & Functions
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void SetValue(string key, object value)
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
        object GetValue(string key)
        {
            if (this._dictionary.ContainsKey(key))
            {
                return this._dictionary[key];
            }
            else
            {
                throw new KeyNotFoundException($"The key \"{key}\" not exists.");
            }
        }

        /// <summary>
        /// Add or set boolean value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void SetBool(string key, bool value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set integer value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void SetInt(string key, int value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set float value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void SetFloat(string key, float value)
        {
            this.SetValue(key, value);
        }

        /// <summary>
        /// Add or set string value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Value to store.</param>
        public void SetString(string key, string value)
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
        /// Return a boolean value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>Return the value associate by key.</returns>
        public bool GetBool(string key)
        {
            return (bool)this.GetValue(key);
        }

        /// <summary>
        /// Return a integer value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>Return the value associate by key.</returns>
        public int GetInt(string key)
        {
            return (int)this.GetValue(key);
        }

        /// <summary>
        /// Return a float value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>Return the value associate by key.</returns>
        public float GetFloat(string key)
        {
            return (float)this.GetValue(key);
        }

        /// <summary>
        /// Return a string value.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>Return the value associate by key.</returns>
        public string GetString(string key)
        {
            return (string)this.GetValue(key);
        }

        /// <summary>
        /// Clear entire dictionary.
        /// </summary>
        public void Clear()
        {
            this._dictionary.Clear();
        }

        /// <summary>
        /// Serialize to JSON.
        /// </summary>
        /// <returns>A JSON formatted string with all data.</returns>
        /// <remarks>The Unity JsonUtility can't serialize Dictionary objects and object variables. This function serialized a simple keypar value list in standard JSON format.</remarks>
        public string ToJSON()
        {
            var json = new StringBuilder();
            {
                json.AppendLine(Storage.CHAR_LEFT_CURLY_BRACKET.ToString());
                {
                    string value = string.Empty;

                    foreach (var item in this._dictionary)
                    {
                        if (item.Value is bool)
                        {
                            value = $"{(bool)item.Value}";
                        }
                        else if (item.Value is int)
                        {
                            value = $"{(int)item.Value}";
                        }
                        else if (item.Value is float)
                        {
                            value = $"{((float)item.Value).ToString("0.0#").Replace(Storage.CHAR_COMMA, Storage.CHAR_DOT)}";
                        }
                        else
                        {
                            value = $"\"{item.Value}\"";
                        }

                        json.AppendLine($"\t\"{item.Key}\": {value},");
                    }
                }
                json.AppendLine(Storage.CHAR_RIGHT_CURLY_BRACKET.ToString());
            }
            return json.ToString();
        }

        /// <summary>
        /// Serialize to binary.
        /// </summary>
        /// <returns>Return the binary array with the all data.</returns>
        public byte[] ToBinary()
        {
            return BinarySerializer.Serialize(this._dictionary);
        }
        #endregion
    }
}