using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Debug extension methods for editor classes.
    /// </summary>
    public static class LoggerExtensions
    {
        #region Extension methods
        /// <summary>
        /// Log a formatted colored message to Unity debug console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Log level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(this Editor instance, string message, LogLevel level = LogLevel.Default, UnityEngine.Object context = null)
        {
            Argos.Framework.Logger.Log(message, level, context);
        }

        /// <summary>
        /// Log a formatted colored message to Unity debug console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Log level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(this EditorWindow instance, string message, LogLevel level = LogLevel.Default, UnityEngine.Object context = null)
        {
            Argos.Framework.Logger.Log(message, level, context);
        }

        /// <summary>
        /// Log a formatted colored message to Unity debug console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Log level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(this PropertyDrawer instance, string message, LogLevel level = LogLevel.Default, UnityEngine.Object context = null)
        {
            Argos.Framework.Logger.Log(message, level, context);
        }

        /// <summary>
        /// Log a formatted colored message to Unity debug console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Log level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(this DecoratorDrawer instance, string message, LogLevel level = LogLevel.Default, UnityEngine.Object context = null)
        {
            Argos.Framework.Logger.Log(message, level, context);
        }
        #endregion
    }
}