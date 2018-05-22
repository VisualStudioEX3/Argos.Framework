using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        #region Constants
        const float FIELD_WIDTH = 50f;
        const float FIELD_SEPARATOR = 5f;
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects) return;

            Vector2 vector = property.type == "Vector2Int" ? property.vector2IntValue : property.vector2Value;
            var minMax = (MinMaxSliderAttribute)attribute ?? new MinMaxSliderAttribute(0f, 1f);
            int currentIndent = EditorGUI.indentLevel;
            
            Rect labelRect = position;
            {
                labelRect.width = EditorGUIUtility.labelWidth;
                labelRect.height = EditorGUIUtility.singleLineHeight;
            }
            EditorGUI.PrefixLabel(labelRect, label);

            EditorGUI.indentLevel = 0;

            Rect minFieldRect = position;
            {
                minFieldRect.width = MinMaxSliderDrawer.FIELD_WIDTH;
                minFieldRect.height = EditorGUIUtility.singleLineHeight;
                minFieldRect.x = position.x + EditorGUIUtility.labelWidth;
            }
            vector.x = Mathf.Clamp(EditorGUI.FloatField(minFieldRect, vector.x), minMax.Range.x, minMax.Range.y);

            Rect maxFieldRect = position;
            {
                maxFieldRect.width = MinMaxSliderDrawer.FIELD_WIDTH;
                maxFieldRect.height = EditorGUIUtility.singleLineHeight;
                maxFieldRect.x = position.xMax - maxFieldRect.width;
            }
            vector.y = Mathf.Clamp(EditorGUI.FloatField(maxFieldRect, vector.y), minMax.Range.x, minMax.Range.y);

            Rect minMaxSliderRect = position;
            {
                minMaxSliderRect.x = minFieldRect.x + minFieldRect.width + MinMaxSliderDrawer.FIELD_SEPARATOR;
                minMaxSliderRect.width = (maxFieldRect.xMin - minFieldRect.xMax) - (MinMaxSliderDrawer.FIELD_SEPARATOR * 2f);
                minMaxSliderRect.height = EditorGUIUtility.singleLineHeight;
            }
            EditorGUI.MinMaxSlider(minMaxSliderRect, ref vector.x, ref vector.y, minMax.Range.x, minMax.Range.y);

            EditorGUI.indentLevel = currentIndent;

            if (property.type == "Vector2Int")
            {
                property.vector2IntValue = new Vector2Int((int)vector.x, (int)vector.y);
            }
            else
            {
                property.vector2Value = new Vector2((float)System.Math.Round((double)vector.x, 2), (float)System.Math.Round((double)vector.y, 2));
            }
        }
        #endregion
    }
}