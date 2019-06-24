using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to an int or float variable in a script shows a label with formatted the vlaue in bytes.
    /// </summary>
    public class FormatBytesAttribute : ArgosPropertyAttributeBase
    {
        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public FormatBytesAttribute(string tooltip = "") : base(tooltip)
        {
        }
        #endregion
    }
}