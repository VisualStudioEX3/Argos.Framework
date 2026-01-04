using System.Runtime.CompilerServices;
using UnityEngine;

namespace Argos.Framework
{
    #region Enums
    /// <summary>
    /// Debug level. Each value is associated to an specific color.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Log message in white.
        /// </summary>
        Default,
        /// <summary>
        /// Log message in orange.
        /// </summary>
        Warning,
        /// <summary>
        /// Log message in red.
        /// </summary>
        Error,
        /// <summary>
        /// Log message in lime.
        /// </summary>
        Success
    }
    #endregion 

    /// <summary>
    /// Logger class to ease log formatted debug messages and target the call point location in your code from Unity debug window.
    /// </summary>
    /// <remarks>The implementation try to optimize using a minimal ammount of managed string instances in the code.</remarks>
    public static class Logger
    {
        #region Constants
        const string LOG_COLOR_STRING_TEMPLATE = "<color={0}>{1}</color>";
        static readonly string[] LOG_LEVEL_COLORS = new string[] { "white", "orange", "red", "lime" };
        #endregion

        #region Properties
        /// <summary>
        /// Force to log on the built-in scene debug console when the game is compiled in development mode. By default not.
        /// </summary>
        /// <remarks>When this property is set to true, all log messages appear as error logs in Unity debug console.</remarks>
        public static bool LogInDevelopmentConsole { get; set; }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Log a formatted colored message to Unity debug console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Debug level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Log(string message, LogLevel level = LogLevel.Default, UnityEngine.Object context = null)
        {
            if (Logger.LogInDevelopmentConsole || level == LogLevel.Error)
            {
                // Only LogError functions appear on the built-in scene debug console when the game is compiled in development mode.
                Debug.LogErrorFormat(context, Logger.LOG_COLOR_STRING_TEMPLATE, Logger.LOG_LEVEL_COLORS[(int)level], message); 
            }
            else if (level == LogLevel.Warning)
            {
                Debug.LogWarningFormat(context, Logger.LOG_COLOR_STRING_TEMPLATE, Logger.LOG_LEVEL_COLORS[(int)level], message);
            }
            else
            {
                Debug.LogFormat(context, Logger.LOG_COLOR_STRING_TEMPLATE, Logger.LOG_LEVEL_COLORS[(int)level], message);
            }
        }
        #endregion

        #region Extension methods
        /// <summary>
        /// Log a formatted colored message to Unity debug console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Log level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(this MonoBehaviour instance, string message, LogLevel level = LogLevel.Default, UnityEngine.Object context = null)
        {
            Logger.Log(message, level, context);
        }

        /// <summary>
        /// Log a formatted colored message to Unity debug console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Log level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(this ScriptableObject instance, string message, LogLevel level = LogLevel.Default, UnityEngine.Object context = null)
        {
            Logger.Log(message, level, context);
        }
        #endregion
    }
}
