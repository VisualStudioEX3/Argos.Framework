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
                    //canSort = true,
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
                    //canSort = true,
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
                    //canSort = true,
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
                    //canSort = true,
                    headerTitle = "Enable",
                    headerTextAlignment = TextAlignment.Left,
                    maxWidth = 47.5f,
                    minWidth = 47.5f,
                    sortingArrowAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 47.5f,
                    propertyName = "enable",
                    readOnly = true
                });

                columns.Add(new DataTable.DataTableColumn()
                {
                    autoResize = true,
                    //canSort = true,
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
                    //canSort = true,
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
                    //canSort = true,
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
                    //canSort = true,
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

            this._dataTable = new DataTable(this._prop, columns.ToArray(), -1, true, "DataTableTest");
            {
                this._dataTable.ShowRowIndexColumn = true;
                this._dataTable.ShowSearchField = true;
                this._dataTable.ShowFooter = true;
                this._dataTable.CanDrag = true;
                this._dataTable.CanMultiselect = true;

                this._dataTable.OnToobarSearchGUI += (rect) =>
                {
                    //EditorGUI.DrawRect(rect, Color.red);
                    //EditorGUI.LabelField(rect, "Test");
                    //rect.xMin += 4f;
                    if (GUI.Button(rect, "Delete state & reload"))
                    {
                        this._dataTable.DeleteState();
                        this._dataTable.Reload();
                    }
                };

                this._dataTable.OnFooterGUI += (rect) =>
                {
                    //EditorGUI.DrawRect(rect, Color.red);
                    //GUI.Button(rect, "Test");
                    EditorGUI.LabelField(rect, new GUIContent($"Total rows: {this._dataTable.RowCount} | Selected: {this._dataTable.Selection.Length} | Search result: {this._dataTable.SearchResult.Length}"));//, EditorStyles.miniLabel);
                };

                this._dataTable.OnRowClick += (rowItem) =>
                {
                    Debug.Log($"User click on row index: {rowItem.Row}");
                };

                this._dataTable.OnRowDoubleClick += (rowItem) =>
                {
                    Debug.Log($"User double-click on row index: {rowItem.Row}");
                };

                this._dataTable.OnRowSelected += (rowItem) =>
                {
                    Debug.Log($"Selected row index: {rowItem.Row}");
                };

                this._dataTable.OnMultipleRowsSelected += (rowItems) =>
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var item in rowItems)
                    {
                        sb.AppendFormat("{0}, ", item.Row);
                    }
                    Debug.Log($"Selected multiple row indexes: {sb.ToString()}");
                };
            }

            this.editorSkin = Editor.CreateEditor(Argos.Framework.Utils.EditorSkinUtility.Skin);

            (this.target as LocalizationManager).texture = Utils.EditorSkinUtility.Styles.Custom.ToolbarSearch.cancelButtonEmpty.normal.scaledBackgrounds[0];
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
                this._dataTable.ShowRowIndexColumn = EditorGUILayout.Toggle("Show Row Index", this._dataTable.ShowRowIndexColumn);
                this._dataTable.ShowSearchField = EditorGUILayout.Toggle("Search", this._dataTable.ShowSearchField);
                this._dataTable.ShowFooter = EditorGUILayout.Toggle("Show Footer", this._dataTable.ShowFooter);                
                this._dataTable.CanDrag = EditorGUILayout.Toggle("Drag & Drop", this._dataTable.CanDrag);
                this._dataTable.CanMultiselect = EditorGUILayout.Toggle("Multiselection", this._dataTable.CanMultiselect);                
            }

            this.serializedObject.ApplyModifiedProperties();

            this.editorSkin.DrawDefaultInspector();
        }
    }
}