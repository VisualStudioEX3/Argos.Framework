using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Argos.Framework.IMGUI;
using System.Reflection;

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

            this._dataTable = new DataTable(this._prop, columns.ToArray(), -1, true, "Test4");
            {
                this._dataTable.ShowRowIndexColumn = true;
                this._dataTable.ShowSearchField = true;

                this._dataTable.OnRowClick += (rowIndex, data) =>
                {
                    Debug.Log($"User click on row index: {rowIndex}");
                };

                this._dataTable.OnRowSelected += (rowIndex, data) =>
                {
                    Debug.Log($"Selected row index: {rowIndex}");
                };

                this._dataTable.OnMultipleRowsSelected += (rowIndexes, data) =>
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var item in rowIndexes)
                    {
                        sb.AppendFormat("{0}, ", item);
                    }
                    Debug.Log($"Selected multiple row indexes: {sb.ToString()}");
                };
            }

            //this.editorSkin = Editor.CreateEditor(Argos.Framework.Utils.EditorSkinUtility.Skin);
        }

        private void OnDisable()
        {
            this._dataTable.SaveState();
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

            SerializedProperty gradient = this.serializedObject.FindProperty("_gradient");

            PropertyInfo gradientValue = gradient.GetType().GetProperty("gradientValue", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

            var source = gradientValue.GetValue(gradient) as Gradient;

            var copy = new Gradient();
            copy.alphaKeys = source.alphaKeys;
            copy.colorKeys = source.colorKeys;
            copy.mode = source.mode;

            EditorGUI.GradientField(EditorGUILayout.GetControlRect(), source);

            SerializedProperty mask = this.serializedObject.FindProperty("_layerMask");
            SerializedProperty letter = this.serializedObject.FindProperty("_letter");
            SerializedProperty longValue = this.serializedObject.FindProperty("_longValue");

            //this.editorSkin.DrawDefaultInspector();
        }
    }
}