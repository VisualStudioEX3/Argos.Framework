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
        const string SERIALIZED_TEXTURE_TYPE = "PPtr<$Texture>";
        const string SERIALIZED_TEXTURE2D_TYPE = "PPtr<$Texture2D>";
        const string SERIALIZED_RENDER_TEXTURE_TYPE = "PPtr<$RenderTexture>";
        const string SERIALIZED_CUBEMAP_TYPE = "PPtr<$Cubemap>";
        const string SERIALIZED_SPRITE_TYPE = "PPtr<$Sprite>";

        static readonly float FIELD_SIZE = EditorGUIUtility.singleLineHeight * 4f;
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return TexturePreviewDrawer.FIELD_SIZE;
        }

        public override bool CheckPropertyType()
        {
            return this.fieldInfo.FieldType == typeof(Texture) ||
                   this.fieldInfo.FieldType == typeof(Texture2D) ||
                   this.fieldInfo.FieldType == typeof(RenderTexture) ||
                   this.fieldInfo.FieldType == typeof(Cubemap) ||
                   this.fieldInfo.FieldType == typeof(Sprite);
        }
        #endregion

        #region Events
        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            // This ensure that the texture always is rendered.
            // Source: https://answers.unity.com/questions/1311926/texture2d-in-scriptableobjects-property-drawer-exp.html?childToView=1401566#answer-1401566
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!this.CheckPropertyType())
            {
                this.PrintErrorMessage(position, label);
                return;
            }

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
                property.objectReferenceValue = EditorGUI.ObjectField(fieldRect, property.objectReferenceValue, this.fieldInfo.FieldType, allowSceneObjects);
            }
            EditorGUI.indentLevel = indent;
        }
        #endregion
    }
}
