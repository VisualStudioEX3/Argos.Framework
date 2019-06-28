using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Base PropertyDrawer class that implement internal type check for target field that the attribute is associated and show visual feedback on inspectors.
    /// </summary>
    public abstract class ArgosPropertyDrawerBase : PropertyDrawer
    {
        #region Constants
        const string DEFAULT_ERROR_MESSAGE = "<color=red>Attribute error! Variable type is not supported!</color>"; 
        #endregion

        #region Internal vars
        GUIStyle _errorMessageStyle;
        string _tooltip = string.Empty;
        #endregion
        
        #region Properties
        /// <summary>
        /// The type of the field.
        /// </summary>
        /// <remarks>A simple shortcut of <see cref="PropertyDrawer.fieldInfo.FieldType"/>.</remarks>
        public Type FieldType { get { return this.fieldInfo.FieldType; } }
        #endregion

        #region Methods & Functions
        string GetTooltipAttributeValue()
        {
            if (string.IsNullOrEmpty(this._tooltip))
            {
                var customAttributes = this.fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), false);
                if (customAttributes.Length > 0)
                {
                    this._tooltip = (customAttributes[0] as TooltipAttribute).tooltip;
                }
            }

            return this._tooltip;
        }

        /// <summary>
        /// Use this function to evaluate if the field type is correct.
        /// </summary>
        /// <param name="property">Serialized property that has the field content.</param>
        /// <returns>Return true if the type is valid.</returns>
        /// <remarks>For basic type checkings, use <see cref="SerializedProperty.propertyType"/> value.</remarks>
        public virtual bool CheckPropertyType(SerializedProperty property)
        {
            return true;
        }

        /// <summary>
        /// Use this function to customize the error type message displayed in the GUI.
        /// </summary>
        /// <returns>Return a string message.</returns>
        public virtual string GetErrorTypeMessage()
        {
            return ArgosPropertyDrawerBase.DEFAULT_ERROR_MESSAGE;
        }

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return this.CheckPropertyType(property) ? this.GetCustomHeight(property, label) : EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Use this function to customize the property height (like the original <see cref="PropertyDrawer.GetPropertyHeight(SerializedProperty, GUIContent)"/> function).
        /// </summary>
        /// <param name="property">Serialized property that has the field content.</param>
        /// <param name="label">The field label displayed on GUI.</param>
        /// <returns>Return the property height.</returns>
        public virtual float GetCustomHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        #endregion

        #region Event listeners
        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = this.GetTooltipAttributeValue();

            if (this.CheckPropertyType(property))
            {
                this.OnCustomGUI(position, property, label);
            }
            else
            {
                if (this._errorMessageStyle == null)
                {
                    this._errorMessageStyle = new GUIStyle(EditorStyles.boldLabel);
                    this._errorMessageStyle.alignment = TextAnchor.MiddleRight;
                    this._errorMessageStyle.richText = true;
                }

                EditorGUI.LabelField(position, label, new GUIContent(this.GetErrorTypeMessage()), this._errorMessageStyle);
            }
        }

        /// <summary>
        /// Use this event to customize the property content (like the original <see cref="PropertyDrawer.OnGUI(Rect, SerializedProperty, GUIContent)"/> event).
        /// </summary>
        /// <param name="position">The area ocupped by the property content.</param>
        /// <param name="property">Serialized property that has the field content.</param>
        /// <param name="label">The field label displayed on GUI.</param>
        public virtual void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        } 
        #endregion
    }
}