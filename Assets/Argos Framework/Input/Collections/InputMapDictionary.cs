using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework.Input
{
    [Serializable]
    public sealed class InputMapDictionary : SerializableDictionary<string, InputMapAsset>
    {
        #region Structs
        [Serializable]
        public struct CustomKeyValuePair
        {
            #region Public vars
            public string key;
            public InputMapAsset value; 
            #endregion

            #region Operators
            public static implicit operator KeyValuePair<string, InputMapAsset>(CustomKeyValuePair data)
            {
                return new KeyValuePair<string, InputMapAsset>(data.key, data.value);
            }
            #endregion
        }
        #endregion

        #region Inspector fields
#pragma warning disable 649
        [SerializeField]
        CustomKeyValuePair[] _elements;
#pragma warning restore
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Update logic of the all maps.
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
