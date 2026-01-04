using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a float variable in a script be a progressbar.
    /// </summary>
    public class ProgressBarAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly string message;
        public readonly bool showLabel;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message show into the progressbar.</param>
        /// <param name="showLabel">Show field prefix label.</param>
        public ProgressBarAttribute(string message = "", bool showLabel = false)
        {
            this.message = message;
            this.showLabel = showLabel;
        }
        #endregion
    }
}
