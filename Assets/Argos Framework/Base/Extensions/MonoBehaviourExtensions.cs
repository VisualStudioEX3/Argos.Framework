using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// MonoBehaviour method extensions.
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// Get the name of this class.
        /// </summary>
        /// <param name="instance"><see cref="MonoBehaviour"/> instance.</param>
        /// <returns>Return the name of this class.</returns>
        /// <remarks>This function is useful for use with log messages.</remarks>
        public static string GetClassName(this MonoBehaviour instance)
        {
            return instance.GetType().Name;
        }
    }
}