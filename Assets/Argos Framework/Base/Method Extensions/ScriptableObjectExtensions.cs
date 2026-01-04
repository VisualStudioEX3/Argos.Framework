using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="ScriptableObject"/> method extensions.
    /// </summary>
    public static class ScriptableObjectExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Get the name of this class.
        /// </summary>
        /// <param name="instance"><see cref="ScriptableObject"/> instance.</param>
        /// <returns>Return the name of this class.</returns>
        /// <remarks>This function is useful for use with log messages.</remarks>
        public static string GetClassName(this ScriptableObject instance)
        {
            return instance.GetType().Name;
        } 
        #endregion
    }
}