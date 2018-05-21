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
        #region Consts
        const float LABEL_MIN_WIDTH = 24f;
        const float LABEL_MAX_WIDTH = 26F;
        const float FIELD_WIDTH = 40f;
        const float MARGIN = 10f;
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight * 2;
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects) return;

            var minProperty = property.FindPropertyRelative("Min");
            var maxProperty = property.FindPropertyRelative("Max");
            var minMax = attribute as MinMaxSliderAttribute ?? new MinMaxSliderAttribute(0f, 1f);

            float min = minProperty.floatValue;
            float max = maxProperty.floatValue;

            var copy = position;
            copy.height = EditorGUIUtility.singleLineHeight;
            position = copy;

            // Property label:
            {
                EditorGUI.LabelField(position, string.Format("{0}", label.text));
            }

            position.x += MARGIN;
            position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;

            // Min field:
            {
                position.width = MinMaxSliderDrawer.LABEL_MIN_WIDTH;
                EditorGUI.LabelField(position, new GUIContent("Min"));

                position.x += position.width + EditorGUIUtility.standardVerticalSpacing;
                position.width = MinMaxSliderDrawer.FIELD_WIDTH;
                min = Mathf.Clamp(EditorGUI.FloatField(position, min), minMax.Min, minMax.Max);
            }

            // Max field:
            {
                position.x = copy.xMax - MinMaxSliderDrawer.LABEL_MAX_WIDTH;
                position.width = MinMaxSliderDrawer.LABEL_MAX_WIDTH;
                EditorGUI.LabelField(position, new GUIContent("Max"));
            }

            // MinMaxSlider field:
            {
                position.x -= MinMaxSliderDrawer.FIELD_WIDTH + EditorGUIUtility.standardVerticalSpacing;
                position.width = MinMaxSliderDrawer.FIELD_WIDTH;
                max = Mathf.Clamp(EditorGUI.FloatField(position, max), minMax.Min, minMax.Max);

                position.x = copy.xMin + MinMaxSliderDrawer.MARGIN + MinMaxSliderDrawer.LABEL_MIN_WIDTH + MinMaxSliderDrawer.FIELD_WIDTH + (EditorGUIUtility.standardVerticalSpacing * 4);
                position.y -= 1f;
                position.width = copy.xMax - position.x - (MinMaxSliderDrawer.LABEL_MAX_WIDTH + MinMaxSliderDrawer.FIELD_WIDTH + (EditorGUIUtility.standardVerticalSpacing * 4));

                EditorGUI.MinMaxSlider(position, ref min, ref max, minMax.Min, minMax.Max);
            }

            minProperty.floatValue = min;
            maxProperty.floatValue = max;
        }
        #endregion
    }
}