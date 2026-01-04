using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;

[CustomEditor(typeof(ComponentTest))]
public class ComponentTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        this.serializedObject.Update();
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUI.indentLevel++;
                this.serializedObject.FindProperty("audioSource").objectReferenceValue.DrawNativeComponentInspector();
            }
            EditorGUILayout.EndVertical();
        }
        this.serializedObject.ApplyModifiedProperties();
    }
}

