using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(TexturePreviewAttribute))]
    public class TexturePreviewDrawer : PropertyDrawer
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
            Rect prefixRect = position;
            prefixRect.width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(prefixRect, label);

            Rect fieldRect = position;
            fieldRect.x = fieldRect.xMax - fieldRect.height;
            fieldRect.width = fieldRect.height;

            bool allowSceneObjects = (this.attribute as TexturePreviewAttribute).AllowSceneObjects;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            switch (property.type)
            {
                case TexturePreviewDrawer.SERIALIZED_TEXTURE_TYPE:

                    property.objectReferenceValue = this.DrawPreviewField<Texture>(fieldRect, property.objectReferenceValue, allowSceneObjects);
                    break;

                case TexturePreviewDrawer.SERIALIZED_TEXTURE2D_TYPE:

                    property.objectReferenceValue = this.DrawPreviewField<Texture2D>(fieldRect, property.objectReferenceValue, allowSceneObjects);
                    break;

                case TexturePreviewDrawer.SERIALIZED_RENDER_TEXTURE_TYPE:

                    property.objectReferenceValue = this.DrawPreviewField<RenderTexture>(fieldRect, property.objectReferenceValue, allowSceneObjects);
                    break;

                case TexturePreviewDrawer.SERIALIZED_CUBEMAP_TYPE:

                    property.objectReferenceValue = this.DrawPreviewField<Cubemap>(fieldRect, property.objectReferenceValue, allowSceneObjects);
                    break;

                case TexturePreviewDrawer.SERIALIZED_SPRITE_TYPE:

                    property.objectReferenceValue = this.DrawPreviewField<Sprite>(fieldRect, property.objectReferenceValue, allowSceneObjects);
                    break;

                default:

                    throw new System.InvalidCastException($"Error to cast \"{property.displayName}\" field. TexturePreview attribute only works with Texture, Texture2D, RenderTexture, Cubemap and Sprite types.");
            }

            EditorGUI.indentLevel = indent;
        }

        UnityEngine.Object DrawPreviewField<T>(Rect position, UnityEngine.Object texture, bool allowSceneObjects) where T : UnityEngine.Object
        {
            return EditorGUI.ObjectField(position, (T)texture, typeof(T), allowSceneObjects);
        }
        #endregion
    }
}
