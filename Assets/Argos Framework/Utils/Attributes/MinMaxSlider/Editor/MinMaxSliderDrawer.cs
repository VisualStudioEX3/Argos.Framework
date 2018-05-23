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

        #region Internal vars
        Rect labelRect, minFieldRect, maxFieldRect, minMaxSliderRect;
        #endregion

        #region Methods & Functions
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

        void DrawControls(MinMaxSliderAttribute attribute, GUIContent label, ref Vector2 value)
        {
            EditorGUI.PrefixLabel(labelRect, label);

            EditorGUI.BeginChangeCheck();
            {
                EditorGUI.MinMaxSlider(this.minMaxSliderRect, ref value.x, ref value.y, attribute.Range.x, attribute.Range.y);
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorGUI.FocusTextInControl(string.Empty);
            }

            value.x = Mathf.Clamp(EditorGUI.FloatField(this.minFieldRect, value.x), attribute.Range.x, attribute.Range.y);
            value.y = Mathf.Clamp(EditorGUI.FloatField(this.maxFieldRect, value.y), attribute.Range.x, attribute.Range.y);
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool isVector2Int = (property.propertyType == SerializedPropertyType.Vector2Int);
            Vector2 vector = isVector2Int ? property.vector2IntValue : property.vector2Value;
            var minMax = (MinMaxSliderAttribute)attribute;

            this.CalculateControlRects(position);
            this.DrawControls(minMax, label, ref vector);

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