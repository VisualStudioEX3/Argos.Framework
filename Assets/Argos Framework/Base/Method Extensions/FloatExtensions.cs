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
        /// <param name="oldMin">Original min range value.</param>
        /// <param name="oldMax">Original max range value.</param>
        /// <param name="newMin">New min range value.</param>
        /// <param name="newMax">New max range value.</param>
        /// <returns>Returns the value in the new range.</returns>
        public static float Remap(this float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return Utils.MathUtility.Remap(value, oldMin, oldMax, newMin, newMax);
        }

        /// <summary>
        /// Remap value from original range to new range.
        /// </summary>
        /// <param name="value"><see cref="float"/> value.</param>
        /// <param name="oldRange">Original range defined by a <see cref="Vector2"/>.</param>
        /// <param name="newRange">New range defined by a <see cref="Vector2"/>.</param>
        /// <returns>Returns the value in the new range.</returns>
        public static float Remap(this float value, Vector2 oldRange, Vector2 newRange)
        {
            return Utils.MathUtility.Remap(value, oldRange.x, oldRange.y, newRange.x, newRange.y);
        }
    }
}
