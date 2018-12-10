using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to present a enum field as a check list with all enum possible values like an option list.
    /// </summary>
    public class OptionListAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly bool Split = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="split">Split the list in two columns.</param>
        public OptionListAttribute(bool split = false)
        {
            this.Split = split;
        }
        #endregion
    }
}