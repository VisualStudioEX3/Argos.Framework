using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : ArgosPropertyDrawerBase
    {
        #region Constants
        static readonly float DEFAULT_BUTTON_SIZE = EditorGUIUtility.singleLineHeight + (EditorGUIUtility.standardVerticalSpacing * 2f);
        static readonly float LARGE_BUTTON_SIZE = EditorGUIUtility.singleLineHeight * 2f;
        static readonly float MINI_BUTTON_SIZE = EditorGUIUtility.singleLineHeight;
        #endregion;

        #region Intenral vars
        ButtonAttribute _attribute;
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.String;
        }

        public override float GetCustomHeight(SerializedProperty property, GUIContent label)
        {
            if (this._attribute == null)
            {
                this._attribute = (ButtonAttribute)this.attribute;
            }

            switch (this._attribute.Size)
            {
                case GUIButtonSize.Large:

                    return ButtonDrawer.LARGE_BUTTON_SIZE;

                case GUIButtonSize.Mini:

                    return ButtonDrawer.MINI_BUTTON_SIZE;

                default:

                    return ButtonDrawer.DEFAULT_BUTTON_SIZE;
            }
        }

        string GetButtonLabel(GUIContent label)
        {
            return string.IsNullOrEmpty(this._attribute.CustomLabel) ? label.text : this._attribute.CustomLabel;
        }
        #endregion

        #region Events
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (GUI.Button(EditorGUI.IndentedRect(position), this.GetButtonLabel(label), this._attribute.Size == GUIButtonSize.Mini ? EditorStyles.miniButton : GUI.skin.button))
            {
                (property.serializedObject.targetObject as MonoBehaviour).Invoke(property.stringValue, 0f);

                if (!EditorApplication.isPlaying)
                {
                    EditorUtility.SetDirty(property.serializedObject.targetObject); // Force in edit mode to update the Monobehaviour Update logic (needed to Invoke() call can be executed).
                }
            }
        }
        #endregion
    }
}
