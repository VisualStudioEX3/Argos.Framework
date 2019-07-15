using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="bool"/> method extensions.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Convert to int value.
        /// </summary>
        /// <param name="value"><see cref="bool"/> expresion value.</param>
        /// <returns>Returns -1 for true or 0 for false.</returns>
        public static int ToInt32(this bool value)
        {
            return value ? -1 : 0;
        }
    } 
}
