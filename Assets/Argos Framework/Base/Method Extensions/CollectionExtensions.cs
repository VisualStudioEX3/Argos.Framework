using System;
using System.Collections.Generic;

namespace Argos.Framework
{
    // Based on https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/Reduce
    public static class CollectionReduceExtension
    {
        #region Methods & Functions
        #region Reduce
        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T Reduce<T>(this IList<T> collection, Action<T, T> callback, T initValue = default)
        {
            return collection.Reduce((acc, curr, idx, src) => callback(acc, curr), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T Reduce<T>(this IList<T> collection, Func<T, T, T> callback, T initValue = default)
        {
            return collection.Reduce((acc, curr, idx, src) => callback(acc, curr), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T Reduce<T>(this IList<T> collection, Action<T, T, int> callback, T initValue = default)
        {
            return collection.Reduce((acc, curr, idx, src) => callback(acc, curr, idx), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T Reduce<T>(this IList<T> collection, Func<T, T, int, T> callback, T initValue = default)
        {
            return collection.Reduce((acc, curr, idx, src) => callback(acc, curr, idx), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index, collection.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T Reduce<T>(this IList<T> collection, Action<T, T, int, IList<T>> callback, T initValue = default)
        {
            return collection.ReduceBase((acc, curr, idx, src) => callback(acc, curr, idx, src), null, initValue, false);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index, collection.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T Reduce<T>(this IList<T> collection, Func<T, T, int, IList<T>, T> callback, T initValue = default)
        {
            return collection.ReduceBase(null, (acc, curr, idx, src) => callback(acc, curr, idx, src), initValue, false);
        }
        #endregion

        #region Reduce right
        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, in reverse orden, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T ReduceRight<T>(this IList<T> collection, Action<T, T> callback, T initValue = default)
        {
            return collection.ReduceRight((acc, curr, idx, src) => callback(acc, curr), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, in reverse orden, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T ReduceRight<T>(this IList<T> collection, Func<T, T, T> callback, T initValue = default)
        {
            return collection.ReduceRight((acc, curr, idx, src) => callback(acc, curr), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, in reverse orden, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T ReduceRight<T>(this IList<T> collection, Action<T, T, int> callback, T initValue = default)
        {
            return collection.ReduceRight((acc, curr, idx, src) => callback(acc, curr, idx), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, in reverse orden, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T ReduceRight<T>(this IList<T> collection, Func<T, T, int, T> callback, T initValue = default)
        {
            return collection.ReduceRight((acc, curr, idx, src) => callback(acc, curr, idx), initValue);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, in reverse orden, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index, collection.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T ReduceRight<T>(this IList<T> collection, Action<T, T, int, IList<T>> callback, T initValue = default)
        {
            return collection.ReduceBase((acc, curr, idx, src) => callback(acc, curr, idx, src), null, initValue, true);
        }

        /// <summary>
        /// Executes a reducer function (that you provide) on each element of the collection, in reverse orden, resulting in a single output value.
        /// </summary>
        /// <typeparam name="T">Type of the collection instance.</typeparam>
        /// <param name="collection">Collection instance.</param>
        /// <param name="callback">Callback to execute in each element. Params: accumulator, current element, current index, collection.</param>
        /// <param name="initValue">Initial accumulator value. By default is default type value.</param>
        /// <returns>Returns the accumulator value.</returns>
        public static T ReduceRight<T>(this IList<T> collection, Func<T, T, int, IList<T>, T> callback, T initValue = default)
        {
            return collection.ReduceBase(null, (acc, curr, idx, src) => callback(acc, curr, idx, src), initValue, true);
        }
        #endregion

        static T ReduceBase<T>(this IList<T> collection, Action<T, T, int, IList<T>> proc, Func<T, T, int, IList<T>, T> func, T initValue = default, bool reverse = false)
        {
            T acumulator = initValue;

            for (int i = reverse ? collection.Count - 1 : 0;
                reverse ? i >= 0 : i < collection.Count;
                i += reverse ? -1 : 1)
            {
                if (proc is null)
                {
                    acumulator = func(acumulator, collection[i], i, collection);
                }
                else
                {
                    proc(acumulator, collection[i], i, collection);
                }
            }

            return acumulator;
        } 
        #endregion
    }
}