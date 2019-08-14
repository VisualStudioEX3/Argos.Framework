using System;
using System.Collections;
using System.Collections.Generic;
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
            this._prop = this.serializedObject.FindProperty("_test");
            var columns = new DataTable.DataTableColumn[5];
            {
                columns[0].autoResize = true;
                columns[0].canSort = true;
                columns[0].headerTitle = "Key";
                columns[0].headerTextAlignment = TextAlignment.Left;
                columns[0].maxWidth = 200f;
                columns[0].minWidth = 100f;
                columns[0].sortingArrowAlignment = TextAlignment.Center;
                columns[0].sortedAscending = false;
                columns[0].width = 100f;
                columns[0].propertyName = "key";

                columns[1].autoResize = true;
                columns[1].canSort = true;
                columns[1].headerTitle = "Text";
                columns[1].headerTextAlignment = TextAlignment.Left;
                columns[1].maxWidth = 150f;
                columns[1].minWidth = 100f;
                columns[1].sortingArrowAlignment = TextAlignment.Center;
                columns[1].sortedAscending = false;
                columns[1].width = 125f;
                columns[1].propertyName = "text";

                columns[2].autoResize = true;
                columns[2].canSort = true;
                columns[2].headerTitle = "Key Code";
                columns[2].headerTextAlignment = TextAlignment.Left;
                columns[2].maxWidth = 150f;
                columns[2].minWidth = 100f;
                columns[2].sortingArrowAlignment = TextAlignment.Center;
                columns[2].sortedAscending = false;
                columns[2].width = 125f;
                columns[2].propertyName = "keyCode";

                columns[3].autoResize = true;
                columns[3].canSort = true;
                columns[3].headerTitle = "Value";
                columns[3].headerTextAlignment = TextAlignment.Left;
                columns[3].maxWidth = 500f;
                columns[3].minWidth = 100f;
                columns[3].sortingArrowAlignment = TextAlignment.Center;
                columns[3].sortedAscending = false;
                columns[3].width = 250f;
                columns[3].propertyName = "value";

                columns[4].autoResize = true;
                columns[4].canSort = true;
                columns[4].headerTitle = "Threshold";
                columns[4].headerTextAlignment = TextAlignment.Left;
                columns[4].maxWidth = 100f;
                columns[4].minWidth = 60f;
                columns[4].sortingArrowAlignment = TextAlignment.Center;
                columns[4].sortedAscending = false;
                columns[4].width = 75f;
                columns[4].propertyName = "threshold";
            }

            this._dataTable = new DataTable(this._prop, columns);
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

            this._dataTable.ShowRowIndexColumn = EditorGUILayout.Toggle("Show row index", this._dataTable.ShowRowIndexColumn);
            this._dataTable.ShowSearchField = EditorGUILayout.Toggle("Search", this._dataTable.ShowSearchField);
            this._dataTable.CanDrag = EditorGUILayout.Toggle("Drag & Drop", this._dataTable.CanDrag);
            this._dataTable.CanMultiselect = EditorGUILayout.Toggle("Multiselection", this._dataTable.CanMultiselect);

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