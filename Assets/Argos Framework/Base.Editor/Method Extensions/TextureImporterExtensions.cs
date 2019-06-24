using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

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
        object[] args = new object[2] { 0, 0 };

        MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
        mi.Invoke(importer, args);

        return new Vector2Int((int)args[0], (int)args[1]);
    }
}
