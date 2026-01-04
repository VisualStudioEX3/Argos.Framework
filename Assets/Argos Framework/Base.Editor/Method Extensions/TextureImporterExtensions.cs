using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// <see cref="TextureImporter"/> method extensions.
    /// </summary>
    public static class TextureImporterExtensions
    {
        // Source: https://forum.unity.com/threads/getting-original-size-of-texture-asset-in-pixels.165295/
        /// <summary>
        /// Get source texture size.
        /// </summary>
        /// <param name="importer"><see cref="TextureImporter"/> instance.</param>
        /// <returns>Return <see cref="Vector2Int"/> value with the texture size.</returns>
        public static Vector2Int GetSourceTextureSize(this TextureImporter importer)
        {
            var args = new object[2] { 0, 0 };

            MethodInfo methodInfo = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(importer, args);

            return new Vector2Int((int)args[0], (int)args[1]);
        }
    }

}