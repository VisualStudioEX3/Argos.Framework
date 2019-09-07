using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;

public class IconExtractor : MonoBehaviour
{
    public string iconName;
    [TexturePreview]
    public Texture2D preview;
    [Button]
    public string loadIcon = "LoadIcon";

    [Space, Folder("Save folder", FolderDialogTypes.SaveFolder)]
    public string savePath;
    [Button]
    public string saveButton = "SaveIcon";

    void LoadIcon()
    {
        this.preview = EditorGUIUtility.IconContent(this.iconName)?.image as Texture2D;
    }

    void SaveIcon()
    {
        this.preview?.SaveToPNGFile($"{this.savePath}/{this.iconName}.png");
    }
}
