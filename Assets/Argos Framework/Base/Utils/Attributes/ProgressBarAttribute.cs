using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a float variable in a script be a progressbar.
    /// </summary>
    public class ProgressBarAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly string Message;
        public readonly bool ShowLabel;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message show into the progressbar.</param>
        /// <param name="showLabel">Show field prefix label.</param>
        public ProgressBarAttribute(string message = "", bool showLabel = false)
        {
            this.Message = message;
            this.ShowLabel = showLabel;
        }
        #endregion
    }
}
