using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(SortingLayerAttribute))]
    public class SortingLayerDrawer : ArgosPropertyDrawerBase
    {
        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Integer;
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int id = property.intValue;
            if (!SortingLayer.IsValid(id))
            {
                id = 0;
            }

            //int index = 

            //EditorGUI.LayerField()

            property.intValue = EditorGUI.Popup(position, label.text, property.intValue, this.GetSortingLayerNames());

            // TODO: Maybe is better idea works with Sorting Layer name instead of id or value.
            // TODO: Add support to SortingLayer struct type.
        }

        string[] GetSortingLayerNames()
        {
            var names = new string[SortingLayer.layers.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = SortingLayer.layers[i].name;
            }

            return names;
        }
        #endregion
    }
}