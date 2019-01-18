using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a Texture, Texture2D, RenderTexture, Cubemap and Sprite variables in a script be a Unity texture preview box field.
    /// </summary>
    public class TexturePreviewAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly bool AllowSceneObjects = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="allowSceneObjects">Allow assigning Scene objects.</param>
        public TexturePreviewAttribute(bool allowSceneObjects = false)
        {
            this.AllowSceneObjects = allowSceneObjects;
        } 
        #endregion
    }
}
