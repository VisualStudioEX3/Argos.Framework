using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.Input.Attributes
{
    [CustomPropertyDrawer(typeof(InputElementNameFieldAttribute))]
    public class InputElementNameFieldDrawer : ArgosPropertyDrawerBase
    {
        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.String;
        }

        public override float GetCustomHeight(SerializedProperty property, GUIContent label)
        {
            return string.IsNullOrEmpty(property.stringValue) ? 0f : base.GetCustomHeight(property, label) + EditorGUIUtility.standardVerticalSpacing * 4f;
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!string.IsNullOrEmpty(property.stringValue))
            {
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.DelayedTextField(position, property, label);
            }
        } 
        #endregion
    } 
}
