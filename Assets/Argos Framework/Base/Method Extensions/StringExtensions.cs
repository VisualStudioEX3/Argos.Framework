using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="string"/> method extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This string is null, empty or white space?
        /// </summary>
        /// <param name="value"><see cref="string"/> instance.</param>
        /// <returns>Return true if the string is null, empty or white space.</returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value);
        }
    }
}