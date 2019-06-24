using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a string variable in a script be restricted to tag values.
    /// </summary>
    public class TagAttribute : ArgosPropertyAttributeBase
    {
        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public TagAttribute(string tooltip = "") : base(tooltip)
        {
        } 
        #endregion
    }
}