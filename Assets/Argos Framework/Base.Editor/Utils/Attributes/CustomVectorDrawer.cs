using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(CustomVectorAttribute))]
    public class CustomVectorDrawer : ArgosPropertyDrawerBase
    {
        #region Constants
        const float VECTOR2_FIELD_WITDH_CORRECTION = 1.5f;
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:

                    return true;

                default:

                    return false;
            }
        }

        bool CheckNameCount(SerializedProperty property, CustomVectorAttribute attribute)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector2Int:

                    return attribute.names.Length == 2;

                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector3Int:

                    return attribute.names.Length == 3;

                case SerializedPropertyType.Vector4:

                    return attribute.names.Length == 4;

                default:

                    return false;
            }
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var vectorAttribute = (CustomVectorAttribute)attribute;

            if (!this.CheckNameCount(property, vectorAttribute))
            {
                this.Log($"Missing names for CustomVector attribute. Ensured to match the names array elements with the right vector type. \nMember target: \"{label.text}\"", 
                         LogLevel.Error, 
                         property.serializedObject.targetObject);
                return;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            {
                var contentPosition = EditorGUI.PrefixLabel(position, label);

                contentPosition.xMin--;

                if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector2Int)
                {
                    contentPosition.width /= CustomVectorDrawer.VECTOR2_FIELD_WITDH_CORRECTION;
                }

                EditorGUI.MultiPropertyField(contentPosition, vectorAttribute.names, property.FindPropertyRelative("x"));
            }
            EditorGUI.EndProperty();
        }
        #endregion
    }
}