using UnityEngine;

namespace Argos.Framework
{
    #region Enums
    /// <summary>
    /// HelpBox MessageType.
    /// </summary>
    public enum HelpBoxMessageType
    {
        None,
        Info,
        Warning,
        Error
    }
    #endregion

    /// <summary>
    /// Use this PropertyAttribute to add a helpbox above some fields in the Inspector.
    /// </summary>
    /// <remarks>
    /// Based on code at https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/#post-3014998
    /// </remarks>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class HelpBoxAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly string text;
        public readonly HelpBoxMessageType messageType;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">The message text.</param>
        /// <param name="messageType">The type of message.</param>
        public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
        {
            this.text = text;
            this.messageType = messageType;
        }
        #endregion
    }
}