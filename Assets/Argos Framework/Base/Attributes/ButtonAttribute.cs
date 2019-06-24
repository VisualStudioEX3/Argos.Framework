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

    public enum GUIButtonDisableEvents
    {
        /// <summary>
        /// The button is enable in play and editor modes.
        /// </summary>
        Never,
        /// <summary>
        /// Disable when the game enter in play mode.
        /// </summary>
        PlayMode,
        /// <summary>
        /// Disable when the game enter in editor mode.
        /// </summary>
        EditorMode
    }
    #endregion

    /// <summary>
    /// Attribute used to make a string variable in a script be a button that can invoke a method (method name is defined in the string value).
    /// </summary>
    /// <remarks>Only methods without parameters.</remarks>
    public class ButtonAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly string customLabel;
        public readonly GUIButtonSize size;
        public readonly GUIButtonDisableEvents disableOn;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="customLabel">Custom label to show in the button. By default using the variable name.</param>
        /// <param name="size">The button size. By default using the default button size.</param>
        /// <param name="disableOn">Disable the button when enter in the selected mode (by default is enable for play and editor modes).</param>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public ButtonAttribute(string customLabel = "", GUIButtonSize size = GUIButtonSize.Normal, GUIButtonDisableEvents disableOn = GUIButtonDisableEvents.Never, string tooltip = "") : base(tooltip)
        {
            this.customLabel = customLabel;
            this.size = size;
            this.disableOn = disableOn;
        }
        #endregion
    }
}
