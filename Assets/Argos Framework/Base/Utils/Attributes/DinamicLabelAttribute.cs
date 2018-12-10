using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a string variable in a script be a dinamic multi-line richtext label.
    /// </summary>
    public class DinamicLabelAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly bool MiniLabel;
        public readonly bool Selectable;
        #endregion

        #region MyRegion
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="miniLabel">Show as mini label style.</param>
        /// <param name="selectable">The content is selectable?</param>
        public DinamicLabelAttribute(bool miniLabel = false, bool selectable = false)
        {
            this.MiniLabel = miniLabel;
            this.Selectable = selectable;
        }
        #endregion
    }
}