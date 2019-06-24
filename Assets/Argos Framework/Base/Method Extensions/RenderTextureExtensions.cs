using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// RenderTexture method extensions.
    /// </summary>
    public static class RenderTextureExtensions
    {
        #region Methods & Functions
        public static Texture2D ToTexture2D(this RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;

            var texture = new Texture2D(renderTexture.width, renderTexture.height);
            texture.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            RenderTexture.active = null;

            return texture;
        } 
        #endregion
    }
}