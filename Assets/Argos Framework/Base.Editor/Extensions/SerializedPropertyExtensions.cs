using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Extension methods for Serialized Property variables.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Check if the Serialized Property is an array element.
        /// </summary>
        /// <param name="property">Serialized Property to evaluate.</param>
        /// <returns>Return true if the Serialized Property is an array element.</returns>
        public static bool IsArrayElement(this SerializedProperty property)
        {
            return IsArrayElement(property, out int i);
        }

        /// <summary>
        /// Check if the Serialized Property is an array element.
        /// </summary>
        /// <param name="property">Serialized Property to evaluate.</param>
        /// <param name="index">Out parameter that return the array element index.</param>
        /// <returns>Return true if the Serialized Property is an array element.</returns>
        public static bool IsArrayElement(this SerializedProperty property, out int index)
        {
            const string ARRAY_DATA_END_MASK = "Array.data";

            int start = property.propertyPath.LastIndexOf('[') + 1;
            int end = property.propertyPath.Length - 2;

            if (start > 0)
            {
                index = start == end ? int.Parse(property.propertyPath[start].ToString()) : int.Parse(property.propertyPath.Substring(start, end));
                return property.propertyPath.Substring(0, start - 1).EndsWith(ARRAY_DATA_END_MASK);
            }

            index = -1;
            return false;
        }
        #endregion
    }
}
