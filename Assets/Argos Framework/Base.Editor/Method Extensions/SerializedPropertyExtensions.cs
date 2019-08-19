using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Argos.Framework.Utils;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="SerializedProperty"/> method extensions.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        #region Static members
        static MethodInfo _getFieldInfoFromPropertyPathMethodInfo;
        #endregion

        #region Initializers
        [InitializeOnLoadMethod]
        static void Init()
        {
            // UnityEditor.ScriptAttributeUtility.GetFieldInfoFromProperty(SerializedProperty property, out System.Type type)
            Type tagManagerInspector = EditorReflectionUtility.GetUnityEditorPrivateType("ScriptAttributeUtility");
            SerializedPropertyExtensions._getFieldInfoFromPropertyPathMethodInfo = tagManagerInspector.GetMethod("GetFieldInfoFromProperty", BindingFlags.NonPublic | BindingFlags.Static);
        }
        #endregion

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
            int end = property.propertyPath.LastIndexOf(']') - 1;

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
        public static string GetEnumDisplayName(this SerializedProperty property)
        {
            return property.enumDisplayNames[property.enumValueIndex];
        }

        /// <summary>
        /// Get the <see cref="FieldInfo"/> data from this property.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <returns>Return the <see cref="FieldInfo"/> data from this property.</returns>
        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            // UnityEditor.ScriptAttributeUtility.GetFieldInfoFromProperty(SerializedProperty property, out System.Type type)
            return SerializedPropertyExtensions._getFieldInfoFromPropertyPathMethodInfo.Invoke(null, new object[] { property, null }) as FieldInfo;
        }

        /// <summary>
        /// Return the custom attribute used by this property.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute.</typeparam>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <returns>Return the custom attribute used by this property.</returns>
        public static T GetCustomAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            return SerializedPropertyExtensions.GetFieldInfo(property).GetCustomAttribute<T>();
        }

        /// <summary>
        /// Get the value of a <see cref="Gradient"/> property.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <returns>Return a copy of the <see cref="Gradient"/> field represented by this property.</returns>
        public static Gradient GetGradientValue(this SerializedProperty property)
        {
            PropertyInfo gradientValue = property.GetType().GetProperty("gradientValue", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
            return gradientValue.GetValue(property) as Gradient;
        }

        /// <summary>
        /// Set the value of a <see cref="Gradient"/> property.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> instance.</param>
        /// <param name="value"><see cref="Gradient"/> value.</param>
        public static void SetGradientValue(this SerializedProperty property, Gradient value)
        {
            PropertyInfo gradientValue = property.GetType().GetProperty("gradientValue", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty);
            gradientValue.SetValue(property, value);
        }
        #endregion
    }
}
