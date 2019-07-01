using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(FormatBytesAttribute))]
    public class FormatBytesDrawer : ArgosPropertyDrawerBase
    {
        #region Constants
        const int SHOW_BYTES_RESUME_AFTER = 512; 
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Integer;
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string value = EditorUtility.FormatBytes(property.intValue);

            if ((attribute as FormatBytesAttribute).showBytes && property.intValue >= FormatBytesDrawer.SHOW_BYTES_RESUME_AFTER)
            {
                value += $" ({property.intValue:#,##0} B)";
            }

            EditorGUI.LabelField(position, label, new GUIContent(value));
        }
        #endregion
    }
}