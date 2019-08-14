using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="SerializedProperty"/> method extension.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Check if the Serialized Property is an array element.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <returns>Return true if the <see cref="SerializedProperty"/> is an array element.</returns>
        public static bool IsArrayElement(this SerializedProperty property)
        {
            return IsArrayElement(property, out int i);
        }

        /// <summary>
        /// Check if the Serialized Property is an array element.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <param name="index">Out parameter that return the array element index.</param>
        /// <returns>Return true if the <see cref="SerializedProperty"/> is an array element.</returns>
        public static bool IsArrayElement(this SerializedProperty property, out int index)
        {
            const string ARRAY_DATA_END_MASK = "Array.data";

            int start = property.propertyPath.LastIndexOf('[') + 1;
            int end = property.propertyPath.Length - 2;

            if (start > 0)
            {
                index = (start == end) ? int.Parse(property.propertyPath[start].ToString()) : int.Parse(property.propertyPath.Substring(start, end));
                return property.propertyPath.Substring(0, start - 1).EndsWith(ARRAY_DATA_END_MASK);
            }

            index = -1;
            return false;
        }

        /// <summary>
        /// Get name of current enumeration value of an enum property.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <returns>Return the current string representation enumeration value.</returns>
        public static string GetEnumName(this SerializedProperty property)
        {
            return property.enumNames[property.enumValueIndex];
        }

        /// <summary>
        /// Get display-friendly name of current enumeration value of an enum property.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <returns>Return the current string representation enumeration value like shows in editor controls.</returns>
        public static string GetDisplayEnumName(this SerializedProperty property)
        {
            return property.enumDisplayNames[property.enumValueIndex];
        }
        #endregion
    }
}
