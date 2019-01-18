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

        #region Internal vars
        Rect rect;
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.depth > 0)
            {
                return base.GetPropertyHeight(property, label);
            }

            rect = EditorGUILayout.GetControlRect(true, TexturePreviewDrawer.FIELD_SIZE);

            return rect.height;
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            /* WARNING: When the attribute is used in a field inside of a data structure (serialized hierachy), the editor layout fails adding extra 
               space (the same that ocuped the property drawer) before start rendering the hierachy.

               This bug is provoked by GetPropertyHeight() function overload. 
               
               For avoid that, rendering a default field inspector for the property type target. */
            if (property.depth > 0)
            {
                EditorGUI.PropertyField(position, property);
                return;
            }

            Rect prefixRect = rect;
            prefixRect.width = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(prefixRect, label);

            Rect fieldRect = rect;
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
