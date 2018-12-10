using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerDrawer : PropertyDrawer
    {
        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
        #endregion
    }
}