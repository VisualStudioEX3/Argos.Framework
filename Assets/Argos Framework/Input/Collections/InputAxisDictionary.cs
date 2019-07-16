using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework.Input
{
    [Serializable]
    public sealed class InputAxisDictionary : SerializableDictionary<string, InputAxis>
    {
        #region Structs
        [Serializable]
        public struct CustomKeyValuePair
        {
            #region Public vars
            public string key;
            public InputAxis value;
            #endregion

            #region Operators
            public static implicit operator KeyValuePair<string, InputAxis>(CustomKeyValuePair data)
            {
                return new KeyValuePair<string, InputAxis>(data.key, data.value);
            }
            #endregion
        }
        #endregion

        #region Inspector fields
        [SerializeField]
        CustomKeyValuePair[] _elements;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Update logic of the all axes.
        /// </summary>
        public void Update()
        {
            foreach (var item in this.Values)
            {
                item.Update();
            }
        } 
        #endregion

        #region Event listeners
        public override void OnSerialize()
        {
            foreach (var item in this._elements)
            {
                this.Add(item);
            }
        }
        #endregion
    }
}
