using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(TexturePreviewAttribute))]
    public class TexturePreviewDrawer : ArgosPropertyDrawerBase
    {
        #region Constants
        static readonly float FIELD_SIZE = EditorGUIUtility.singleLineHeight * 4f;
        #endregion

        #region Methods & Functions
        public override float GetCustomHeight(SerializedProperty property, GUIContent label)
        {
            return TexturePreviewDrawer.FIELD_SIZE;
        }

        public override bool CheckPropertyType(SerializedProperty property)
        {
            return this.FieldType == typeof(Texture) ||
                   this.FieldType == typeof(Texture2D) ||
                   this.FieldType == typeof(RenderTexture) ||
                   this.FieldType == typeof(Cubemap) ||
                   this.FieldType == typeof(Sprite);
        }
        #endregion

        #region Events
        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            // This ensure that the texture always is rendered.
            // Source: https://answers.unity.com/questions/1311926/texture2d-in-scriptableobjects-property-drawer-exp.html?childToView=1401566#answer-1401566
            return false;
        }

        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect prefixRect = position;
            prefixRect.width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(prefixRect, label);

            Rect fieldRect = position;
            fieldRect.x = fieldRect.xMax - fieldRect.height;
            fieldRect.width = fieldRect.height;

            bool allowSceneObjects = (this.attribute as TexturePreviewAttribute).AllowSceneObjects;

            int indent = EditorGUI.indentLevel;
            {
                EditorGUI.indentLevel = 0;
                property.objectReferenceValue = EditorGUI.ObjectField(fieldRect, property.objectReferenceValue, this.FieldType, allowSceneObjects);
            }
            EditorGUI.indentLevel = indent;
        }
        #endregion
    }
}
