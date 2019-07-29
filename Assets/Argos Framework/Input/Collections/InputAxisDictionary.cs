using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework.Input
{
    [Serializable]
    public sealed class InputAxisDictionary : SerializableDictionary<string, InputAxis>
    {
        #region Inspector fields
#pragma warning disable 649
        [SerializeField]
        InputAxis[] _elements;
#pragma warning restore
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
            if (this._elements != null)
            {
                foreach (var item in this._elements)
                {
                    this.Add(item.Name, item);
                } 
            }
        }
        #endregion
    }
}
