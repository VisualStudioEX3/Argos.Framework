using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;

public class TexturePreviewFieldTest : MonoBehaviour
{
    [TexturePreview(true)]
    public Texture Texture;
    [TexturePreview]
    public Texture2D Texture2D;
    [TexturePreview]
    public RenderTexture RenderTexture;
    [TexturePreview]
    public Cubemap CubeMap;
    [TexturePreview]
    public Sprite Sprite;
    [TexturePreview]
    public string Puta;
}