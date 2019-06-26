using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make an integer or string variable in a script be restricted to <see cref="SortingLayer"/> values.
    /// </summary>
    /// <remarks>Whe the variable is an intenger type, this store the layer id, when the variable is a string type, store the layer name.</remarks>
    public class SortingLayerAttribute : ArgosPropertyAttributeBase
    {
        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public SortingLayerAttribute(string tooltip = "") : base(tooltip)
        {
        }
        #endregion
    }
}