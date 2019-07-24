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
            return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        }

        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }

        public static float Clamp(this float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }
    }
}
