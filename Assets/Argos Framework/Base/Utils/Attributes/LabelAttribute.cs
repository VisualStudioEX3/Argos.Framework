using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to add a multi-line richtext label above some fields in the Inspector.
    /// </summary>
    public class LabelAttribute : PropertyAttribute
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
        public LabelAttribute(string text, bool miniLabel = false, bool selectable = false)
        {
            this.text = text;
            this.miniLabel = miniLabel;
            this.selectable = selectable;
        }
        #endregion
    }
}