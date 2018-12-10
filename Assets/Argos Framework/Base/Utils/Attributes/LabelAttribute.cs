using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Use this PropertyAttribute to add a multi-line richtext label above some fields in the Inspector.
    /// </summary>
    public class LabelAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly string Text;
        public readonly bool MiniLabel;
        public readonly bool Selectable;
        #endregion

        #region MyRegion
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="miniLabel">Show as mini label style.</param>
        /// <param name="selectable">The content is selectable?</param>
        public LabelAttribute(string text, bool miniLabel = false, bool selectable = false)
        {
            this.Text = text;
            this.MiniLabel = miniLabel;
            this.Selectable = selectable;
        }
        #endregion
    }
}