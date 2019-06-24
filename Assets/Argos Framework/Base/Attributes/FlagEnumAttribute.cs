using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    public sealed class FlagEnumAttribute : ArgosPropertyAttributeBase
    {
        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public FlagEnumAttribute(string tooltip = "") : base(tooltip)
        {
        } 
        #endregion
    }
}