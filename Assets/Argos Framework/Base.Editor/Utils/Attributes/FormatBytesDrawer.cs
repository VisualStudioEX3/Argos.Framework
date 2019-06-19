using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(FormatBytesAttribute))]
    public class FormatBytesDrawer : ArgosPropertyDrawerBase
    {
        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Integer ||
                   property.propertyType == SerializedPropertyType.Float;
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            long value = (long)(property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue);
            EditorGUI.LabelField(position, label, EditorUtility.FormatBytes(value));
        }
        #endregion
    }
}