using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            //SortingLayer sl = "";
            

            string[] layerNames = this.GetSortingLayerNames();
            int layerCount = layerNames.Length;
            ArrayUtility.AddRange(ref layerNames, new string[] { string.Empty, "Add Sorting Layer..." });

            property.intValue = EditorGUI.Popup(position, label.text, property.intValue, layerNames);

            if (property.intValue >= layerCount)
            {
                this.OpenTagManager();
            }

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

        void OpenTagManager()
        {
            //TagManagerInspector.ShowWithInitialExpansion(TagManagerInspector.InitialExpansionState.SortingLayers);

            // TODO: Make static members for one-time initialization.
            Assembly unityEditor = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.GetName().Name == "UnityEditor").FirstOrDefault();
            Type tagManagerInspector = unityEditor.GetType("UnityEditor.TagManagerInspector");
            MethodInfo mi = tagManagerInspector.GetMethod("ShowWithInitialExpansion", BindingFlags.NonPublic | BindingFlags.Static);
            object enumArgument = Enum.GetValues(tagManagerInspector.GetMember("InitialExpansionState").GetType()).GetValue(3); // TODO: Fix the cast to enum reflexion (the get member call works finding the enum member).

            mi.Invoke(enumArgument, null);
        }
        #endregion
    }
}