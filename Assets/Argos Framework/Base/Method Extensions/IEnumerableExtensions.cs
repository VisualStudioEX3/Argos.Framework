using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Argos.Framework
{
    public static class IEnumerableExtensions
    {
        #region Methods & Functions
        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            return ascending ? source.OrderBy(selector) : source.OrderByDescending(selector);
        }

        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            return ascending ? source.ThenBy(selector) : source.ThenByDescending(selector);
        } 
        #endregion
    }
}
