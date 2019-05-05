using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a Texture, Texture2D, RenderTexture, Cubemap and Sprite variables in a script be a Unity texture preview box field.
    /// </summary>
    public class TexturePreviewAttribute : ArgosPropertyAttributeBase
    {
        #region Public vars
        public readonly bool allowSceneObjects;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="allowSceneObjects">Allow assigning Scene objects.</param>
        /// <param name="tooltip">Specify a tooltip for the field. Left empty for non display tooltip.</param>
        public TexturePreviewAttribute(bool allowSceneObjects = false, string tooltip = "") : base(tooltip)
        {
            this.allowSceneObjects = allowSceneObjects;
        } 
        #endregion
    }
}
