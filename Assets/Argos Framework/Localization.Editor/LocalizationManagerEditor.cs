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
        TreeView _table;
        TreeViewState _tableState;
        MultiColumnHeader _tableHeader;
        MultiColumnHeaderState _tableHeaderState;
        MultiColumnHeaderState.Column[] _tableHeaderColumns;

        private void OnEnable()
        {
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
            this._tableHeaderColumns[0].sortedAscending = false;
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
            this._tableHeaderColumns[2].maxWidth = 1000f;
            this._tableHeaderColumns[2].minWidth = 100f;
            this._tableHeaderColumns[2].sortingArrowAlignment = TextAlignment.Right;
            this._tableHeaderColumns[2].sortedAscending = false;
            this._tableHeaderColumns[2].width = 500f;

            this._tableHeaderState = new MultiColumnHeaderState(this._tableHeaderColumns);
            this._tableHeader = new MultiColumnHeader(this._tableHeaderState);
            this._table = new TableTest(this._tableState, this._tableHeader);
        }

        public override void OnInspectorGUI()
        {
            this._table.OnGUI(EditorGUILayout.GetControlRect(false, 300f));
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
            this.rowHeight = 20f;
            this.showAlternatingRowBackgrounds = true;
            this.showBorder = true;

            // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
            // are created from data. Here we create a fixed set of items. In a real world example,
            // a data model should be passed into the TreeView and the items created from the model.

            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var allItems = new List<TreeViewItem>
            {
                new TreeViewItem {id = 1, depth = 0, displayName = "Animals"},
                new TreeViewItem {id = 2, depth = 0, displayName = "Mammals"},
                new TreeViewItem {id = 3, depth = 0, displayName = "Tiger"},
                new TreeViewItem {id = 4, depth = 0, displayName = "Elephant"},
                new TreeViewItem {id = 5, depth = 0, displayName = "Okapi"},
                new TreeViewItem {id = 6, depth = 0, displayName = "Armadillo"},
                new TreeViewItem {id = 7, depth = 0, displayName = "Reptiles"},
                new TreeViewItem {id = 8, depth = 0, displayName = "Crocodile"},
                new TreeViewItem {id = 9, depth = 0, displayName = "Lizard"},
            };

            foreach (var item in allItems)
            {
                root.AddChild(item);
            }

            return root;
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);
        }
    }

    [Serializable]
    internal class LocalizationItem : TreeViewItem
    {
        public long Row;
        public string Id;
        public string text = "";

        public LocalizationItem(int id, int depth, string name) : base(id, depth, name)
        {
            
        }
    }
}