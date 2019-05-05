using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to set custom names to a Vector2, Vector2Int, Vector3, Vector3Int or Vector4 variable in a script.
    /// </summary>
    public class CustomVectorAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly GUIContent[] names;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="names">Names of each vector element (one character).</param>
        public CustomVectorAttribute(params string[] names) : this(names, "")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="names">Names of each vector element (one character).</param>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public CustomVectorAttribute(string[] names, string tooltip = "") : base(tooltip)
        {
            this.names = new GUIContent[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                this.names[i] = new GUIContent(names[i]);
            }
        }
        #endregion
    }
}