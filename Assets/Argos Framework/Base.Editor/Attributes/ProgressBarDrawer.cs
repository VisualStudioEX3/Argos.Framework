using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarDrawer : ArgosPropertyDrawerBase
    {
        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Float;
        } 
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var progressBarAttribute = (ProgressBarAttribute)attribute;
            Rect rect = progressBarAttribute.showLabel ? EditorGUI.PrefixLabel(position, label) : EditorGUI.IndentedRect(position);

            EditorGUI.ProgressBar(rect, property.floatValue, progressBarAttribute.message);
        }
        #endregion
    }
}