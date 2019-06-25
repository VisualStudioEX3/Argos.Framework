using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(SortingLayerAttribute)), CustomPropertyDrawer(typeof(SortingLayer))]
    public class SortingLayerDrawer : ArgosPropertyDrawerBase
    {
        #region Constants
        const string ADD_SORTING_LAYER_LABEL = "Add Sorting Layer...";
        static readonly string[] ADD_SORTING_LABEL_OPTION_ARRAY = new string[] { string.Empty, SortingLayerDrawer.ADD_SORTING_LAYER_LABEL };
        #endregion

        #region Static members
        static Assembly _unityEditor;
        static Type _tagManagerInspector;
        static MethodInfo _showWithInitialExpansion;
        #endregion

        #region Initializers
        [InitializeOnLoadMethod]
        static void GetReflectionMethod()
        {
            const string UNITY_EDITOR_ASSEMBLY_NAME = "UnityEditor";
            const string TAG_MANAGER_INSPECTOR_CLASS_NAME = "TagManagerInspector";
            const string SHOW_TAG_MANAGER_METHOD_NAME = "ShowWithInitialExpansion";

            SortingLayerDrawer._unityEditor = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.GetName().Name == UNITY_EDITOR_ASSEMBLY_NAME).FirstOrDefault();
            SortingLayerDrawer._tagManagerInspector = SortingLayerDrawer._unityEditor.GetType($"{UNITY_EDITOR_ASSEMBLY_NAME}.{TAG_MANAGER_INSPECTOR_CLASS_NAME}");
            SortingLayerDrawer._showWithInitialExpansion = SortingLayerDrawer._tagManagerInspector.GetMethod(SHOW_TAG_MANAGER_METHOD_NAME, BindingFlags.NonPublic | BindingFlags.Static);
        } 
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.String || this.FieldType == typeof(SortingLayer);
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int id;
            
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:

                    id = this.ValidateSortingLayer(property.intValue);

                    break;

                case SerializedPropertyType.String:

                    id = this.ValidateSortingLayer(property.stringValue);
                    break;

                default: // Type is SortingLayer:

                    //
                    break;
            }

            //property.intValue = EditorGUI.Popup(position, label.text, property.intValue, popupItems);

            // TODO: Maybe is better idea works with Sorting Layer name instead of id or value.
        }

        int ValidateSortingLayer(int id)
        {
            return !SortingLayer.IsValid(id) ? 0 : id;
        }

        int ValidateSortingLayer(string name)
        {
            return this.ValidateSortingLayer(SortingLayer.NameToID(name));
        }

        string[] CreatePopupItems(out int layerCount)
        {
            layerCount = SortingLayer.layers.Length;

            var names = new string[layerCount + 2];

            for (int i = 0; i < layerCount; i++)
            {
                names[i] = SortingLayer.layers[i].name;
            }

            // Add separator and option to open Unity tag/layer manager window panel.
            names[layerCount + 1] = string.Empty;
            names[layerCount + 2] = SortingLayerDrawer.ADD_SORTING_LAYER_LABEL;

            return names;
        }

        bool DoPopup(Rect position, GUIContent label, ref int selection)
        {
            int layerCount;
            string[] popupItems = this.CreatePopupItems(out layerCount);
            int prevSelection = selection;

            selection = EditorGUI.Popup(position, label.text, selection, popupItems);

            if (selection > layerCount)
            {
                this.OpenTagManager();
                selection = prevSelection;
                return false;
            }
            else
            {
                return true;
            }
        }

        void OpenTagManager()
        {
            //TagManagerInspector.ShowWithInitialExpansion(TagManagerInspector.InitialExpansionState.SortingLayers);

            object enumArgument = Enum.GetValues(SortingLayerDrawer._tagManagerInspector.GetMember("InitialExpansionState").GetType()).GetValue(3); // TODO: Fix the cast to enum reflexion (the get member call works finding the enum member).

            SortingLayerDrawer._showWithInitialExpansion.Invoke(enumArgument, null);
        }
        #endregion
    }
}