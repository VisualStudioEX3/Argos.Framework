using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    public abstract class TextFieldWithButtonDrawerBase : PropertyDrawer
    {
        #region Constants
        const string BUTTON_LABEL = "...";
        const float BUTTON_WIDTH = 22f;
        #endregion

        #region Events
        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect fieldRect = position;
            fieldRect.width -= TextFieldWithButtonDrawerBase.BUTTON_WIDTH + 2f;
            EditorGUI.PropertyField(fieldRect, property, label);

            Rect browseButtonRect = position;
            browseButtonRect.x = browseButtonRect.xMax - TextFieldWithButtonDrawerBase.BUTTON_WIDTH;
            browseButtonRect.width = TextFieldWithButtonDrawerBase.BUTTON_WIDTH;

            if (GUI.Button(browseButtonRect, TextFieldWithButtonDrawerBase.BUTTON_LABEL, EditorStyles.miniButton))
            {
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
