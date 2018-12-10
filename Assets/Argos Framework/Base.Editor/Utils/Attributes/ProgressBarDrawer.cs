using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarDrawer : PropertyDrawer
    {
        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var progressBarAttribute = (ProgressBarAttribute)attribute;
            Rect rect = progressBarAttribute.ShowLabel ? EditorGUI.PrefixLabel(position, label) : EditorGUI.IndentedRect(position);

            EditorGUI.ProgressBar(rect, property.floatValue, progressBarAttribute.Message);
        }
        #endregion
    }
}