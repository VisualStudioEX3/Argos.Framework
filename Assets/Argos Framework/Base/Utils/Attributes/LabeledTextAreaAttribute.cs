using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    public class LabeledTextAreaAttribute : PropertyAttribute
    {
        #region Public vars
        public int Lines; 
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="lines">The lines to show in the field at same time. By default 3.</param>
        public LabeledTextAreaAttribute(int lines = 3)
        {
            this.Lines = lines;
        }
    } 
}
