using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    #region Enums
    public enum GUIButtonSize
    {
        Normal,
        Large,
        Mini
    }
    #endregion
    /// <summary>
    /// Attribute used to make a string variable in a script be a button that can invoke a method (method name is defined in the string value).
    /// </summary>
    /// <remarks>Only methods without parameters.</remarks>
    public class ButtonAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly string CustomLabel;
        public readonly GUIButtonSize Size;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="customLabel">Custom label to show in the button. By default using the variable name.</param>
        /// <param name="size">The button size. By default using the default button size.</param>
        public ButtonAttribute(string customLabel = "", GUIButtonSize size = GUIButtonSize.Normal)
        {
            this.CustomLabel = customLabel;
            this.Size = size;
        } 
        #endregion
    } 
}
