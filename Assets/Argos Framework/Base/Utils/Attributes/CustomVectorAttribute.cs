using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to set custom names to a Vector2, Vector2Int, Vector3, Vector3Int or Vector4 variable in a script.
    /// </summary>
    public class CustomVectorAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly GUIContent[] Names;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="names">Names of each vector element (one character).</param>
        public CustomVectorAttribute(params string[] names)
        {
            this.Names = new GUIContent[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                this.Names[i] = new GUIContent(names[i]);
            }
        }
        #endregion
    }
}