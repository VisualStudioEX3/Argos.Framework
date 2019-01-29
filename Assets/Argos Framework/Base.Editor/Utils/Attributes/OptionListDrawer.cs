using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework.Helpers;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(OptionListAttribute))]
    public class OptionListDrawer : PropertyDrawer
    {
        #region Internal vars
        bool _split;
        int _splitCount;
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect initial = position;

            EditorGUI.LabelField(position, label);

            EditorGUI.indentLevel++;
            {
                if (this._split)
                {
                    position.width = EditorGUIUtility.currentViewWidth * 0.5f;
                }

                position.height = EditorGUIUtility.singleLineHeight;

                for (int i = 0; i < property.enumDisplayNames.Length; i++)
                {
                    position.x = initial.x;

                    if (i % 2 == 0 || !this._split)
                    {
                        position.y += EditorGUIUtility.singleLineHeight;
                    }
                    else
                    {
                        position.x += position.width;
                    }

                    this.DrawLeftToggleField(position, property, i);
                }
            }
            EditorGUI.indentLevel--;
        }
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            this._split = ((OptionListAttribute)this.attribute).Split;

            if (this._split)
            {
                this._splitCount = (int)(MathHelper.ForceEvenValue(property.enumDisplayNames.Length) * 0.5f);

                height += (this._splitCount * EditorGUIUtility.singleLineHeight);
            }
            else
            {
                height += (property.enumDisplayNames.Length * EditorGUIUtility.singleLineHeight);
            }

            height += EditorGUIUtility.standardVerticalSpacing;

            return height;
        }

        void DrawLeftToggleField(Rect position, SerializedProperty property, int index)
        {
            if (EditorGUI.ToggleLeft(position, property.enumDisplayNames[index], property.enumValueIndex == index))
            {
                property.enumValueIndex = index;
            }
        }
        #endregion
    }
}
