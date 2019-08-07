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
        #region Classes
        public struct DataTableColumn
        {
            #region Public vars
            public bool autoResize;
            public bool canSort;

            public string headerTitle;
            public TextAlignment haederTextAlignment;

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
                    headerTextAlignment = column.haederTextAlignment,
                    sortingArrowAlignment = column.sortingArrowAlignment,
                    sortedAscending = column.sortedAscending,
                    width = column.width,
                    minWidth = column.minWidth,
                    maxWidth = column.maxWidth
                };
            }
            #endregion
        }

        // TODO: This implementation works with SerializedProperties, allowing save changes from TreeView.
        // TOOD: Found a way to sorting using standard types from SerializedProperty. The sorting behaviour is external to Unity Treeview, then we can implement own sorting code based on any data of this class.
        // TOOD: Maybe only store the parent SerializedProperty (the element retrieved by GetArrayElementAtIndex function) and store the SerializedProperties on constructor (using a unique property name list from the DataTable instance, maybe associated with the column header definition).
        // TODO: The SerializedProperty enable the possibility to draw custom drawer easily in each Treeview cell using EditorGUI.PropertyField.
        class InternalTreeViewItem : TreeViewItem
        {
            #region Static members
            static int _index = 0;
            #endregion

            #region Public vars
            public SerializedProperty[] data;
            #endregion

            #region Constructors
            public InternalTreeViewItem(InternalTreeView treeView, SerializedProperty property) : base(++InternalTreeViewItem._index, 0)
            {
                this.data = new SerializedProperty[treeView.propertyNames.Length];

                for (int i = 0; i < treeView.propertyNames.Length; i++)
                {
                    this.data[i] = property.FindPropertyRelative(treeView.propertyNames[i]);
                }
            }

            ~InternalTreeViewItem()
            {
                InternalTreeViewItem._index--;
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
            #endregion

            #region Constructors
            public InternalTreeView(MultiColumnHeaderState.Column[] columStates, SerializedProperty property, string[] propertyNames) : base(new TreeViewState(), null)
            {
                this.multiColumnHeader = new MultiColumnHeader(new MultiColumnHeaderState(columStates));
                this.multiColumnHeader.sortingChanged += this.OnSortingChanged;

                this.rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                this.showAlternatingRowBackgrounds = this.showBorder = true;

                this.propertyNames = propertyNames;

                this.Reload();
            }
            #endregion

            #region Methods & Functions
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
            protected override TreeViewItem BuildRoot()
            {
                // BuildRoot is called every time Reload is called to ensure that TreeViewItems are created from data.
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
                return true;
            }

            protected override bool CanMultiSelect(TreeViewItem item)
            {
                return true;
            }

            void OnSortingChanged(MultiColumnHeader multiColumnHeader)
            {
                if (multiColumnHeader.sortedColumnIndex == -1)// || rows.Count <= 1)
                {
                    return; // No column to sort for (just use the order the data are in)
                }

                //SortIfNeeded(rootItem, GetRows());
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
                //Rect rect = args.rowRect;
                //this.CenterRectUsingSingleLineHeight(ref rect);

                //T rowData = (T)args.item;

                //for (int i = 0; i < this._columnCount; i++)
                //{
                //    this.columnIndexForTreeFoldouts = i;

                //    Rect cellRect = this.GetCellRectForTreeFoldouts(rect);
                //    cellRect.xMin += 1.5f;
                //    cellRect.xMax -= 1.5f;

                //    if (i == 0 && this.ShowRowIndex)
                //    {
                //        EditorGUI.LabelField(cellRect, args.row.ToString());
                //    }
                //    else
                //    {
                //        this.OnCellGUI(cellRect, args.row, i, rowData);
                //    }
                //}
            }
            #endregion
        }
        #endregion

        #region Internal vars
        SearchField _searchField;
        InternalTreeView _treeView;
        #endregion

        #region Properties
        public SerializedProperty SerializedProperty { get { return this._treeView.property; } }
        public string[] PropertyNames { get { return this._treeView.propertyNames; } }

        public bool ShowRowIndexColumn { get; set; }
        #endregion

        #region Constructor & Destructor
        public DataTable(SerializedProperty property, params DataTableColumn[] columns)
        {
            // TODO: Check if property is an array.

            this._treeView = new InternalTreeView(columns.Cast<MultiColumnHeaderState.Column>().ToArray(), property, columns.Select(e => e.propertyName).ToArray());
            this._searchField = new SearchField();
        }

        ~DataTable()
        {

        } 
        #endregion

        #region Methods & Functions
        public void Do(Rect layout)
        {
            // TODO: Draw the search field.
            this._treeView.OnGUI(layout);
        }

        public void DoLayout()
        {

        }
        #endregion
    }
}

static class IEnumerableExtensionMethods
{
    public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
    {
        if (ascending)
        {
            return source.OrderBy(selector);
        }
        else
        {
            return source.OrderByDescending(selector);
        }
    }

    public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
    {
        if (ascending)
        {
            return source.ThenBy(selector);
        }
        else
        {
            return source.ThenByDescending(selector);
        }
    }
}