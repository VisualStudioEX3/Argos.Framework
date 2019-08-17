using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Argos.Framework.IMGUI;

namespace Argos.Framework.Localization
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        SerializedProperty _prop;
        DataTable _dataTable;

        Editor editorSkin;

        private void OnEnable()
        {
            SerializedProperty gradient = this.serializedObject.FindProperty("_gradient");
            SerializedProperty mask = this.serializedObject.FindProperty("_layerMask");
            SerializedProperty letter = this.serializedObject.FindProperty("_letter");

            this._prop = this.serializedObject.FindProperty("_test");
            var columns = new List<DataTable.DataTableColumn>();
            {
                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Key",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 200f,
                    minWidth = 100f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 100f,
                    propertyName = "key"
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Text",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 150f,
                    minWidth = 100f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 125f,
                    propertyName = "text"
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Key Code",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 150f,
                    minWidth = 100f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 125f,
                    propertyName = "keyCode",
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Enable",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 150f,
                    minWidth = 100f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 125f,
                    propertyName = "enable",
                    readOnly = true
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Value",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 500f,
                    minWidth = 300f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 400f,
                    propertyName = "value",
                    readOnly = true
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Color",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 250f,
                    minWidth = 100f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 175f,
                    propertyName = "color",
                    readOnly = true
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Curve",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 250f,
                    minWidth = 100f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 175f,
                    propertyName = "curve",
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    canSort = true,
                    headerTitle = "Threshold",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 100f,
                    minWidth = 60f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 75f,
                    propertyName = "threshold",
                    readOnly = true
                });
            }

            this._dataTable = new DataTable(this._prop, columns.ToArray());
            this._dataTable.ShowRowIndexColumn = true;
            this._dataTable.ShowSearchField = true;

            //this.editorSkin = Editor.CreateEditor(Argos.Framework.Utils.EditorSkinUtility.Skin);
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            this.DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Argos.Framework DataTable control test", EditorStyles.boldLabel);

            this._dataTable.DoLayout();
            {
                this._dataTable.ResizeToFitColumns = EditorGUILayout.Toggle("Resize To Fit Columns", this._dataTable.ResizeToFitColumns);
                this._dataTable.ShowRowIndexColumn = EditorGUILayout.Toggle("Show row index", this._dataTable.ShowRowIndexColumn);
                this._dataTable.ShowSearchField = EditorGUILayout.Toggle("Search", this._dataTable.ShowSearchField);
                this._dataTable.CanDrag = EditorGUILayout.Toggle("Drag & Drop", this._dataTable.CanDrag);
                this._dataTable.CanMultiselect = EditorGUILayout.Toggle("Multiselection", this._dataTable.CanMultiselect);
            }

            this.serializedObject.ApplyModifiedProperties();

            //this.editorSkin.DrawDefaultInspector();
        }

        string GetValue(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.Integer: return property.intValue.ToString();

                case SerializedPropertyType.Boolean: return property.boolValue.ToString();

                case SerializedPropertyType.Float: return property.boolValue.ToString();

                case SerializedPropertyType.String: return property.stringValue;

                case SerializedPropertyType.Gradient:
                case SerializedPropertyType.Color: return property.colorValue.ToString();

                case SerializedPropertyType.ObjectReference: return $"Object Reference: {property.objectReferenceValue}";

                case SerializedPropertyType.ExposedReference: return $"Exposed reference value: {property.exposedReferenceValue}";

                case SerializedPropertyType.Enum: return property.enumDisplayNames[property.enumValueIndex];

                case SerializedPropertyType.Vector2: return property.vector2Value.ToString();

                case SerializedPropertyType.Vector3: return property.vector3Value.ToString();

                case SerializedPropertyType.Vector4: return property.vector4Value.ToString();

                case SerializedPropertyType.Rect: return property.rectValue.ToString();

                case SerializedPropertyType.ArraySize: return $"Array size: {property.arraySize}";

                case SerializedPropertyType.Character: return property.stringValue;

                case SerializedPropertyType.AnimationCurve: return property.animationCurveValue.ToString();

                case SerializedPropertyType.Bounds: return property.boundsValue.ToString();

                case SerializedPropertyType.Quaternion: return property.quaternionValue.ToString();

                case SerializedPropertyType.FixedBufferSize: return $"Fixed buffer size: {property.fixedBufferSize}";

                case SerializedPropertyType.Vector2Int: return property.vector2IntValue.ToString();

                case SerializedPropertyType.Vector3Int: return property.vector3IntValue.ToString();

                case SerializedPropertyType.RectInt: return property.rectIntValue.ToString();

                case SerializedPropertyType.BoundsInt: return property.boundsIntValue.ToString();

                case SerializedPropertyType.Generic:
                default:

                    return string.Empty;
            }
        }
    }
}