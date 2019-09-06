using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Utils
{
    /// <summary>
    /// Help functions to manage Unity editor skin theme.
    /// </summary>
    public static class EditorSkinUtility
    {
        #region Structs
        public readonly struct Colors
        {
            #region Constants
            public static readonly Color Bright = new Color(0.4823529f, 0.4862745f, 0.4823529f, 1f);
            public static readonly Color Dark = new Color(0.9333333f, 0.9372549f, 0.9333333f, 1f);
            #endregion
        } 

        public readonly struct Icons
        {
            #region Properties
            public static Texture2D DragIcon { get; private set; }
            #endregion

            #region Initializers
            [InitializeOnLoadMethod]
            static void Initialize()
            {
                EditorSkinUtility.Icons.DragIcon = EditorSkinUtility.Icons.CreateDragButtonIcon();
            } 
            #endregion

            #region Methods & Functions
            static Texture2D CreateDragButtonIcon()
            {
                var pixels = new Color[14 * 14];

                for (int i = 0; i < pixels.Length; i++)
                {
                    switch (i)
                    {
                        case int v when (v > 72 && v < 81) || (v > 114 && v < 123): pixels[i] = EditorSkinUtility.Colors.Bright; break;
                        case int v when v > 86 && v < 95 || (v > 128 && v < 137): pixels[i] = EditorSkinUtility.Colors.Dark; break;
                        default: pixels[i] = Color.clear; break;
                    }
                }

                var texture = new Texture2D(14, 14);
                {
                    texture.filterMode = FilterMode.Point;
                    texture.alphaIsTransparency = true;
                    texture.SetPixels(pixels);
                    texture.Apply();
                }

                return texture;
            } 
            #endregion
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current Unity editor skin.
        /// </summary>
        public static GUISkin Skin
        {
            get
            {
                return EditorGUIUtility.GetBuiltinSkin(EditorGUIUtility.isProSkin ? EditorSkin.Scene : EditorSkin.Inspector);
            }
        }
        #endregion
    }
}
