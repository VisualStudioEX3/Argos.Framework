using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : ArgosPropertyDrawerBase
    {
        #region Constants
        const float FIELD_WIDTH = 50f;
        const float FIELD_SEPARATOR = 5f;
        #endregion

        #region Internal vars
        Rect labelRect, minFieldRect, maxFieldRect, minMaxSliderRect;
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:

                    return true;
                
                default:

                    return false;
            }
        }

        void CalculateControlRects(Rect position)
        {
            Rect indentedPosition = EditorGUI.IndentedRect(position);

            this.labelRect = indentedPosition;
            {
                this.labelRect.x = indentedPosition.xMin;
                this.labelRect.width = EditorGUIUtility.labelWidth;
            }

            EditorGUI.indentLevel = 0;

            this.minFieldRect = position;
            {
                this.minFieldRect.width = MinMaxSliderDrawer.FIELD_WIDTH;
                this.minFieldRect.x = position.xMin + EditorGUIUtility.labelWidth;
#if UNITY_2019_3_OR_NEWER
                this.minFieldRect.x += 2f;
#endif
            }

            this.maxFieldRect = position;
            {
                this.maxFieldRect.width = MinMaxSliderDrawer.FIELD_WIDTH;
                this.maxFieldRect.x = position.xMax - this.maxFieldRect.width;
            }

            this.minMaxSliderRect = position;
            {
                this.minMaxSliderRect.x = this.minFieldRect.x + this.minFieldRect.width + MinMaxSliderDrawer.FIELD_SEPARATOR;
                this.minMaxSliderRect.width = (this.maxFieldRect.xMin - this.minFieldRect.xMax) - (MinMaxSliderDrawer.FIELD_SEPARATOR * 2f);
            }
        }

        void DrawControls(MinMaxSliderAttribute attribute, GUIContent label, ref Vector2 value, bool isVector2Int)
        {
            EditorGUI.PrefixLabel(labelRect, label);

            EditorGUI.BeginChangeCheck();
            {
                EditorGUI.MinMaxSlider(this.minMaxSliderRect, ref value.x, ref value.y, attribute.range.x, attribute.range.y);
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorGUI.FocusTextInControl(string.Empty);
            }

            if (isVector2Int)
            {
                value.x = this.DrawField(this.minFieldRect, (int)value.x, (int)attribute.range.x, (int)attribute.range.y);
                value.y = this.DrawField(this.maxFieldRect, (int)value.y, (int)attribute.range.x, (int)attribute.range.y);
            }
            else
            {
                value.x = this.DrawField(this.minFieldRect, value.x, attribute.range.x, attribute.range.y);
                value.y = this.DrawField(this.maxFieldRect, value.y, attribute.range.x, attribute.range.y);
            }
        }

        float DrawField(Rect position, float value, float min, float max)
        {
            return Mathf.Clamp(EditorGUI.FloatField(position, value), min, max);
        }

        int DrawField(Rect position, int value, int min, int max)
        {
            return Mathf.Clamp(EditorGUI.IntField(position, value), min, max);
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool isVector2Int = (property.propertyType == SerializedPropertyType.Vector2Int);
            Vector2 vector = isVector2Int ? property.vector2IntValue : property.vector2Value;
            var minMax = (MinMaxSliderAttribute)attribute;

            this.CalculateControlRects(position);
            this.DrawControls(minMax, label, ref vector, isVector2Int);

            if (isVector2Int)
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