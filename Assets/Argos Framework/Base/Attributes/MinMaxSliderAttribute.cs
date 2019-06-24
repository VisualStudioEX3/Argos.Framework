using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a Vector2 or Vector2Int variable in a script be restricted to a specific range.
    /// </summary>
    public class MinMaxSliderAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly Vector2 range;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public MinMaxSliderAttribute(float min = 0f, float max = 1f, string tooltip = "") : base(tooltip)
        {
            this.range = new Vector2(min, max);
        }
        #endregion
    }
}