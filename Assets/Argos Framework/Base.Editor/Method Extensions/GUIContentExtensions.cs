using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="GUIContent"/> method extensions.
    /// </summary>
    public static class GUIContentExtensions
    {
        /// <summary>
        /// Gets a copy from this content.
        /// </summary>
        /// <param name="style"><see cref="GUIContent"/> instance.</param>
        /// <returns>Returns a new <see cref="GUIContent"/> instance as copy of this instance.</returns>
        public static GUIContent Copy(this GUIContent content)
        {
            return new GUIContent(content);
        }
    } 
}
