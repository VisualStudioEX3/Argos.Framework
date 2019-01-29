using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Argos.Framework
{
    public class LabeledTextAreaDrawer : PropertyDrawer
    {
        #region Internal vars
        GUISkin _editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
        LabeledTextAreaAttribute _attribute; 
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelRect = position;
            position.width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(position, label);

            Rect rect = position;
            float delta = 7f; // editorSkin.textArea.padding.horizontal;
            rect.x -= delta;
            rect.width += delta;

            property.stringValue = EditorGUI.TextArea(rect, property.stringValue);
        } 
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            this._attribute = (LabeledTextAreaAttribute)this.attribute;

            return (this._editorSkin.textArea.lineHeight * this._attribute.Lines) + this._editorSkin.textArea.margin.vertical;
        } 
        #endregion
    } 
}
