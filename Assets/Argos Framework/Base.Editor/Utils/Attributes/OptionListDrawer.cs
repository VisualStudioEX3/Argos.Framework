using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
            bool listHasSplited = false;

            EditorGUI.LabelField(position, label);
            EditorGUI.indentLevel++;

            position.width = EditorGUIUtility.currentViewWidth * 0.5f;
            position.height = EditorGUIUtility.singleLineHeight;

            for (int i = 0; i < property.enumDisplayNames.Length; i++)
            {
                if (this._split && !listHasSplited && i > this._splitCount)
                {
                    position = initial;
                    position.x += position.width;
                    listHasSplited = true;
                }

                position.y += EditorGUIUtility.singleLineHeight;
                if (EditorGUI.ToggleLeft(position, property.enumDisplayNames[i], property.enumValueIndex == i))
                {
                    property.enumValueIndex = i;
                }
            }
        }
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            this._split = ((OptionListAttribute)this.attribute).Split;

            if (this._split)
            {
                this._splitCount = (int)(property.enumDisplayNames.Length * 0.5f);

                if (property.enumDisplayNames.Length % 2 == 0)
                {
                    this._splitCount--;
                }

                height += (this._splitCount * EditorGUIUtility.singleLineHeight) + EditorGUIUtility.singleLineHeight;
            }
            else
            {
                height += (property.enumDisplayNames.Length * EditorGUIUtility.singleLineHeight);
            }

            height += (EditorGUIUtility.standardVerticalSpacing * 2f);

            return height;
        }
        #endregion
    }
}
