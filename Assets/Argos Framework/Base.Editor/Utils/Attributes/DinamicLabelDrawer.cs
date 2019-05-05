using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(DinamicLabelAttribute))]
    public class DinamicLabelDrawer : ArgosPropertyDrawerBase
    {
        #region Internal vars
        DinamicLabelAttribute _attribute;
        GUIStyle _style;
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.String;
        }

        public override float GetCustomHeight(SerializedProperty property, GUIContent label)
        {
            this._attribute = (DinamicLabelAttribute)attribute;
            this._style = this._attribute.miniLabel ? EditorStyles.wordWrappedMiniLabel : EditorStyles.wordWrappedLabel;
            this._style.richText = true;

            return this._style.CalcHeight(new GUIContent(property.stringValue), EditorGUIUtility.currentViewWidth);
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (this._attribute.selectable)
            {
                EditorGUI.SelectableLabel(position, property.stringValue, this._style);
            }
            else
            {
                EditorGUI.LabelField(position, property.stringValue, this._style);
            }
        }
        #endregion
    }
}