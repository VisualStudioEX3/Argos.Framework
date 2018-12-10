using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Argos.Framework.Helpers
{
    #region Enums
    public enum DebugLevel
    {
        Default,
        Warning,
        Error,
        Success
    }
    #endregion

    /// <summary>
    /// Debug helper class.
    /// </summary>
    public static class DebugHelper
    {
        #region Constants
        const string DEBUG_COLOR_STRING_TEMPLATE = "<color={0}>{1}</color>";
        static readonly string[] DEBUG_COLORS = new string[] { "white", "orange", "red", "lime" };
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Helper to print colored debug messages.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Debug level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <remarks>This messages appear on the built-in scene debug console when the game is compiled in development mode.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Log(string message, DebugLevel level = DebugLevel.Default, UnityEngine.Object context = null)
        {
            UnityEngine.Debug.LogErrorFormat(context, DebugHelper.DEBUG_COLOR_STRING_TEMPLATE, DebugHelper.DEBUG_COLORS[(int)level], message);
        }
        #endregion
    }
}
