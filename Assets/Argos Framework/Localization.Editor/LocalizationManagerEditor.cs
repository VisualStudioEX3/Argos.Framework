using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;

namespace Argos.Framework.Localization
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        SearchField _searchField;
        TreeView _table;
        TreeViewState _tableState;
        MultiColumnHeader _tableHeader;
        MultiColumnHeaderState _tableHeaderState;
        MultiColumnHeaderState.Column[] _tableHeaderColumns;

        private void OnEnable()
        {
            this._searchField = new SearchField();

            this._tableState = new TreeViewState();
            this._tableHeaderColumns = new MultiColumnHeaderState.Column[3];

            this._tableHeaderColumns[0] = new MultiColumnHeaderState.Column();
            this._tableHeaderColumns[0].autoResize = true;
            this._tableHeaderColumns[0].canSort = true;
            this._tableHeaderColumns[0].headerContent = new GUIContent("Row");
            this._tableHeaderColumns[0].headerTextAlignment = TextAlignment.Left;
            this._tableHeaderColumns[0].maxWidth = 40f;
            this._tableHeaderColumns[0].minWidth = 40f;
            this._tableHeaderColumns[0].sortingArrowAlignment = TextAlignment.Right;
            this._tableHeaderColumns[0].sortedAscending = true;
            this._tableHeaderColumns[0].width = 40f;

            this._tableHeaderColumns[1] = new MultiColumnHeaderState.Column();
            this._tableHeaderColumns[1].autoResize = true;
            this._tableHeaderColumns[1].canSort = true;
            this._tableHeaderColumns[1].headerContent = new GUIContent("Id");
            this._tableHeaderColumns[1].headerTextAlignment = TextAlignment.Left;
            this._tableHeaderColumns[1].maxWidth = 200f;
            this._tableHeaderColumns[1].minWidth = 100f;
            this._tableHeaderColumns[1].sortingArrowAlignment = TextAlignment.Right;
            this._tableHeaderColumns[1].sortedAscending = false;
            this._tableHeaderColumns[1].width = 100f;

            this._tableHeaderColumns[2] = new MultiColumnHeaderState.Column();
            this._tableHeaderColumns[2].autoResize = true;
            this._tableHeaderColumns[2].canSort = true;
            this._tableHeaderColumns[2].headerContent = new GUIContent("Text");
            this._tableHeaderColumns[2].headerTextAlignment = TextAlignment.Left;
            this._tableHeaderColumns[2].maxWidth = 500f;
            this._tableHeaderColumns[2].minWidth = 100f;
            this._tableHeaderColumns[2].sortingArrowAlignment = TextAlignment.Right;
            this._tableHeaderColumns[2].sortedAscending = false;
            this._tableHeaderColumns[2].width = 250f;

            this._tableHeaderState = new MultiColumnHeaderState(this._tableHeaderColumns);

            this._tableHeader = new MultiColumnHeader(this._tableHeaderState);
            this._tableHeader.SetSortingColumns(new int[] { 0, 1, 2 }, new bool[] { true, true, true });
            //this._tableHeader.sortingChanged
            this._tableHeader.

            this._table = new TableTest(this._tableState, this._tableHeader);
        }

        public override void OnInspectorGUI()
        {
            Rect searchFieldRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            Rect fieldRect = EditorGUI.PrefixLabel(searchFieldRect, new GUIContent("Label"));

            this._table.searchString = this._searchField.OnGUI(fieldRect, this._table.searchString);
            this._table.OnGUI(EditorGUILayout.GetControlRect(false, (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 10f));
        }
    }

    public class TableTest : TreeView
    {
        public TableTest(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
            this.Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            this.rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            this.showAlternatingRowBackgrounds = true;
            this.showBorder = true;

            // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
            // are created from data. Here we create a fixed set of items. In a real world example,
            // a data model should be passed into the TreeView and the items created from the model.

            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

            var allItems = new List<LocalizationItem>
            {
                new LocalizationItem ("ui_submit", "Submit"),
                new LocalizationItem ("ui_cancel", "Cancel"),
                new LocalizationItem ("ui_delete", "Delete"),
                new LocalizationItem ("ui_defaults", "Set Defaults"),
                new LocalizationItem ("ui_ok", "Ok"),
                new LocalizationItem ("ui_navigation", "Navigation"),
            };

            foreach (var item in allItems)
            {
                root.AddChild(item);
            }

            return root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            return base.BuildRows(root);
        }

        

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Rect rect = args.rowRect;
            this.CenterRectUsingSingleLineHeight(ref rect);

            var data = args.item as LocalizationItem;

            for (int i = 0; i < args.GetNumVisibleColumns(); i++)
            {
                this.columnIndexForTreeFoldouts = i;
                Rect cellRect = this.GetCellRectForTreeFoldouts(rect);
                cellRect.xMin += 0.5f;
                cellRect.xMax -= 0.5f;

                switch (i)
                {
                    case 0:

                        EditorGUI.LabelField(cellRect, args.row.ToString());
                        break;

                    case 1:

                        data.id = EditorGUI.DelayedTextField(cellRect, data.id);
                        break;

                    case 2:

                        data.text = EditorGUI.DelayedTextField(cellRect, data.text);
                        break;
                }
            }
        }
    }

    [Serializable]
    internal class LocalizationItem : TreeViewItem
    {
        static int _index = 0;

        public new string id;
        public string text;

        public LocalizationItem(string id, string text) : base(++LocalizationItem._index, 0, id)
        {
            this.id = id;
            this.text = text;
        }
    }
}