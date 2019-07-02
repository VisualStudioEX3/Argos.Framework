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

            switch (this._attribute.size)
            {
                case GUIButtonSize.Large: return ButtonDrawer.LARGE_BUTTON_SIZE;
                case GUIButtonSize.Mini: return ButtonDrawer.MINI_BUTTON_SIZE;
                default: return ButtonDrawer.DEFAULT_BUTTON_SIZE;
            }
        }

        GUIContent GetButtonLabel(GUIContent label)
        {
            return string.IsNullOrEmpty(this._attribute.customLabel) ? label : new GUIContent(this._attribute.customLabel);
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = this._attribute.disableOn == GUIButtonDisableEvents.Never ||
                          (this._attribute.disableOn == GUIButtonDisableEvents.EditorMode && EditorApplication.isPlaying) ||
                          (this._attribute.disableOn == GUIButtonDisableEvents.PlayMode && !EditorApplication.isPlaying);

            if (GUI.Button(EditorGUI.IndentedRect(position), this.GetButtonLabel(label), this._attribute.size == GUIButtonSize.Mini ? EditorStyles.miniButton : GUI.skin.button))
            {
                if (!string.IsNullOrEmpty(property.stringValue))
                {
                    (property.serializedObject.targetObject as MonoBehaviour).Invoke(property.stringValue, 0f);

                    if (!EditorApplication.isPlaying && !EditorApplication.isCompiling)
                    {
                        EditorUtility.SetDirty(property.serializedObject.targetObject); // Force in edit mode to update the Monobehaviour Update logic (needed to Invoke() call can be executed).
                    } 
                }
                else
                {
                    Debug.LogWarning("ButtonAttribute: The target method name is empty! Check if the string variable has value!");
                }
            }

            GUI.enabled = true;
        }
        #endregion
    }
}
