using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Argos.Framework
{
    /// <summary>
    /// Texture2D method extensions.
    /// </summary>
    public static class Texture2DExtensions
    {
        #region Methods & Functions
        /// <summary>
        /// Convert to Sprite.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/> instance.</param>
        /// <returns>Return a <see cref="Sprite"/> instance with the <see cref="Texture2D"/> data with pivot on 0x 0y.</returns>
        public static Sprite ToSprite(this Texture2D texture)
        {
            return ToSprite(texture, Vector2.zero);
        }

        /// <summary>
        /// Convert to Sprite.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/> instance.</param>
        /// <param name="pivot">Pivot position.</param>
        /// <returns>Return a <see cref="Sprite"/> instance with <see cref="Texture2D"/> data.</returns>
        public static Sprite ToSprite(this Texture2D texture, Vector2 pivot)
        {
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), pivot);
        } 

        static void SaveToFile(string filename, byte[] encodedPixels)
        {
            File.WriteAllBytes(filename, encodedPixels);
        }

        /// <summary>
        /// Save to EXR file format.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/> instance.</param>
        /// <param name="filename">Filename for the new EXR file.</param>
        /// <param name="flags">Optional flags to compress the output EXR file. By default is <see cref="Texture2D.EXRFlags.None"/>.</param>
        public static void SaveToEXRFile(this Texture2D texture, string filename, Texture2D.EXRFlags flags = Texture2D.EXRFlags.None)
        {
            Texture2DExtensions.SaveToFile(filename, texture.EncodeToEXR(flags));
        }

        /// <summary>
        /// Save to JPEG file format.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/> instance.</param>
        /// <param name="filename">Filename for the new JPEG file.</param>
        /// <param name="quality">Optional quality level. by default is 75.</param>
        public static void SaveToJPGFile(this Texture2D texture, string filename, int quality = 75)
        {
            Texture2DExtensions.SaveToFile(filename, texture.EncodeToJPG(Mathf.Clamp(quality, 0, 100)));
        }

        /// <summary>
        /// Save to PNG file format.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/> instance.</param>
        /// <param name="filename">Filename for the new PNG file.</param>
        public static void SaveToPNGFile(this Texture2D texture, string filename)
        {
            Texture2DExtensions.SaveToFile(filename, texture.EncodeToPNG());
        }

        /// <summary>
        /// Save to TGA file format.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/> instance.</param>
        /// <param name="filename">Filename for the new TGA file.</param>
        public static void SaveToTGAFile(this Texture2D texture, string filename)
        {
            Texture2DExtensions.SaveToFile(filename, texture.EncodeToTGA());
        }

        /// <summary>
        /// Load a JPEG/PNG texture from disk.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/> instance.</param>
        /// <param name="filename">Filename of the JPEG/PNG file to load.</param>
        /// <param name="markNonReadable">Set to false by default, pass true to optionally mark the texture as non-readable.</param>
        /// <returns>Return a new <see cref="Texture2D"/> object with the texture file data. 
        /// Throw a <see cref="FormatException"/> if the file is not loaded (maybe if trying to load a file that not is a JPEG or PNG image format).</returns>
        public static Texture2D LoadImageFile(this Texture2D texture, string filename, bool markNonReadable)
        {
            var texture2D = new Texture2D(0, 0);

            if (!texture2D.LoadImage(File.ReadAllBytes(filename), markNonReadable))
            {
                throw new FormatException("Texture2D.LoadImageFile: Fail to load image file, maybe is not a JPEG/PNG format.");
            }

            return texture2D;
        }
        #endregion
    }
}