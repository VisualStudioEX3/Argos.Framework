using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Argos.Framework.IMGUI
{
    public sealed class DataTable
    {
        #region Constants
        const float ROW_COLUMN_CHAR_WIDTH = 10f;
        const float ROW_COLUMN_MIN_WIDTH = 30f;
        const float ROW_COLUMN_MAX_WIDTH = DataTable.ROW_COLUMN_CHAR_WIDTH * 5;
        #endregion

        #region Classes
        public struct DataTableColumn
        {
            #region Public vars
            public bool autoResize;
            public bool canSort;

            public string headerTitle;
            public TextAlignment headerTextAlignment;

            public TextAlignment sortingArrowAlignment;
            public bool sortedAscending;

            public float width;
            public float minWidth;
            public float maxWidth;

            public string propertyName;
            #endregion

            #region Operators
            public static implicit operator MultiColumnHeaderState.Column(DataTableColumn column)
            {
                return new MultiColumnHeaderState.Column()
                {
                    autoResize = column.autoResize,
                    canSort = column.canSort,
                    headerContent = new GUIContent(column.headerTitle),
                    headerTextAlignment = column.headerTextAlignment,
                    sortingArrowAlignment = column.sortingArrowAlignment,
                    sortedAscending = column.sortedAscending,
                    width = column.width,
                    minWidth = column.minWidth,
                    maxWidth = column.maxWidth
                };
            }
            #endregion
        }

        // TOOD: Found a way to sorting using standard types from SerializedProperty. The sorting behaviour is external to Unity Treeview, then we can implement own sorting code based on any data of this class.
        // TODO: The SerializedProperty enable the possibility to draw custom drawer easily in each Treeview cell using EditorGUI.PropertyField.
        class InternalTreeViewItem : TreeViewItem
        {
            #region Public vars
            public SerializedProperty[] data;
            #endregion

            #region Static members
            static int _index = 0;
            #endregion

            #region Constructors
            public InternalTreeViewItem(InternalTreeView treeView, SerializedProperty property) : base(++InternalTreeViewItem._index, 0, string.Empty)
            {
                this.data = new SerializedProperty[treeView.propertyNames.Length];

                for (int i = 0; i < treeView.propertyNames.Length; i++)
                {
                    if (!string.IsNullOrEmpty(treeView.propertyNames[i]))
                    {
                        this.data[i] = property.FindPropertyRelative(treeView.propertyNames[i]);
                    }
                }

                treeView.OnSearchColumnIndexChange += this.OnSetSearchColumn;
            }

            ~InternalTreeViewItem()
            {
                InternalTreeViewItem._index--;
            }
            #endregion

            #region Event listeners
            void OnSetSearchColumn(InternalTreeView treeView, int index)
            {
                switch (this.data[index].propertyType)
                {
                    case SerializedPropertyType.Integer:

                        this.displayName = this.data[index].intValue.ToString();
                        break;

                    case SerializedPropertyType.Float:

                        this.displayName = this.data[index].floatValue.ToString();
                        break;

                    case SerializedPropertyType.String:

                        this.displayName = this.data[index].stringValue;
                        break;

                    case SerializedPropertyType.Enum:

                        this.displayName = this.data[index].enumDisplayNames[this.data[index].enumValueIndex];
                        break;

                    default:

                        this.displayName = string.Empty;
                        break;
                }
            } 
            #endregion
        }

        class InternalTreeView : TreeView
        {
            #region Constants
            const string GENERIC_DRAG_ID = "GenericDragColumnDragging";
            #endregion

            #region Public vars
            public SerializedProperty property;
            public string[] propertyNames;

            public bool showRowIndex;

            public int searchColumnIndex;
            public SerializedPropertyType searchColumnType;

            public bool canDrag;
            public bool canMultiselect;
            #endregion

            #region Events
            public event Action<InternalTreeView, int> OnSearchColumnIndexChange;
            #endregion

            #region Constructors
            public InternalTreeView(MultiColumnHeaderState.Column[] columStates, SerializedProperty property, string[] propertyNames) : base(new TreeViewState(), null)
            {
                this.multiColumnHeader = new MultiColumnHeader(new MultiColumnHeaderState(columStates));
                this.multiColumnHeader.sortingChanged += this.OnSortingChanged;

                this.rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                this.showAlternatingRowBackgrounds = this.showBorder = true;

                this.property = property;
                this.propertyNames = propertyNames;

                this.Reload();
            }
            #endregion

            #region Methods & Functions
            public void SetSearchColumnIndex(int index)
            {
                this.OnSearchColumnIndexChange?.Invoke(this, index);
                this.Repaint();
            }

            int GetItemIndex(TreeViewItem item)
            {
                return this.GetItemIndex(item.id);
            }

            int GetItemIndex(int id)
            {
                IList<TreeViewItem> rows = this.GetRows();
                for (int i = 0; i < rows.Count; i++)
                {
                    if (rows[i].id == id)
                    {
                        return i;
                    }
                }

                return -1;
            }

            // TODO: Revise this
            bool ValidDrag(TreeViewItem parent, List<TreeViewItem> draggedItems)
            {
                TreeViewItem currentParent = parent;
                while (currentParent != null)
                {
                    if (draggedItems.Contains(currentParent))
                    {
                        return false;
                    }
                    currentParent = currentParent.parent;
                }

                return true;
            }
            #endregion

            #region Event listeners
            // BuildRoot is called every time Reload() method is called to ensure that TreeViewItems are created from data.
            protected override TreeViewItem BuildRoot()
            {
                this.OnSearchColumnIndexChange = null;

                var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

                for (int i = 0; i < this.property.arraySize; i++)
                {
                    root.AddChild(new InternalTreeViewItem(this, this.property.GetArrayElementAtIndex(i)));
                }

                return root;
            }

            protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
            {
                var rows = base.BuildRows(root);
                //SortIfNeeded(root, rows); // Sorting here?
                return rows;
            }

            protected override bool CanStartDrag(CanStartDragArgs args)
            {
                return this.canDrag;
            }

            protected override bool CanMultiSelect(TreeViewItem item)
            {
                return this.canMultiselect;
            }

            void OnSortingChanged(MultiColumnHeader multiColumnHeader)
            {
                if (multiColumnHeader.sortedColumnIndex == -1)// || rows.Count <= 1)
                {
                    return; // No column to sort for (just use the order the data are in)
                }

                Debug.LogWarning($"Sorting column index: {multiColumnHeader.sortedColumnIndex}, sorting order is ascending: {multiColumnHeader.IsSortedAscending(multiColumnHeader.sortedColumnIndex)}");

                this.Repaint();
            }

            protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
            {
                // Check if we can handle the current drag data (could be dragged in from other areas/windows in the editor)
                var draggedRows = DragAndDrop.GetGenericData(InternalTreeView.GENERIC_DRAG_ID) as List<TreeViewItem>;

                if (draggedRows == null)
                {
                    return DragAndDropVisualMode.None;
                }

                // Parent item is null when dragging outside any tree view items.
                switch (args.dragAndDropPosition)
                {
                    case DragAndDropPosition.UponItem:
                    case DragAndDropPosition.BetweenItems:

                        int index = args.insertAtIndex < 0 ? this.GetItemIndex(args.parentItem) : args.insertAtIndex;

                        //Debug.Log($"Drag & Drop: Move to target item/s index {index}");

                        bool validDrag = ValidDrag(args.parentItem, draggedRows);
                        if (args.performDrop && validDrag)
                        {
                            //Debug.Log($"Drag & Drop: Drop to target item/s index {index}");

                            //T parentData = ((TreeViewItem<T>)args.parentItem).data;
                            //OnDropDraggedElementsAtIndex(draggedRows, parentData, args.insertAtIndex == -1 ? 0 : args.insertAtIndex);

                            // TODO: Revise this code to finish the drag & drop behaviour:
                            ////LocalizationItem parentData = (LocalizationItem)args.parentItem;
                            //OnDropDraggedElementsAtIndex(draggedRows, parentData, args.insertAtIndex == -1 ? 0 : args.insertAtIndex);
                        }
                        return validDrag ? DragAndDropVisualMode.Move : DragAndDropVisualMode.None;


                    case DragAndDropPosition.OutsideItems:

                        //if (args.performDrop)
                        //{
                        //    Debug.LogError("Drag & Drop: Drop outside of table.");
                        //    //OnDropDraggedElementsAtIndex(draggedRows, this.root, m_TreeModel.root.children.Count);
                        //}
                        //else
                        //{
                        //    Debug.LogError("Drag & Drop: Move outside of table.");
                        //}

                        return DragAndDropVisualMode.Move;

                    default:

                        Debug.LogError($"Unhandled enum: {args.dragAndDropPosition}");
                        return DragAndDropVisualMode.None;
                }
            }

            protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
            {
                if (this.hasSearch) return;

                DragAndDrop.PrepareStartDrag();

                IList<TreeViewItem> draggedRows = this.GetRows().Where(e => args.draggedItemIDs.Contains(e.id)).ToList();
                DragAndDrop.SetGenericData(InternalTreeView.GENERIC_DRAG_ID, draggedRows);
                DragAndDrop.objectReferences = new UnityEngine.Object[] { };

                DragAndDrop.StartDrag(draggedRows.Count == 1 ? draggedRows[0].displayName : "< Multiple >");

                string indexes = string.Empty;

                foreach (var item in args.draggedItemIDs)
                {
                    indexes += $"{this.GetItemIndex(item)}, ";
                }

                Debug.Log($"Drag & Drop: Item index/es to move {indexes.Remove(indexes.Length - 2)}");
            }

            //public event Action<IList<TreeViewItem>> beforeDroppingDraggedItems;

            public virtual void OnDropDraggedElementsAtIndex(List<TreeViewItem> draggedRows, InternalTreeViewItem parent, int insertIndex)
            {
                //if (beforeDroppingDraggedItems != null)
                //    beforeDroppingDraggedItems(draggedRows);

                //var draggedElements = new List<LocalizationItem>();
                //foreach (var x in draggedRows)
                //    draggedElements.Add((LocalizationItem)x);

                //var selectedIDs = draggedElements.Select(x => x.id).ToArray();
                //this.MoveElements(parent, insertIndex, draggedElements);
                //this.SetSelection(selectedIDs, TreeViewSelectionOptions.RevealAndFrame);
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                Rect rect = args.rowRect;
                this.CenterRectUsingSingleLineHeight(ref rect);

                var rowData = (InternalTreeViewItem)args.item;

                for (int i = 0; i < this.propertyNames.Length + 1; i++)
                {
                    this.columnIndexForTreeFoldouts = i;

                    Rect cellRect = this.GetCellRectForTreeFoldouts(rect);
                    {
                        cellRect.xMin += 1.5f;
                        cellRect.xMax -= 1.5f;
                    }

                    if (i == 0 && this.showRowIndex)
                    {
                        EditorGUI.LabelField(cellRect, args.row.ToString());
                    }
                    else if (i > 0 && !string.IsNullOrEmpty(this.propertyNames[i - 1]))
                    {
                        EditorGUI.PropertyField(cellRect, rowData.data[i - 1], GUIContent.none, false);
                    }
                }
            }
            #endregion
        }
        #endregion

        #region Internal vars
        SearchField _searchField;
        InternalTreeView _treeView;
        float _rowColumnWidth;
        #endregion

        #region Properties
        public SerializedProperty SerializedProperty => this._treeView.property;
        public string[] PropertyNames => this._treeView.propertyNames;

        public bool ShowRowIndexColumn { get { return this._treeView.showRowIndex; } set { this._treeView.showRowIndex = value; } }
        public bool ShowSearchField { get; set; }
        public string SearchFieldLabel { get; set; }

        public int RowCount { get; private set; }
        public int CurrentSearchColumnIndex { get; set; }

        public bool CanDrag { get { return this._treeView.canDrag; } set { this._treeView.canDrag = value; } }
        public bool CanMultiselect { get { return this._treeView.canMultiselect; } set { this._treeView.canMultiselect = value; } }
        #endregion

        #region Constructor & Destructor
        public DataTable(SerializedProperty property, DataTableColumn[] columns)
        {
            if (property == null)
            {
                throw new ArgumentNullException("DataTable: The SerializedProperty can't be null!");
            }

            if (!property.isArray)
            {
                throw new ArgumentException("DataTable: The SerializedProperty must be an array!");
            }

            var columnStates = new MultiColumnHeaderState.Column[columns.Length + 1];

            columnStates[0] = new DataTableColumn();

            for (int i = 1; i < columnStates.Length; i++)
            {
                columnStates[i] = columns[i - 1];
            }

            this._rowColumnWidth = Mathf.Clamp(DataTable.ROW_COLUMN_CHAR_WIDTH * property.arraySize.ToString().Length, DataTable.ROW_COLUMN_MIN_WIDTH, DataTable.ROW_COLUMN_MAX_WIDTH);

            this._treeView = new InternalTreeView(columnStates, property, columns.Select(e => e.propertyName).ToArray());
            this._searchField = new SearchField();

            this.SetSearchColumn(0);
        }

        ~DataTable()
        {

        }
        #endregion

        #region Methods & Functions
        public void SetSearchColumn(int columnIndex)
        {
            if (this.PropertyNames.Length > 0)
            {
                this.CurrentSearchColumnIndex = Mathf.Clamp(columnIndex, 0, this.SerializedProperty.arraySize - 1);
                this._treeView.SetSearchColumnIndex(this.CurrentSearchColumnIndex);
            }
        }

        public void Reload()
        {
            this._treeView.Reload();
        }

        public void Do(Rect layout)
        {
            if (this.ShowSearchField)
            {
                layout = this.DrawSearchToolbar(layout);
            }

            this._treeView.multiColumnHeader.state.columns[0].width = this.ShowRowIndexColumn ? this._rowColumnWidth : 0f;
            this._treeView.multiColumnHeader.ResizeToFit();

            this._treeView.OnGUI(layout);
        }

        public void DoLayout(float height)
        {
            this.Do(EditorGUILayout.GetControlRect(false, height));
        }

        Rect DrawSearchToolbar(Rect layout)
        {
            Rect toolBarRect = layout;
            toolBarRect.height = EditorGUIUtility.singleLineHeight + 2f;

            EditorGUI.LabelField(toolBarRect, GUIContent.none, GUIContent.none, EditorStyles.toolbar);

            Rect searchField = EditorGUI.PrefixLabel(toolBarRect, new GUIContent(" "), EditorStyles.miniLabel);
            searchField.x -= 4f;
            searchField.y += 2f;
            searchField.height = EditorGUIUtility.singleLineHeight;

            Rect popupField = toolBarRect;
            popupField.x = toolBarRect.x + 6f;
            popupField.xMax = searchField.x - 6f;

            // TODO: Replace by dropdown button with the label value "Search by column"
            EditorGUI.Popup(popupField, 0, new string[] { "Search by column...", "Key", "Text" }, EditorStyles.toolbarDropDown);

            this._treeView.searchString = this._searchField.OnToolbarGUI(searchField, this._treeView.searchString);

            Rect dataTableRect = layout;
            dataTableRect.y += toolBarRect.height - 1f;
            dataTableRect.height -= toolBarRect.height;

            return dataTableRect;
        }
        #endregion
    }
}

static class IEnumerableExtensionMethods
{
    public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
    {
        return ascending ? source.OrderBy(selector) : source.OrderByDescending(selector);
    }

    public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
    {
        return ascending ? source.ThenBy(selector) : source.ThenByDescending(selector);
    }
}