using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to an integer variable in a script shows a label with formatted the vlaue in bytes.
    /// </summary>
    public class FormatBytesAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly bool showBytes; 
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>Show bytes count if the value is upper of 512 bytes.</remarks>
        public FormatBytesAttribute(bool showBytes = false)
        {
            this.showBytes = showBytes;
        }
        #endregion
    }
}