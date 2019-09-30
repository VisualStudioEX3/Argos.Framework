using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;
using Argos.Framework.Utils;

public class ExtractUnityEditorGUISkin : MonoBehaviour
{
    public string style;
    [TexturePreview] public Texture normal;
    [TexturePreview] public Texture focused;
    [TexturePreview] public Texture active;
    [TexturePreview] public Texture hover;
}

[CustomEditor(typeof(ExtractUnityEditorGUISkin))]
public class ExtractUnityEditorGUISkinEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ExtractUnityEditorGUISkin t = this.target as ExtractUnityEditorGUISkin;

        if (GUILayout.Button("Get textures"))
        {
            GUIStyle style = EditorSkinUtility.Skin.FindStyle(t.style);
            // style.active;
            //style.focused;
            //style.hover;
            //style.normal;
        }
    }
}