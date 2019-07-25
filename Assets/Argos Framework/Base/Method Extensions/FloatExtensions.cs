using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="float"/> method extensions.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Convert value to decibels.
        /// </summary>
        /// <param name="value"><see cref="float"/> value.</param>
        /// <returns>Returns the decibels equivalence.</returns>
        /// <remarks>This function clamp the value between 0 and 1.</remarks>
        public static float ToDecibels(this float value)
        {
            return Utils.MathUtility.ToDecibels(value);
        }

        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }

        public static float Clamp(this float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        /// <summary>
        /// Remap value from original range to new range.
        /// </summary>
        /// <param name="value"><see cref="float"/> value.</param>
        /// <param name="from1">Original min range value.</param>
        /// <param name="to1">Original max range value.</param>
        /// <param name="from2">New min range value.</param>
        /// <param name="to2">New max range value.</param>
        /// <returns>Returns the value in the new range.</returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return Utils.MathUtility.Remap(value, from1, to1, from2, to2);
        }
    }
}
