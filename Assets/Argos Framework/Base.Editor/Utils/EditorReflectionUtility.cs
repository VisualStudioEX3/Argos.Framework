using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Utils
{
    /// <summary>
    /// Helper functions to manage reflection calls to <see cref="UnityEditor"/> assembly.
    /// </summary>
    public static class EditorReflectionUtility
    {
        #region Internal vars
        static Assembly _unityEditorAssembly; 
        #endregion

        #region Properties
        /// <summary>
        /// UnityEditor assembly reference.
        /// </summary>
        public static Assembly UnityEditorAssembly
        {
            get
            {
                if (EditorReflectionUtility._unityEditorAssembly == null)
                {
                    EditorReflectionUtility._unityEditorAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.GetName().Name == "UnityEditor").FirstOrDefault();
                }

                return EditorReflectionUtility._unityEditorAssembly;
            }
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Get reference to private <see cref="UnityEditor"/> type.
        /// </summary>
        /// <param name="name">Type name.</param>
        /// <returns>Return the <see cref="Type"/> reference.</returns>
        public static Type GetUnityEditorPrivateType(string name)
        {
            return EditorReflectionUtility.UnityEditorAssembly.GetType($"UnityEditor.{name}");
        }

        /// <summary>
        /// Get reference to nested type from a private <see cref="UnityEditor"/> type.
        /// </summary>
        /// <param name="typeName">Type name that owns the nested type.</param>
        /// <param name="nestedTypeName">Nested type name.</param>
        /// <returns>Return the nested <see cref="Type"/> reference.</returns>
        public static Type GetNestedTypeFromUnityEditorPrivateType(string typeName, string nestedTypeName)
        {
            return EditorReflectionUtility.UnityEditorAssembly.GetType($"UnityEditor.{typeName}+{nestedTypeName}");
        }

        /// <summary>
        /// Get reference to nested type from a private <see cref="UnityEditor"/> type.
        /// </summary>
        /// <param name="typeReference"><see cref="Type"/> that owns the nested type.</param>
        /// <param name="nestedTypeName">Nested type name.</param>
        /// <returns>Return the nested <see cref="Type"/> reference.</returns>
        public static Type GetNestedTypeFromUnityEditorPrivateType(Type typeReference, string nestedTypeName)
        {
            return EditorReflectionUtility.GetNestedTypeFromUnityEditorPrivateType(typeReference.Name, nestedTypeName);
        } 
        #endregion
    } 
}
