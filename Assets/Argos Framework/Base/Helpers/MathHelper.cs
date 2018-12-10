using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Argos.Framework.Helpers
{
    /// <summary>
    /// Math helper class.
    /// </summary>
    public static class MathHelper
    {
        #region Constants
        const float DELTA_COMPARER_TOLERANCE = 0.000001f;
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Determine if a value is clamped in a range.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Lower bound of range.</param>
        /// <param name="max">Upper bound of range.</param>
        /// <returns>Return true if the value is clamped.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool IsClamped(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determine if a value is clamped in a range.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Lower bound of range.</param>
        /// <param name="max">Upper bound of range.</param>
        /// <returns>Return true if the value is clamped.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool IsClamped(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Return a random element index on base of a probability table values.
        /// </summary>
        /// <param name="probabilityRatesTable">The array of probability with the occurrences for each element.</param>
        /// <returns>A random index based on its occurrence.</returns>
        /// <remarks>The probability rates table array contains values, from 0 to a max value to determine the occurrence of each element of an array. 
        /// This algorithm is based on a symbol rate generation code from a slot machine game.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GetRandomIndexByProbabilityRate(int[] probabilityRatesTable)
        {
            int index;

            while (true)
            {
                // Get random element from the list:
                index = UnityEngine.Random.Range(0, probabilityRatesTable.Length);

                // Calculate the max occurrence value, using the probability rate table array, and if its major than random value between 0 and 1, return the element index:
                if (((float)probabilityRatesTable[index] / (float)probabilityRatesTable.Length) >= UnityEngine.Random.value)
                {
                    return index;
                }
            }
        }

        /// <summary>
        /// Get unclamped angle between two vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>Return the unclampled angle.</returns>
        /// <remarks>Return angle in inverse clockwise direction (the opposite as Unity GetAngle functions).</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float GetAngle(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(b.y - a.y, b.x - a.x) / Mathf.PI * 180f;
        }

        /// <summary>
        /// Determine if a float value is similar to a second float value, using tolerance value.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="tolerance">Tolerance to similarity.</param>
        /// <returns>Return true if the first value is similar to second value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool CompareFloats(float a, float b, float tolerance = MathHelper.DELTA_COMPARER_TOLERANCE)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        /// <summary>
        /// Determine if a vector value is similar to a second vector value, using tolerance value.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <param name="tolerance">Tolerance to similarity.</param>
        /// <returns>Return true if the first vector is similar to second vector.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool CompareVector(Vector2 a, Vector2 b, float tolerance = MathHelper.DELTA_COMPARER_TOLERANCE)
        {
            return Vector2.SqrMagnitude(a - b) < tolerance;
        }

        /// <summary>
        /// Determine if a vector value is similar to a second vector value, using tolerance value.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <param name="tolerance">Tolerance to similarity.</param>
        /// <returns>Return true if the first vector is similar to second vector.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool CompareVector(Vector3 a, Vector3 b, float tolerance = MathHelper.DELTA_COMPARER_TOLERANCE)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance;
        }

        /// <summary>
        /// Force value to the next even number.
        /// </summary>
        /// <param name="value">Value to evaluate and force.</param>
        /// <returns>The next even number.</returns>14104
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int ForceEvenValue(int value)
        {
            return (value % 2 != 0) ? value + 1 : value;
        }

        /// <summary>
        /// Force value to the next even number.
        /// </summary>
        /// <param name="value">Value to evaluate and force.</param>
        /// <returns>The next even number.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float ForceEvenValue(float value)
        {
            return ForceEvenValue((int)value);
        }

        /// <summary>
        /// Force value to the next odd number.
        /// </summary>
        /// <param name="value">Value to evaluate and force.</param>
        /// <returns>The next odd number.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int ForceOddValue(int value)
        {
            return (value % 2 == 0) ? value + 1 : value;
        }

        /// <summary>
        /// Force value to the next odd number.
        /// </summary>
        /// <param name="value">Value to evaluate and force.</param>
        /// <returns>The next odd number.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static float ForceOddValue(float value)
        {
            return ForceOddValue((int)value);
        }
        #endregion
    }
}
