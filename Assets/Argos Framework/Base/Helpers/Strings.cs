using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

namespace Argos.Framework.Helpers
{
    /// <summary>
    /// String helper class.
    /// </summary>
    public static class Strings
    {
        #region Methods & Functions
        /// <summary>
        /// Check if the string contain any of the values in the array.
        /// </summary>
        /// <param name="model">Model string to check.</param>
        /// <param name="values">Array of posible values.</param>
        /// <returns>Return true if any value is containing in the model string.</returns>
        /// <remarks>This version es specialized to work with strings values.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool CheckForString(string model, string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (model.Contains(values[i]))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
