using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagDrawer : ArgosPropertyDrawerBase
    {
        #region Constants
        const string DEFAULT_TAG = "Untagged";
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.String;
        } 
        #endregion

        #region Events
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TagField(position, label, string.IsNullOrEmpty(property.stringValue.Trim()) ? TagDrawer.DEFAULT_TAG : property.stringValue);
        }
        #endregion
    }
}