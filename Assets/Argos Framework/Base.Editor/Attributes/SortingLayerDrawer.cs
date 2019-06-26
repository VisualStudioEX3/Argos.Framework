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
        #region Constants
        const string ADD_SORTING_LAYER_LABEL = "Add Sorting Layer...";
        static readonly string[] ADD_SORTING_LAYER_POPUP_ITEM = new string[] { string.Empty, SortingLayerDrawer.ADD_SORTING_LAYER_LABEL };
        #endregion

        #region Static members
        static MethodInfo _methodInfo;
        static object[] _args;
        #endregion

        #region Initializers
        [InitializeOnLoadMethod]
        static void GetReflectionMethod()
        {
            Assembly unityEditor = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.GetName().Name == "UnityEditor").FirstOrDefault();

            Type tagManagerInspector = unityEditor.GetType("UnityEditor.TagManagerInspector");
            SortingLayerDrawer._methodInfo = tagManagerInspector.GetMethod("ShowWithInitialExpansion", BindingFlags.NonPublic | BindingFlags.Static);

            Type enumType = unityEditor.GetType("UnityEditor.TagManagerInspector+InitialExpansionState");
            SortingLayerDrawer._args = new object[1] { Enum.Parse(enumType, "SortingLayers") };
        } 
        #endregion

        #region Methods & Functions
        public override bool CheckPropertyType(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.String;
        }

        string GetValidatedSortingLayerName(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.Integer ? this.ValidateSortingLayer(property.intValue) : this.ValidateSortingLayer(property.stringValue);
        }

        void SetSelectedSortingLayer(SerializedProperty property, string layerName)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:

                    property.intValue = SortingLayer.NameToID(layerName);
                    break;

                case SerializedPropertyType.String:

                    property.stringValue = layerName;
                    break;
            }
        }

        string ValidateSortingLayer(int id)
        {
            return SortingLayer.IDToName(!SortingLayer.IsValid(id) ? 0 : id);
        }

        string ValidateSortingLayer(string name)
        {
            return this.ValidateSortingLayer(SortingLayer.NameToID(name));
        }

        string[] GetLayerNames()
        {
            var names = new string[SortingLayer.layers.Length];

            for (int i = 0; i < SortingLayer.layers.Length; i++)
            {
                names[i] = SortingLayer.layers[i].name;
            }

            return names;
        }

        bool DoPopup(Rect position, GUIContent label, string[] layerNames, ref int selection)
        {
            int prevSelection = selection;

            // Added separator and item button to invoke Unity Tag Manager:
            ArrayUtility.AddRange(ref layerNames, SortingLayerDrawer.ADD_SORTING_LAYER_POPUP_ITEM);
            selection = EditorGUI.Popup(position, label.text, selection, layerNames);

            if (selection > SortingLayer.layers.Length)
            {
                // Internal call to show Tag Manager window popup with the Sorting Layer list open:
                // TagManagerInspector.ShowWithInitialExpansion(TagManagerInspector.InitialExpansionState.SortingLayers);
                SortingLayerDrawer._methodInfo.Invoke(null, SortingLayerDrawer._args);

                selection = prevSelection;
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Event listeners
        public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] layerNames = this.GetLayerNames();
            int index = ArrayUtility.IndexOf(layerNames, this.GetValidatedSortingLayerName(property));

            if (this.DoPopup(position, label, layerNames, ref index))
            {
                this.SetSelectedSortingLayer(property, layerNames[index]);
            }
        }
        #endregion
    }
}