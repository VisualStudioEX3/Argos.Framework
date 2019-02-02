using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    public abstract class TextFieldWithButtonDrawerBase : ArgosPropertyDrawerBase
    {
        #region Constants
        public const string BUTTON_LABEL = "...";
        public const float BUTTON_WIDTH = 22f;
        #endregion

        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.String;
        }

        #region Events
        public sealed override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect fieldRect = position;
            fieldRect.width -= TextFieldWithButtonDrawerBase.BUTTON_WIDTH + 2f;
            EditorGUI.PropertyField(fieldRect, property, label);

            Rect browseButtonRect = position;
            browseButtonRect.x = browseButtonRect.xMax - TextFieldWithButtonDrawerBase.BUTTON_WIDTH;
            browseButtonRect.width = TextFieldWithButtonDrawerBase.BUTTON_WIDTH;

            if (GUI.Button(browseButtonRect, TextFieldWithButtonDrawerBase.BUTTON_LABEL, EditorStyles.miniButton))
            {
                EditorGUIUtility.editingTextField = false;
                this.OnButtonClick(property);
            }
        }

        /// <summary>
        /// Event raised when the user click on the button.
        /// </summary>
        public abstract void OnButtonClick(SerializedProperty property);
        #endregion
    } 
}
