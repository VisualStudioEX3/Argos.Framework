using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(MinMaxSlider))]
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        #region Constants
        const float FIELD_WIDTH = 50f;
        const float SEPARATOR = 5f;
        const float MIN_FIELD_X_CORRECTION = 14f;
        const float SLIDER_WIDTH_CORRECTION = 130f;
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects) return;

            var minProperty = property.FindPropertyRelative("Min");
            var maxProperty = property.FindPropertyRelative("Max");
            var minMax = (MinMaxSliderAttribute)attribute ?? new MinMaxSliderAttribute(0f, 1f);

            float min = minProperty.floatValue;
            float max = maxProperty.floatValue;

            int currentIndent = EditorGUI.indentLevel;
            
            Rect labelRect = position;
            labelRect.width = EditorGUIUtility.labelWidth;
            labelRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PrefixLabel(labelRect, new GUIContent(label.text));

            EditorGUI.indentLevel = 0;

            Rect minFieldRect = position;
            minFieldRect.width = MinMaxSliderDrawer.FIELD_WIDTH;
            minFieldRect.height = EditorGUIUtility.singleLineHeight;
            minFieldRect.x = EditorGUIUtility.labelWidth + MinMaxSliderDrawer.MIN_FIELD_X_CORRECTION;
            min = Mathf.Clamp(EditorGUI.FloatField(minFieldRect, min), minMax.Min, minMax.Max);

            Rect maxFieldRect = position;
            maxFieldRect.width = MinMaxSliderDrawer.FIELD_WIDTH;
            maxFieldRect.height = EditorGUIUtility.singleLineHeight;
            maxFieldRect.x = EditorGUIUtility.currentViewWidth - maxFieldRect.width - MinMaxSliderDrawer.SEPARATOR;
            max = Mathf.Clamp(EditorGUI.FloatField(maxFieldRect, max), minMax.Min, minMax.Max);

            Rect minMaxSliderRect = position;
            minMaxSliderRect.x = minFieldRect.x + minFieldRect.width + MinMaxSliderDrawer.SEPARATOR;
            minMaxSliderRect.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - SLIDER_WIDTH_CORRECTION;
            minMaxSliderRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.MinMaxSlider(minMaxSliderRect, ref min, ref max, minMax.Min, minMax.Max);

            EditorGUI.indentLevel = currentIndent;

            minProperty.floatValue = (float)System.Math.Round((double)min, 2);
            maxProperty.floatValue = (float)System.Math.Round((double)max, 2);
        }
        #endregion
    }
}