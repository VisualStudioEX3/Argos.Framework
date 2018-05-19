using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework
{
    /// <summary>
    /// Disabled field for edit values from the editor.
    /// 
    /// Usage:
    ///      [ReadOnly]
    ///      public int myVar;
    ///      
    /// Source: http://www.jeffpizano.com/blog/2015/06/read-only-properties-in-unity3d-inspector/
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    } 
#endif
}