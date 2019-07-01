using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Argos.Framework.Utils
{
    /// <summary>
    /// Collections utility class.
    /// </summary>
    public static class CollectionUtility
    {
        #region Methods & Functions
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
        /// Suffle an array.
        /// </summary>
        /// <typeparam name="T">Type of the array elements.</typeparam>
        /// <param name="array">Array of elements.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(ref T[] array)
        {
            var rng = new System.Random(ApplicationUtility.GenerateSafeRandomSeed());
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
            var rng = new System.Random(ApplicationUtility.GenerateSafeRandomSeed());
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
        /// Check if a value exists in array.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="array">Array where to check.</param>
        /// <returns>Return true if the value exist in the array.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool Exists<T>(T value, T[] array)
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
        #endregion
    }
}
