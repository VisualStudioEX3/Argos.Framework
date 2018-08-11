using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class Helper
    {
        #region Constants
        const float DELTA_COMPARER_TOLERANCE = 0.000001f;
        const string DEBUG_COLOR_STRING_TEMPLATE = "<color={0}>{1}</color>";
        static readonly string[] DEBUG_COLORS = new string[] { "white", "orange", "red", "lime" };
        #endregion

        #region Enums
        public enum DebugLevel
        {
            Default,
            Warning,
            Error,
            Success
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Swap variable values.
        /// </summary>
        /// <typeparam name="T">Type of variables.</typeparam>
        /// <param name="a">First var.</param>
        /// <param name="b">Second var.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a; a = b; b = c;
        }

        /// <summary>
        /// Fill an array with a same value.
        /// </summary>
        /// <typeparam name="T">Array type data.</typeparam>
        /// <param name="array">Initialized array to fill.</param>
        /// <param name="value">Value to fill each element of array.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(ref T[] array, T value)
        {
            array = Enumerable.Repeat<T>(value, array.Length).ToArray();
        }

        /// <summary>
        /// Fill a list with a same value.
        /// </summary>
        /// <typeparam name="T">List type data.</typeparam>
        /// <param name="list">Initialized list to fill.</param>
        /// <param name="value">Value to fill each element of list.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(ref List<T> list, T value)
        {
            list = Enumerable.Repeat<T>(value, list.Count).ToList();
        }

        /// <summary>
        /// Created a safe random seed for intializing the System.Random class.
        /// </summary>
        /// <returns>Return a random seed.</returns>
        /// <remarks>This functions calculated the seed using the System.Security.Cryptography.RandomNumberGenerator.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CalculateSafeRandomSeed()
        {
            var cryptoResult = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(cryptoResult);
            return BitConverter.ToInt32(cryptoResult, 0);
        }

        /// <summary>
        /// Suffle an array.
        /// </summary>
        /// <typeparam name="T">Type of the array elements.</typeparam>
        /// <param name="array">Array of elements.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(ref T[] array)
        {
            var rng = new System.Random(CalculateSafeRandomSeed());
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        /// <summary>
        /// Suffle a list.
        /// </summary>
        /// <typeparam name="T">Type of the generic list elements.</typeparam>
        /// <param name="list">Generic list of elements.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(ref List<T> list)
        {
            var rng = new System.Random(CalculateSafeRandomSeed());
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Transform the enum name element in to string value.
        /// </summary>
        /// <param name="value">Enum value.</param>
        /// <returns>Return the string name value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string EnumToString(object value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        /// <summary>
        /// Generate a Int32 value based on a GUID value.
        /// </summary>
        /// <returns>Return a Int32 GUID value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int GenerateInt32GuidValue()
        {
            return BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
        }

        /// <summary>
        /// Return a random element index on base of a probability table values.
        /// </summary>
        /// <param name="probabilityRatesTable">The array of probability with the occurrences for each element.</param>
        /// <returns>A random index based on its occurrence.</returns>
        /// <remarks>The probability rates table array contains values, from 0 to a max value to determine the occurrence of each element of an array. 
        /// This algorithm is based on a simbol rate generation code from a slot machine game.</remarks>
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
        /// Determine if a value is in range.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Lower bound of range.</param>
        /// <param name="max">Upper bound of range.</param>
        /// <returns></returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool IsValueInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determine if a value is in range.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="min">Lower bound of range.</param>
        /// <param name="max">Upper bound of range.</param>
        /// <returns></returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool IsValueInRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Determine if a float value is similar to a second float value, using tolerance value.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="tolerance">Tolerance to similarity.</param>
        /// <returns>Return true if the first value is similar to second value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool CompareFloats(float a, float b, float tolerance = Helper.DELTA_COMPARER_TOLERANCE)
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
        public static bool CompareVector(Vector2 a, Vector2 b, float tolerance = Helper.DELTA_COMPARER_TOLERANCE)
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
        public static bool CompareVector(Vector3 a, Vector3 b, float tolerance = Helper.DELTA_COMPARER_TOLERANCE)
        {
            return Vector3.SqrMagnitude(a - b) < tolerance;
        }

        /// <summary>
        /// Set the ragdoll pose based on the character current pose.
        /// </summary>
        /// <param name="ragdoll">Ragdoll based on character.</param>
        /// <param name="character">Character to copy the pose.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetRagdollPose(Transform ragdoll, Transform character)
        {
            ragdoll.position = character.position;
            ragdoll.rotation = character.rotation;

            for (int i = 0; i < ragdoll.childCount; i++)
            {
                Helper.SetRagdollPose(ragdoll.GetChild(i), character.GetChild(i));
            }
        }

        /// <summary>
        /// Check if a value is in array.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="array">Array where to check.</param>
        /// <returns>Return true if the value exist in the array.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool IsInArray<T>(T value, T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the string contain any of the values in the array.
        /// </summary>
        /// <param name="model">Model string where check.</param>
        /// <param name="values">Array of posible values.</param>
        /// <returns>Return true if any value is containing in the model string.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool IsInString(string model, string[] values)
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

        /// <summary>
        /// Force value to the next even number.
        /// </summary>
        /// <param name="value">Value to evaluate and force.</param>
        /// <returns>The next even number.</returns>
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

        /// <summary>
        /// Helper to print colored debug messages.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Debug level (Normal as default).</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <remarks>This messages appear on the builtin scene debug console when the game is compiled in development mode.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Log(string message, DebugLevel level = DebugLevel.Default, UnityEngine.Object context = null)
        {
            UnityEngine.Debug.LogErrorFormat(context, Helper.DEBUG_COLOR_STRING_TEMPLATE, Helper.DEBUG_COLORS[(int)level], message);
        }

        /// <summary>
        /// Cleanup memory and unused assets.
        /// </summary>
        /// <param name="discardGCCollect">Discard System.GC.Collect() call during the cleanup process.</param>
        /// <returns>Return an AsyncOperation for controlling the wait period during the cleanup process.</returns>
        /// <remarks>This function is only a shortcut to call an System.GC.Collect() and UnityEngine.Resources.UnloadUnussedAssets() functions.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static AsyncOperation CleanUpMemoryAndAssets(bool discardGCCollect = false)
        {
            if (!discardGCCollect)
            {
                System.GC.Collect();
            }

            return Resources.UnloadUnusedAssets();
        }
        #endregion
    }
}
