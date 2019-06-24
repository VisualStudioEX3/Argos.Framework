using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to add a multi-line richtext label above some fields in the Inspector.
    /// </summary>
    public class LabelAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly string text;
        public readonly bool miniLabel;
        public readonly bool selectable;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="miniLabel">Show as mini label style.</param>
        /// <param name="selectable">The content is selectable?</param>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public LabelAttribute(string text, bool miniLabel = false, bool selectable = false, string tooltip = "") : base(tooltip)
        {
            this.text = text;
            this.miniLabel = miniLabel;
            this.selectable = selectable;
        }
        #endregion
    }
}