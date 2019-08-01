using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Argos.Framework.IMGUI
{
    public abstract class DataTable<T> where T : DataTable<T>.DataTableItem<T>
    {
        #region Classes
        public abstract class DataTableItem<TItem> : TreeViewItem
        {
            #region Public vars
            public TItem data;
            #endregion

            #region Static members
            static int _index = 0;
            #endregion

            #region Constructors
            public DataTableItem() : this(default)
            {
            }

            public DataTableItem(TItem data) : base(++DataTableItem<TItem>._index, 0, string.Empty)
            {
                this.data = data;
            }

            ~DataTableItem()
            {
                --DataTableItem<TItem>._index;
            }
            #endregion
        }

        class TreeViewImplementation : TreeView
        {
            #region Internal vars
            int _columnCount;
            #endregion

            #region Properties
            public TreeView TreeViewInstance { get; private set; }
            public bool ShowRowIndex { get; set; }
            #endregion

            #region Events
            public event Action OnDeserialize;
            public event Action<Rect, int, int, T> OnCellGUI;
            #endregion

            #region Constructors
            public TreeViewImplementation(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
            {
                this.rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                this.showAlternatingRowBackgrounds = this.showBorder = true;

                this.Reload();
            }
            #endregion

            #region Event listeners
            protected override TreeViewItem BuildRoot()
            {
                // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
                // are created from data. Here we create a fixed set of items. In a real world example,
                // a data model should be passed into the TreeView and the items created from the model.

                var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

                this.OnDeserialize?.Invoke();

                return root;
            }

            protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
            {
                return base.BuildRows(root);
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                Rect rect = args.rowRect;
                this.CenterRectUsingSingleLineHeight(ref rect);

                T rowData = (T)args.item;

                for (int i = 0; i < this._columnCount; i++)
                {
                    this.columnIndexForTreeFoldouts = i;

                    Rect cellRect = this.GetCellRectForTreeFoldouts(rect);
                    cellRect.xMin += 1.5f;
                    cellRect.xMax -= 1.5f;

                    if (i == 0 && this.ShowRowIndex)
                    {
                        EditorGUI.LabelField(cellRect, args.row.ToString());
                    }
                    else
                    {
                        this.OnCellGUI(cellRect, args.row, i, rowData);
                    }
                }
            }
            #endregion
        }
        #endregion

        #region Internal vars
        SearchField _searchField;

        TreeViewImplementation _table;

        TreeViewState _tableState;
        MultiColumnHeader _tableHeader;
        MultiColumnHeaderState _tableHeaderState;
        MultiColumnHeaderState.Column[] _tableHeaderColumns;
        #endregion

        public DataTable()
        {
            this._searchField = new SearchField();

            this._tableState = new TreeViewState();

            // TODO: Allow to define columns outside:
            this._tableHeaderColumns = new MultiColumnHeaderState.Column[3];
            {
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
                this._tableHeaderColumns[2].maxWidth = 500f;
                this._tableHeaderColumns[2].minWidth = 100f;
                this._tableHeaderColumns[2].sortingArrowAlignment = TextAlignment.Right;
                this._tableHeaderColumns[2].sortedAscending = false;
                this._tableHeaderColumns[2].width = 250f;
            }

            this._tableHeaderState = new MultiColumnHeaderState(this._tableHeaderColumns);
            this._tableHeader = new MultiColumnHeader(this._tableHeaderState);

            this._table = new TreeViewImplementation(this._tableState, this._tableHeader);
            {
                this._table.OnDeserialize += this.OnDeserialize;
                this._table.OnCellGUI += this.OnCellGUI;
            }
        }

        ~DataTable()
        {
            this._table.OnDeserialize -= this.OnDeserialize;
            this._table.OnCellGUI -= this.OnCellGUI;
        }

        #region Methods & Functions
        public void Do(Rect layout)
        {

        }

        public void DoLayout()
        {

        }
        #endregion

        #region Event listeners
        public abstract void OnDeserialize();

        public abstract void OnCellGUI(Rect cellRect, int rowIndex, int cellIndex, T rowData);
        #endregion
    }
}