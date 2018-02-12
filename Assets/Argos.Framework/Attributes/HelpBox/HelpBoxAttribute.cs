using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// HelpBox MessageType enum non dependant form UnityEditor namespace.
    /// </summary>
    public enum HelpBoxMessageType
    {
        None,
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Shows a help box before the field on the inspector
    /// </summary>
    /// <remarks>
    /// Modified from code at https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/#post-3014998
    /// </remarks>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public string text;
        public HelpBoxMessageType messageType;

        public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
        {
            this.text = text;
            this.messageType = messageType;
        }
    }
}