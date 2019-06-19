using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a string variable in a script be a dinamic multi-line richtext label.
    /// </summary>
    public class DinamicLabelAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly bool miniLabel;
        public readonly bool selectable;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="miniLabel">Show as mini label style.</param>
        /// <param name="selectable">The content is selectable?</param>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public DinamicLabelAttribute(bool miniLabel = false, bool selectable = false, string tooltip = "") : base(tooltip)
        {
            this.miniLabel = miniLabel;
            this.selectable = selectable;
        }
        #endregion
    }
}