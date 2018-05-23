using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Attribute used to set custom names to a Vector2, Vector2Int, Vector3, Vector3Int or Vector4 variable in a script.
/// </summary>
public class CustomVectorAttribute : PropertyAttribute
{
    #region Public vars
    public readonly GUIContent[] Names;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="names">Names of each vector element (one character).</param>
    public CustomVectorAttribute(params string[] names)
    {
        this.Names = new GUIContent[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            this.Names[i] = new GUIContent(names[i]);
        }
    } 
    #endregion
}

[CustomPropertyDrawer(typeof(CustomVectorAttribute))]
public class CustomVectorDrawer : PropertyDrawer
{
    #region Methods & Functions
    bool CheckNameCount(SerializedProperty property, CustomVectorAttribute attribute)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Vector2:
            case SerializedPropertyType.Vector2Int:

                return attribute.Names.Length == 2;

            case SerializedPropertyType.Vector3:
            case SerializedPropertyType.Vector3Int:

                return attribute.Names.Length == 3;

            case SerializedPropertyType.Vector4:

                return attribute.Names.Length == 4;

            default:

                return false;
        }
    }
    #endregion

    #region Events
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var vectorAttribute = (CustomVectorAttribute)attribute;

        if (!this.CheckNameCount(property, vectorAttribute))
        {
            Debug.LogError($"Missing names for CustomVector attribute. Ensured to match the names array elements with the right vector type. \nMember target: \"{label.text}\"");
            return;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        {
            var contentPosition = EditorGUI.PrefixLabel(position, label);

            contentPosition.xMin--;

            if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector2Int)
            {
                contentPosition.width /= 1.5f;
            }

            EditorGUI.MultiPropertyField(contentPosition, vectorAttribute.Names, property.FindPropertyRelative("x"));
        }
        EditorGUI.EndProperty();
    } 
    #endregion
}
