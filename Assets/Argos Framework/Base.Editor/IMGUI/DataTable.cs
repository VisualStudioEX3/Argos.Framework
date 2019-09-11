using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Utils;

namespace Argos.Framework.IMGUI
{
    public class DataTable
    {
        #region Constants
        const float ROW_CHAR_WIDTH = 7f;
        const float ROW_COLUMN_MIN_WIDTH = (DataTable.ROW_CHAR_WIDTH * 4) + 4f;
        const float ROW_COLUMN_MAX_WIDTH = (DataTable.ROW_CHAR_WIDTH * 5) + 4f;

        const float SLIDER_FIELD_WIDTH = 50f;
        const float SLIDER_SEPARATOR = 4f;

        const float TOGGLE_WIDTH = 16f;
        const float TOGGLE_HALF_WIDTH = DataTable.TOGGLE_WIDTH / 2f;

        const float ERROR_CONTENT_CELLRECT_CORRECTION = 4f;
        #endregion

        #region Structs
        /// <summary>
        /// Setup a column for a <see cref="DataTable"/> control.
        /// </summary>
        public struct DataTableColumn
        {
            #region Public vars
            /// <summary>
            /// The column auto resize his width size.
            /// </summary>
            public bool autoResize;

            /// <summary>
            /// Enable sorting for this column.
            /// </summary>
            public bool canSort;

            /// <summary>
            /// Header title caption for this column.
            /// </summary>
            public string headerTitle;

            /// <summary>
            /// Header title caption alignment.
            /// </summary>
            public TextAlignment headerTextAlignment;

            /// <summary>
            /// Sorting arrow alignment on header column.
            /// </summary>
            public TextAlignment sortingArrowAlignment;

            /// <summary>
            /// Is sorting asceding?
            /// </summary>
            public bool sortedAscending;

            /// <summary>
            /// Column width.
            /// </summary>
            public float width;

            /// <summary>
            /// Column minimal width.
            /// </summary>
            public float minWidth;

            /// <summary>
            /// Column maximum width.
            /// </summary>
            public float maxWidth;

            /// <summary>
            /// Name of the <see cref="SerializedProperty"/> that contains the value of this column cells.
            /// </summary>
            /// <remarks>If this value is null or empty, or the name is wrong, the <see cref="DataTable"/> shows an error warning when trying to draw the cell.</remarks>
            public string propertyName;

            /// <summary>
            /// Enable <see cref="DataTable.OnCustomCellGUI"/> event for this column.
            /// </summary>
            public bool useCustomGUI;

            /// <summary>
            /// This column is read-only.
            /// </summary>
            public bool readOnly;
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
                    minWidth = column.minWidth,
                    maxWidth = column.maxWidth,
                    width = Mathf.Clamp(column.width, column.minWidth, column.maxWidth)
                };
            }
            #endregion
        }
        #endregion

        #region Classes
        class InternalTreeViewItem : TreeViewItem
        {
            #region Public vars
            public int arrayIndex; // Array index on root Serialized Property, for drag & drop operations.
            public SerializedProperty[] data;
            #endregion

            #region Operators
            public static implicit operator DataTable.DataTableRow(InternalTreeViewItem item) => new DataTable.DataTableRow(item.id, item.data);
            #endregion

            #region Constructors
            public InternalTreeViewItem(InternalTreeView treeView, SerializedProperty property) : base(treeView.GetNewRowIndex(), 0, string.Empty)
            {
                this.arrayIndex = this.id;
                this.data = new SerializedProperty[treeView.columnsSetup.Length];

                for (int i = 0; i < treeView.columnsSetup.Length; i++)
                {
                    if (!string.IsNullOrEmpty(treeView.columnsSetup[i].propertyName))
                    {
                        this.data[i] = property.FindPropertyRelative(treeView.columnsSetup[i].propertyName);
                    }
                }

                this.OnSetSearchColumn(treeView, treeView.searchColumnIndex);

                treeView.OnSearchColumnIndexChange += this.OnSetSearchColumn;
            }
            #endregion

            #region Methods & Functions
            public int CompareTo(InternalTreeViewItem other, int columnIndex)
            {
                switch (this.data[columnIndex].propertyType)
                {
                    case SerializedPropertyType.Boolean:

                        return this.data[columnIndex].boolValue.CompareTo(other.data[columnIndex].boolValue);

                    case SerializedPropertyType.Integer:

                        if (this.data[columnIndex].IsLongValue())
                        {
                            return this.data[columnIndex].longValue.CompareTo(other.data[columnIndex].longValue);
                        }
                        else
                        {
                            return this.data[columnIndex].intValue.CompareTo(other.data[columnIndex].intValue);
                        }

                    case SerializedPropertyType.Float:

                        return this.data[columnIndex].floatValue.CompareTo(other.data[columnIndex].floatValue);

                    case SerializedPropertyType.String:

                        return EditorUtility.NaturalCompare(this.data[columnIndex].stringValue, other.data[columnIndex].stringValue);

                    case SerializedPropertyType.Enum:

                        return EditorUtility.NaturalCompare(this.data[columnIndex].GetEnumDisplayName(), other.data[columnIndex].GetEnumDisplayName());

                    default:

                        return 0;
                }
            }

            public DataTable.DataTableRow ToDataTableRow()
            {
                return new DataTable.DataTableRow(this.id, this.data);
            }
            #endregion

            #region Event listeners
            void OnSetSearchColumn(InternalTreeView treeView, int columnIndex)
            {
                switch (this.data[columnIndex].propertyType)
                {
                    case SerializedPropertyType.Integer:

                        if (this.data[columnIndex].IsLongValue())
                        {
                            this.displayName = this.data[columnIndex].longValue.ToString();
                        }
                        else
                        {
                            this.displayName = this.data[columnIndex].intValue.ToString();
                        }
                        break;

                    case SerializedPropertyType.Float:

                        this.displayName = this.data[columnIndex].floatValue.ToString();
                        break;

                    case SerializedPropertyType.String:

                        this.displayName = this.data[columnIndex].stringValue;
                        break;

                    case SerializedPropertyType.Enum:

                        this.displayName = this.data[columnIndex].GetEnumDisplayName();
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

            #region Structs
            struct InternalDataTableColumnAttribute
            {
                #region Public vars
                public bool isChecked;
                public Attribute attribute;
                #endregion

                #region Methods & Functions
                public T CheckAttribute<T>(SerializedProperty property) where T : Attribute
                {
                    if (!this.isChecked)
                    {
                        this.attribute = property.GetCustomAttribute<T>();
                        this.isChecked = true;
                    }

                    return this.attribute as T;
                }
                #endregion
            }
            #endregion

            #region Classes
            public class InternalDataTableState
            {
                #region Constants
                const string BASE_ID = "Argos.Framework.IMGUI.DataTable.State[{0}.{1}]";
                #endregion

                #region Structs
                [Serializable]
                public struct InternalDataTableColumnState
                {
                    #region Public vars
                    public bool sortedAscending;
                    public float currentWidth;
                    #endregion
                }
                #endregion

                #region Internal vars
                string _id;
                #endregion

                #region Public vars
                public Vector2 scrollPosition;
                public List<int> selectedIDs;
                public int lastClickedID;
                public string searchString;
                public int searchColumnIndex;
                public int sortedColumnIndex;
                public InternalDataTableColumnState[] columnStates;
                public bool isPreviouslySorted;
                #endregion

                #region Properties
                public bool IsLoadedPreviousState { get; private set; }
                #endregion

                #region Constructors
                public InternalDataTableState(string controlID)
                {
                    if (!controlID.IsNullOrEmptyOrWhiteSpace())
                    {
                        this._id = string.Format(InternalDataTableState.BASE_ID, Application.productName, controlID);

                        if (EditorPrefs.HasKey(this._id))
                        {
                            EditorJsonUtility.FromJsonOverwrite(EditorPrefs.GetString(this._id), this);
                            this.IsLoadedPreviousState = true;
                        }
                    }
                }
                #endregion

                #region Methods & Functions
                public void Save(InternalTreeView treeView)
                {
                    if (string.IsNullOrEmpty(this._id)) return;

                    this.scrollPosition = treeView.state.scrollPos;
                    this.selectedIDs = treeView.state.selectedIDs;
                    this.lastClickedID = treeView.state.lastClickedID;
                    this.searchString = treeView.state.searchString;

                    this.searchColumnIndex = treeView.searchColumnIndex;
                    this.columnStates = new InternalDataTableColumnState[treeView.multiColumnHeader.state.columns.Length - 1];
                    for (int i = 1; i < treeView.multiColumnHeader.state.columns.Length; i++)
                    {
                        this.columnStates[i - 1].sortedAscending = treeView.multiColumnHeader.state.columns[i].sortedAscending;
                        this.columnStates[i - 1].currentWidth = treeView.multiColumnHeader.state.columns[i].width;
                    }
                    this.sortedColumnIndex = treeView.sortColumnIndex;

                    this.isPreviouslySorted = treeView.isPreviouslySorted;

                    EditorPrefs.SetString(this._id, EditorJsonUtility.ToJson(this, true));
                }

                public void Delete()
                {
                    if (string.IsNullOrEmpty(this._id)) return;

                    if (EditorPrefs.HasKey(this._id))
                    {
                        EditorPrefs.DeleteKey(this._id);
                    }
                }

                public TreeViewState ToTreeViewState()
                {
                    return !this.IsLoadedPreviousState ?
                        new TreeViewState() :
                        new TreeViewState()
                        {
                            scrollPos = this.scrollPosition,
                            selectedIDs = this.selectedIDs,
                            lastClickedID = this.lastClickedID,
                            searchString = this.searchString
                        };
                }

                public void SetDirty()
                {
                    this.IsLoadedPreviousState = false;
                }
                #endregion
            }
            #endregion

            #region Internal vars
            bool _sortRows;
            InternalDataTableColumnAttribute[] _columnAttributes;
            #endregion

            #region Public vars
            public SerializedProperty property;
            public DataTableColumn[] columnsSetup;
            public InternalDataTableState internalState;

            public int rowCount;
            public bool showRowIndex;

            public int searchColumnIndex = 0;
            public IList<TreeViewItem> searchResult = new List<TreeViewItem>();

            public int sortColumnIndex;
            public bool sortAscending;
            public bool isPreviouslySorted;

            public bool canDrag;
            public bool canMultiselect;
            #endregion

            #region Properties
            public float RowHeight => this.rowHeight;
            public IList<InternalTreeViewItem> Selection => this.GetItemsById(this.GetSelection());
            #endregion

            #region Events
            public event Action<InternalTreeView, int> OnSearchColumnIndexChange;

            public event DataTable.OnCustomCellGUIHandler OnCustomCellGUI;
            public event DataTable.OnRowClickHandler OnRowClick;
            public event DataTable.OnRowClickHandler OnRowDoubleClick;
            public event DataTable.OnRowClickHandler OnRowSelected;
            public event DataTable.OnMultipleRowSelectedHandler OnMultipleRowsSelected;
            #endregion

            #region Static members
            static GUIContent _errorGUIContentNull;
            static GUIContent _errorGUIContentNotImplemented;
            #endregion

            #region Initializers
            [InitializeOnLoadMethod]
            static void Init()
            {
                InternalTreeView._errorGUIContentNull = EditorSkinUtility.Icons.Console.errorIcon.Copy();
                {
                    InternalTreeView._errorGUIContentNull.text = "Null!";
                }

                InternalTreeView._errorGUIContentNotImplemented = EditorSkinUtility.Icons.Console.errorIcon.Copy();
                {
                    InternalTreeView._errorGUIContentNotImplemented.text = "Read-only field not implemented for this type!";
                }
            }
            #endregion

            #region Constructors
            public InternalTreeView(SerializedProperty property, DataTableColumn[] columns, float rowHeight, InternalDataTableState state) : base(state.ToTreeViewState(), null)
            {
                this.internalState = state;
                this.property = property;

                this.columnsSetup = columns;
                this._columnAttributes = new InternalDataTableColumnAttribute[this.columnsSetup.Length];

                this.searchString = this.searchString ?? string.Empty;
                this.rowHeight = rowHeight < 0f ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : rowHeight;
                this.showAlternatingRowBackgrounds = this.showBorder = true;

                var columnStates = new MultiColumnHeaderState.Column[columns.Length + 1];
                {
                    columnStates[0] = new DataTableColumn(); // Row index column.

                    for (int i = 1; i < columnStates.Length; i++)
                    {
                        columnStates[i] = columns[i - 1];
                        if (this.internalState.IsLoadedPreviousState)
                        {
                            columnStates[i].sortedAscending = this.internalState.columnStates[i - 1].sortedAscending;
                            columnStates[i].width = this.internalState.columnStates[i - 1].currentWidth;
                        }
                    }
                }

                this.multiColumnHeader = new MultiColumnHeader(new MultiColumnHeaderState(columnStates));
                this.multiColumnHeader.sortingChanged += this.OnSortingChanged;

                this.Refresh();
            }
            #endregion

            #region Methods & Functions
            public void Refresh()
            {
                if (this.internalState.IsLoadedPreviousState)
                {
                    this.SetSelection(this.state.selectedIDs);

                    if (this.internalState.isPreviouslySorted)
                    {
                        this.multiColumnHeader.sortedColumnIndex = this.internalState.sortedColumnIndex + 1;
                        this.OnSortingChanged(this.multiColumnHeader);
                        return;
                    }
                }

                this.Reload();
            }

            public void ResetState()
            {
                this.isPreviouslySorted = false;
                this.SetSelection(new int[] { }, TreeViewSelectionOptions.None);

                this.internalState.Delete();
            }

            public void SetSearchColumnIndex(int index)
            {
                if (this.searchColumnIndex != index)
                {
                    this.searchColumnIndex = index;
                    this.OnSearchColumnIndexChange?.Invoke(this, index);
                    this.Repaint();
                }
            }

            void CheckInputForRowClickEvent(RowGUIArgs args)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (args.rowRect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.clickCount >= 2)
                        {
                            this.OnRowDoubleClick?.Invoke(args.item as InternalTreeViewItem);
                        }
                        else
                        {
                            this.OnRowClick?.Invoke(args.item as InternalTreeViewItem);
                        }

                        if (!Event.current.control)
                        {
                            if (args.selected && this.canDrag && this.Selection.Count > 1)
                            {
                                return;
                            }

                            this.SetSelection(new int[] { args.item.id });
                        }
                    }
                }
            }

            public int GetNewRowIndex()
            {
                this.rowCount++;
                return this.rowCount - 1;
            }

            public void RenumberRowIndexes()
            {
                this.RenumberRowIndexes(this.rootItem);
            }

            public void RenumberRowIndexes(TreeViewItem root)
            {
                for (int i = 0; i < root.children.Count; i++)
                {
                    root.children[i].id = i;
                }
            }

            InternalTreeViewItem GetItemById(int id)
            {
                return this.FindItem(id, this.rootItem) as InternalTreeViewItem;
            }

            InternalTreeViewItem GetItemByRowIndex(int index)
            {
                return (this.rootItem.children[index] as InternalTreeViewItem);
            }

            IList<InternalTreeViewItem> GetItemsById(IList<int> ids)
            {
                return this.FindRows(ids).Cast<InternalTreeViewItem>().ToList();
            }

            int GetArrayIndex(int itemId)
            {
                return this.GetItemById(itemId).arrayIndex;
            }

            int GetArrayIndex(TreeViewItem item)
            {
                return (item as InternalTreeViewItem).arrayIndex;
            }

            public SerializedProperty[] GetRowData(int index)
            {
                return (this.rootItem.children[index] as InternalTreeViewItem).data;
            }

            public void MoveToRow(int rowIndex)
            {
                this.FrameItem(this.GetItemByRowIndex(rowIndex).id);
            }

            int DragPropertyAtIndex(int sourceArrayIndex, int targetArrayIndex)
            {
                this.property.MoveArrayElement(this.GetItemByRowIndex(sourceArrayIndex).arrayIndex, this.GetItemByRowIndex(targetArrayIndex).arrayIndex);

                return targetArrayIndex;
            }

            // Unity built-in slider control has a bug with the slider input rect that overlaps other controls to right when the slider control width is over 180px.
            // This function implements custom slider control that works and look same as Unity built-in control, and adds the read-only version.
            void DrawFixedSlider(Rect cellRect, SerializedProperty property, RangeAttribute attribute, bool readOnly)
            {
                Rect sliderRect = cellRect;
                {
                    sliderRect.width -= DataTable.SLIDER_FIELD_WIDTH + DataTable.SLIDER_SEPARATOR;
                }

                Rect fieldRect = cellRect;
                {
                    fieldRect.xMin = fieldRect.xMax - DataTable.SLIDER_FIELD_WIDTH;
                }

                if (readOnly)
                {
                    var value = (property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue);

                    GUI.HorizontalSlider(sliderRect, value, attribute.min, attribute.max);
                    EditorGUI.SelectableLabel(fieldRect, value.ToString(), EditorStyles.textField);
                }
                else
                {
                    if (property.propertyType == SerializedPropertyType.Integer)
                    {
                        this.DrawIntSlider(sliderRect, fieldRect, property, attribute);
                    }
                    else
                    {
                        this.DrawFloatSlider(sliderRect, fieldRect, property, attribute);
                    }
                }
            }

            void DrawIntSlider(Rect sliderRect, Rect fieldRect, SerializedProperty property, RangeAttribute attribute)
            {
                int value = property.intValue;
                value = (int)GUI.HorizontalSlider(sliderRect, value, attribute.min, attribute.max);
                value = EditorGUI.IntField(fieldRect, value, EditorStyles.textField);
                property.intValue = value;
            }

            void DrawFloatSlider(Rect sliderRect, Rect fieldRect, SerializedProperty property, RangeAttribute attribute)
            {
                float value = property.floatValue;
                value = GUI.HorizontalSlider(sliderRect, value, attribute.min, attribute.max);
                value = EditorGUI.FloatField(fieldRect, value, EditorStyles.textField);
                property.floatValue = value;
            }

            void DrawReadOnlyProperty(Rect cellRect, SerializedProperty property, int columnSetupIndex)
            {
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.Float:

                        var rangeAttribute = this._columnAttributes[columnSetupIndex].CheckAttribute<RangeAttribute>(property);

                        if (rangeAttribute != null)
                        {
                            this.DrawFixedSlider(cellRect, property, rangeAttribute, true);
                        }
                        else
                        {
                            EditorGUI.SelectableLabel(cellRect, (property.propertyType == SerializedPropertyType.Integer ? (property.IsLongValue() ? property.longValue : property.intValue) : property.floatValue).ToString(), EditorStyles.textField);
                        }
                        break;

                    case SerializedPropertyType.Boolean:

                        EditorGUI.Toggle(cellRect, GUIContent.none, property.boolValue);
                        break;

                    case SerializedPropertyType.String:

                        EditorGUI.SelectableLabel(cellRect, property.stringValue, EditorStyles.textField);
                        break;

                    case SerializedPropertyType.Color:

                        var colorAttribute = this._columnAttributes[columnSetupIndex].CheckAttribute<ColorUsageAttribute>(property);
                        EditorGUI.ColorField(cellRect, GUIContent.none, property.colorValue, false, (colorAttribute != null) ? colorAttribute.showAlpha : true, (colorAttribute != null) ? colorAttribute.hdr : false);
                        break;

                    case SerializedPropertyType.LayerMask:

                        int currentMask = property.intValue;
                        EditorGUI.PropertyField(cellRect, property, GUIContent.none);
                        property.intValue = currentMask;
                        break;

                    case SerializedPropertyType.Enum:

                        EditorGUI.Popup(cellRect, property.enumValueIndex, property.enumDisplayNames);
                        break;

                    case SerializedPropertyType.Vector2:

                        EditorGUI.Vector2Field(cellRect, GUIContent.none, property.vector2Value);
                        break;

                    case SerializedPropertyType.Vector3:

                        EditorGUI.Vector3Field(cellRect, GUIContent.none, property.vector3Value);
                        break;

                    case SerializedPropertyType.Vector4:

                        EditorGUI.Vector4Field(cellRect, GUIContent.none, property.vector4Value);
                        break;

                    case SerializedPropertyType.Rect:

                        EditorGUI.RectField(cellRect, GUIContent.none, property.rectValue);
                        break;

                    case SerializedPropertyType.Character:

                        EditorGUI.SelectableLabel(cellRect, $"Char: '{(char)property.intValue}', Code: {property.intValue}");
                        break;

                    case SerializedPropertyType.AnimationCurve:

                        EditorGUI.CurveField(cellRect, property.animationCurveValue);
                        break;

                    case SerializedPropertyType.Bounds:

                        EditorGUI.BoundsField(cellRect, GUIContent.none, property.boundsValue);
                        break;

                    case SerializedPropertyType.Gradient:

                        EditorGUI.GradientField(cellRect, property.GetGradientValue());
                        break;

                    case SerializedPropertyType.Quaternion:

                        EditorGUI.Vector4Field(cellRect, GUIContent.none, new Vector4(property.quaternionValue.x, property.quaternionValue.y, property.quaternionValue.z, property.quaternionValue.w));
                        break;

                    case SerializedPropertyType.Vector2Int:

                        EditorGUI.Vector2IntField(cellRect, GUIContent.none, property.vector2IntValue);
                        break;

                    case SerializedPropertyType.Vector3Int:

                        EditorGUI.Vector3IntField(cellRect, GUIContent.none, property.vector3IntValue);
                        break;

                    case SerializedPropertyType.RectInt:

                        EditorGUI.RectIntField(cellRect, GUIContent.none, property.rectIntValue);
                        break;

                    case SerializedPropertyType.BoundsInt:

                        EditorGUI.BoundsIntField(cellRect, GUIContent.none, property.boundsIntValue);
                        break;

                    default:

                        GUI.enabled = false;
                        EditorGUI.PropertyField(cellRect, property, GUIContent.none, false);
                        GUI.enabled = true;
                        break;
                }
            }

            void DrawDragIcon(Rect rect)
            {

            }
            #endregion

            #region Event listeners
            protected override TreeViewItem BuildRoot()
            {
                this.rowCount = 0;
                this.OnSearchColumnIndexChange = null;

                var root = new TreeViewItem { id = -1, depth = -1, displayName = string.Empty };

                if (this.property.arraySize > 0)
                {
                    for (int i = 0; i < this.property.arraySize; i++)
                    {
                        root.AddChild(new InternalTreeViewItem(this, this.property.GetArrayElementAtIndex(i)));
                    }

                    if (this._sortRows)
                    {
                        // FYI: I can't achieve sorting directly rootItem.Children list out of this event. I can't understand 100% why can clear list or remove elements on overrided BuildRows event but can apply this code outside (a separated method to apply sorting).
                        // This way is the only solution I found, reload entirely all data from data source and apply the new sorting. I think that may have better way to do this... but whatever...

                        root.children.Sort((x, y) => (this.sortAscending ? 1 : -1) * (x as InternalTreeViewItem).CompareTo(y as InternalTreeViewItem, this.sortColumnIndex));
                        this.RenumberRowIndexes(root);
                        this._sortRows = false;
                    }

                    if (this.internalState.IsLoadedPreviousState)
                    {
                        this.SetSearchColumnIndex(this.internalState.searchColumnIndex);
                    }
                }
                else
                {
                    root.children = new List<TreeViewItem>();
                }

                return root;
            }

            protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
            {
                IList<TreeViewItem> rows = base.BuildRows(root);

                if (this.hasSearch)
                {
                    this.searchResult = rows;
                }

                return rows;
            }

            protected override bool CanStartDrag(CanStartDragArgs args)
            {
                return this.canDrag && !this.hasSearch && !this.isPreviouslySorted && (this.GetSelection().Count != this.rowCount);
            }

            protected override bool CanMultiSelect(TreeViewItem item)
            {
                return this.canMultiselect;
            }

            void OnSortingChanged(MultiColumnHeader multiColumnHeader)
            {
                if (multiColumnHeader.sortedColumnIndex == -1 || this.property.arraySize <= 2) return;

                this.sortColumnIndex = multiColumnHeader.sortedColumnIndex - 1;
                this.sortAscending = multiColumnHeader.IsSortedAscending(multiColumnHeader.sortedColumnIndex);
                this._sortRows = this.isPreviouslySorted = true;

                this.Reload();
            }

            protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
            {
                // Check if we can handle the current drag data (could be dragged in from other areas/windows in the editor)
                var draggedRows = DragAndDrop.GetGenericData(InternalTreeView.GENERIC_DRAG_ID) as List<TreeViewItem>;

                if (draggedRows == null)
                {
                    return DragAndDropVisualMode.None;
                }

                switch (args.dragAndDropPosition)
                {
                    case DragAndDropPosition.UponItem:
                    case DragAndDropPosition.BetweenItems:

                        int insertAtArrayIndex = -1;
                        int first, last;
                        this.GetFirstAndLastVisibleRows(out first, out last);

                        for (int i = first; i < last + 1; i++)
                        {
                            if (this.GetRowRect(i).Contains(Event.current.mousePosition))
                            {
                                insertAtArrayIndex = this.GetItemByRowIndex(i).arrayIndex;
                                break;
                            }
                        }

                        int firstIndex = this.GetArrayIndex(draggedRows[0]);
                        bool validDrag = (insertAtArrayIndex != firstIndex);
                        var updatedSelection = new int[draggedRows.Count];
                        if (args.performDrop && validDrag)
                        {
                            if (firstIndex < insertAtArrayIndex)
                            {
                                int targetIndex = insertAtArrayIndex + draggedRows.Count - 1;
                                if (targetIndex > rowCount - 1)
                                {
                                    targetIndex -= (targetIndex - rowCount) + 1;
                                }

                                for (int moveIndex = draggedRows.Count - 1, toIndex = targetIndex; moveIndex >= 0; moveIndex--, toIndex--)
                                {
                                    updatedSelection[moveIndex] = this.DragPropertyAtIndex(this.GetArrayIndex(draggedRows[moveIndex]), toIndex);
                                } 
                            }
                            else
                            {
                                for (int moveIndex = 0, toIndex = insertAtArrayIndex; moveIndex < draggedRows.Count; moveIndex++, toIndex++)
                                {
                                    updatedSelection[moveIndex] = this.DragPropertyAtIndex(this.GetArrayIndex(draggedRows[moveIndex]), toIndex);
                                }
                            }
                        }

                        this.RenumberRowIndexes();
                        this.SetSelection(updatedSelection, TreeViewSelectionOptions.None);

                        return validDrag ? DragAndDropVisualMode.Move : DragAndDropVisualMode.Rejected;

                    case DragAndDropPosition.OutsideItems:
                    default:

                        return DragAndDropVisualMode.Rejected;
                }
            }

            protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
            {
                if (this.hasSearch) return;

                IList<TreeViewItem> draggedRows = this.GetRows().Where(e => args.draggedItemIDs.Contains(e.id)).ToList();

                DragAndDrop.PrepareStartDrag();
                {
                    DragAndDrop.SetGenericData(InternalTreeView.GENERIC_DRAG_ID, draggedRows);
                    DragAndDrop.objectReferences = new UnityEngine.Object[] { };
                }
                DragAndDrop.StartDrag(draggedRows.Count == 1 ? draggedRows[0].displayName : "< Multiple >");
            }

            protected override void SelectionChanged(IList<int> selectedIds)
            {
                if (this.OnRowSelected != null && (!this.canMultiselect || (this.canMultiselect && selectedIds.Count == 1)))
                {
                    this.OnRowSelected(this.GetItemById(selectedIds[0]));
                }
                else if (this.OnMultipleRowsSelected != null && this.canMultiselect)
                {
                    this.OnMultipleRowsSelected(this.GetItemsById(selectedIds).Select(e => e.ToDataTableRow()).ToArray());
                }
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                this.CheckInputForRowClickEvent(args);
                this.CenterRectUsingSingleLineHeight(ref args.rowRect);

                var rowData = (args.item as InternalTreeViewItem).data;

                for (int i = 0; i < this.columnsSetup.Length + 1; i++)
                {
                    if (!this.multiColumnHeader.IsColumnVisible(i)) continue;

                    Rect cellRect = this.multiColumnHeader.GetCellRect(i, args.rowRect);
                    {
                        cellRect.width = this.multiColumnHeader.GetColumn(i).width;
                        cellRect.xMin++;
                        cellRect.xMax--;
                    }

                    int columnIndex = i - 1;

                    if (i == 0 && (this.showRowIndex || this.canDrag))
                    {
                        if (this.canDrag)
                        {
                            Rect dragHandleRect = cellRect;
                            {
                                dragHandleRect.x += 2f;
                                dragHandleRect.y += 5f;
                                dragHandleRect.width = 10f;
                                dragHandleRect.height -= (dragHandleRect.height - 7f);
                            }
                            EditorSkinUtility.Styles.Custom.ReorderableList.dragHandle.SafeDraw(dragHandleRect, false, false, false, false); 
                        }

                        if (this.showRowIndex)
                        {
                            Rect numberRect = cellRect;
                            {
                                if (this.canDrag)
                                {
                                    numberRect.xMin = 16f;
                                }
                            }
                            EditorGUI.LabelField(numberRect, (args.row + 1).ToString()); 
                        }
                    }
                    else if (i > 0 && !string.IsNullOrEmpty(this.columnsSetup[columnIndex].propertyName))
                    {
                        SerializedProperty cellProperty = rowData[columnIndex];

                        if (cellProperty != null)
                        {
                            if (this.columnsSetup[columnIndex].useCustomGUI)
                            {
                                this.OnCustomCellGUI?.Invoke(cellRect, cellProperty);
                            }
                            else
                            {
                                if (cellProperty.propertyType == SerializedPropertyType.Boolean)
                                {
                                    cellRect.x += (cellRect.width * 0.5f) - DataTable.TOGGLE_HALF_WIDTH;
                                    cellRect.width = DataTable.TOGGLE_WIDTH;
                                }

                                if (this.columnsSetup[columnIndex].readOnly)
                                {
                                    this.DrawReadOnlyProperty(cellRect, cellProperty, columnIndex);
                                }
                                else
                                {
                                    var rangeAttribute = this._columnAttributes[columnIndex].CheckAttribute<RangeAttribute>(cellProperty);

                                    if (rangeAttribute != null)
                                    {
                                        // Unity built-in slider control has a bug with the slider input rect that overlaps other controls on the right when the slider control width is over 180px.
                                        // This function implements custom slider control that works and look same as Unity built-in control, and adds the read-only version.
                                        this.DrawFixedSlider(cellRect, cellProperty, rangeAttribute, false);
                                    }
                                    else
                                    {
                                        EditorGUI.PropertyField(cellRect, cellProperty, GUIContent.none, false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            cellRect.y -= DataTable.ERROR_CONTENT_CELLRECT_CORRECTION;
                            cellRect.height += DataTable.ERROR_CONTENT_CELLRECT_CORRECTION;

                            EditorGUI.LabelField(cellRect, InternalTreeView._errorGUIContentNull, EditorSkinUtility.Styles.ArgosCustomVariants.errorMiniLabel);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// DataTable row object.
        /// </summary>
        public class DataTableRow
        {
            #region Properties
            /// <summary>
            /// Row index.
            /// </summary>
            public int Row { get; private set; }

            /// <summary>
            /// <see cref="SerializedProperty"/> array with the row data.
            /// </summary>
            public SerializedProperty[] Data { get; private set; }
            #endregion

            #region Constructor
            public DataTableRow(int row, SerializedProperty[] data)
            {
                this.Row = row;
                this.Data = data;
            } 
            #endregion
        }
        #endregion

        #region Internal vars
        SearchField _searchField;
        InternalTreeView _treeView;
        float _indexRowColumnWidth;
        float _indexDragAndDropRowColumnWidth;
        #endregion

        #region Properties
        /// <summary>
        /// Source data of this <see cref="DataTable"/> control. Must be an array.
        /// </summary>
        public SerializedProperty Property => this._treeView.property;

        /// <summary>
        /// Names of the each <see cref="SerializedProperty"/> for each column.
        /// </summary>
        public string[] PropertyNames => this._treeView.columnsSetup.Select(e => e.propertyName).ToArray();

        /// <summary>
        /// Shows a column in the left with the index of each row.
        /// </summary>
        public bool ShowRowIndexColumn
        {
            get { return this._treeView.showRowIndex; }
            set { this._treeView.showRowIndex = value; }
        }

        /// <summary>
        /// Enables the search field that allow filter results for column values.
        /// </summary>
        public bool ShowSearchField { get; set; }

        /// <summary>
        /// Shows footer area to customize data.
        /// </summary>
        public bool ShowFooter { get; set; }

        /// <summary>
        /// Current row selection.
        /// </summary>
        public DataTableRow[] Selection => this.ToDataTableRowArray(this._treeView.Selection);

        /// <summary>
        /// The current search result, sorted alphabetically.
        /// </summary>
        public DataTableRow[] SearchResult => this.ToDataTableRowArray(this._treeView.searchResult);

        /// <summary>
        /// Get all rows.
        /// </summary>
        public DataTableRow[] Rows => this.ToDataTableRowArray(this._treeView.GetRows());

        /// <summary>
        /// Row count.
        /// </summary>
        public int RowCount => (this.Property != null && this.Property.isArray) ? this.Property.arraySize : 0;

        /// <summary>
        /// Row height.
        /// </summary>
        public float RowHeight => this._treeView.RowHeight;

        /// <summary>
        /// The index of the column used by search field to filter data.
        /// </summary>
        /// <remarks>The row sorting feature only works with basic types: <see cref="bool"/>, <see cref="int"/>, <see cref="long"/>, <see cref="float"/>, <see cref="string"/> and <see cref="Enum"/> values, treated as <see cref="string"/> values. Other types sets the original row order. 
        /// The <see cref="string"/> and <see cref="Enum"/> values are sorting using <see cref="EditorUtility.NaturalCompare(string, string)"/> function.</remarks>
        public int CurrentSearchColumnIndex
        {
            get { return this._treeView.searchColumnIndex; }
            set { this._treeView.searchColumnIndex = value; }
        }

        /// <summary>
        /// Enable this to automatic resize all columns to fit in the control width size.
        /// </summary>
        public bool ResizeToFitColumns { get; set; }

        /// <summary>
        /// User can drag and drop rows?
        /// </summary>
        /// <remarks>Drag & drog operations only allowed when sorting columns is disabled and not search is applied.</remarks>
        public bool CanDrag
        {
            get { return this._treeView.canDrag; }
            set { this._treeView.canDrag = value; }
        }

        /// <summary>
        /// User can select multiple rows at once?
        /// </summary>
        public bool CanMultiselect
        {
            get { return this._treeView.canMultiselect; }
            set { this._treeView.canMultiselect = value; }
        }
        #endregion

        #region Delegates
        /// <summary>
        /// Custom cell GUI handler.
        /// </summary>
        /// <param name="cellRect">Cell area on inspector.</param>
        /// <param name="data"><see cref="SerializedProperty"/> data of the cell.</param>
        public delegate void OnCustomCellGUIHandler(Rect cellRect, SerializedProperty data);

        /// <summary>
        /// Row cell click handler.
        /// </summary>
        /// <param name="rowIndex">Row index on users click.</param>
        /// <param name="data"><see cref="SerializedProperty"/> array data of the row.</param>
        public delegate void OnRowClickHandler(DataTableRow row);

        /// <summary>
        /// Multiple rows selection handler.
        /// </summary>
        /// <param name="rowIndexes">Array of indexes of selected rows.</param>
        /// <param name="data">Array of <see cref="SerializedProperty"/> arrays, each array item is the data of each selected row.</param>
        public delegate void OnMultipleRowSelectedHandler(DataTableRow[] rows);

        /// <summary>
        /// Rect area for GUI operations.
        /// </summary>
        /// <param name="rect"><see cref="Rect"/> value with the label area.</param>
        public delegate void OnRectGUIHandler(Rect rect);
        #endregion

        #region Events
        /// <summary>
        /// Event used to customized data representation of a column cell.
        /// </summary>
        /// <see cref="DataTableColumn.useCustomGUI"/>
        public event OnCustomCellGUIHandler OnCustomCellGUI
        {
            add { this._treeView.OnCustomCellGUI += value; }
            remove { this._treeView.OnCustomCellGUI -= value; }
        }

        /// <summary>
        /// Event used to get user click on a row, and get the data.
        /// </summary>
        public event OnRowClickHandler OnRowClick
        {
            add { this._treeView.OnRowClick += value; }
            remove { this._treeView.OnRowClick -= value; }
        }

        /// <summary>
        /// Event used to get user double-click on a row, and get the data.
        /// </summary>
        public event OnRowClickHandler OnRowDoubleClick
        {
            add { this._treeView.OnRowDoubleClick += value; }
            remove { this._treeView.OnRowDoubleClick -= value; }
        }

        /// <summary>
        /// Event used to get a user selected row.
        /// </summary>
        public event OnRowClickHandler OnRowSelected
        {
            add { this._treeView.OnRowSelected += value; }
            remove { this._treeView.OnRowSelected -= value; }
        }

        /// <summary>
        /// Event used to get user multiple row selection.
        /// </summary>
        public event OnMultipleRowSelectedHandler OnMultipleRowsSelected
        {
            add { this._treeView.OnMultipleRowsSelected += value;  }
            remove { this._treeView.OnMultipleRowsSelected -= value; }
        }

        /// <summary>
        /// Event used to get the toolbar area when the search field is active. Useful to draw custom labels or toolbar controls or any kind of GUI stuff.
        /// </summary>
        public event OnRectGUIHandler OnToobarSearchGUI;

        /// <summary>
        /// Event used to get the footer area of control.
        /// </summary>
        public event OnRectGUIHandler OnFooterGUI;
        #endregion

        #region Constructor & Destructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="property"><see cref="SerializedProperty"/> soruce data. Must be an array.</param>
        /// <param name="columns">Definition of each column.</param>
        /// <param name="rowHeight">Row height. By default is the default height of a normal control.</param>
        /// <param name="resizeToFitColumns">Enable this to automatic resize all columns to fit in the control width size. By default is true. This value can be changed via <see cref="ResizeToFitColumns"/>.</param>
        /// <param name="controlID">A unique id that identifies this control instance to allow save control state.</param>
        public DataTable(SerializedProperty property, DataTableColumn[] columns, float rowHeight = -1f, bool resizeToFitColumns = true, string controlID = "")
        {
            if (property == null)
            {
                throw new ArgumentNullException($"DataTable.ctor(): The SerializedProperty \"{property.name}\" can't be null!");
            }

            if (!property.isArray)
            {
                throw new ArgumentException($"DataTable.ctor(): The SerializedProperty \"{property.name}\" must be an array!");
            }

            this._indexRowColumnWidth = Mathf.Clamp(DataTable.ROW_CHAR_WIDTH * property.arraySize.ToString().Length, DataTable.ROW_COLUMN_MIN_WIDTH, DataTable.ROW_COLUMN_MAX_WIDTH);
            this._indexDragAndDropRowColumnWidth = this._indexRowColumnWidth + 14f;

            var treeviewState = new InternalTreeView.InternalDataTableState(controlID);
            this._treeView = new InternalTreeView(property, columns, rowHeight, treeviewState);
            this._searchField = new SearchField();
            {
                this._searchField.DropDownItems = columns.Select(e => e.headerTitle).ToArray();
                this._searchField.OnDropDownSelect += this.SetSearchColumn;

                if (treeviewState.IsLoadedPreviousState)
                {
                    this._searchField.DropDownSelection = treeviewState.searchColumnIndex;
                    treeviewState.SetDirty();
                }
            }

            this.ResizeToFitColumns = resizeToFitColumns;
        }

        ~DataTable()
        {
            this._searchField.OnDropDownSelect -= this.SetSearchColumn;
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Sets the column used by search field to filter data.
        /// </summary>
        /// <param name="columnIndex">Index of column. (0 is the first user defined column. Row index column not count).</param>
        /// <remarks>The row sorting feature only works with basic types: <see cref="bool"/>, <see cref="int"/>, <see cref="long"/>, <see cref="float"/>, <see cref="string"/> and <see cref="Enum"/> values, treated as <see cref="string"/> values. Other types sets the original row order. 
        /// The <see cref="string"/> and <see cref="Enum"/> values are sorting using <see cref="EditorUtility.NaturalCompare(string, string)"/> function.</remarks>
        public void SetSearchColumn(int columnIndex)
        {
            if (this.PropertyNames.Length > 0)
            {
                this.CurrentSearchColumnIndex = Mathf.Clamp(columnIndex, 0, this.Property.arraySize - 1);
                this._treeView.SetSearchColumnIndex(this.CurrentSearchColumnIndex);
            }
        }

        /// <summary>
        /// Reload all data from data source.
        /// </summary>
        public void Reload()
        {
            this._treeView.Refresh();
        }

        /// <summary>
        /// Get all fields data from a row.
        /// </summary>
        /// <param name="index">Row index.</param>
        /// <returns>Returns a <see cref="SerializedProperty"/> array with data of each row fields.</returns>
        public SerializedProperty[] GetRowData(int index)
        {
            if (MathUtility.IsClamped(index, 0, this.RowCount - 1))
            {
                return this._treeView.GetRowData(index);
            }

            throw new IndexOutOfRangeException("DataTable: The index of row not exists.");
        }

        float GetZeroColumnWidth()
        {
            if (this.ShowRowIndexColumn && this.CanDrag)
            {
                return this._indexDragAndDropRowColumnWidth;
            }
            else if (this.ShowRowIndexColumn && !this.CanDrag)
            {
                return this._indexRowColumnWidth;
            }
            else if (!this.ShowRowIndexColumn && this.CanDrag)
            {
                return 20f;
            }
            else
            {
                return 0f;
            }
        }

        Rect DrawHeaderBackground(Rect rect)
        {
            rect.xMax--;
            rect.height = EditorGUIUtility.singleLineHeight + 1f;

            EditorGUI.LabelField(rect, GUIContent.none, EditorStyles.toolbarButton);

            return rect;
        }

        Rect DrawFooterBackground(Rect rect)
        {
            rect.yMin = rect.yMax - (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

            EditorGUI.LabelField(rect, GUIContent.none, EditorSkinUtility.Styles.Custom.greyBorder);

            return rect;
        }

        /// <summary>
        /// Draws the <see cref="DataTable"/>.
        /// </summary>
        /// <param name="layout">Rect that positioned the control.</param>
        public void Do(Rect layout)
        {
            if (this.ShowSearchField)
            {
                Rect headerRect = this.DrawHeaderBackground(layout);
                Rect searchFieldRect = headerRect;
                {
                    searchFieldRect.x += EditorGUIUtility.labelWidth;
                    searchFieldRect.y += 2f;
                    searchFieldRect.xMax = headerRect.xMax - 3f;
                }
                this._treeView.searchString = this._searchField.Do(searchFieldRect, this._treeView.searchString);

                layout.yMin = headerRect.yMax;

                if (this.OnToobarSearchGUI != null)
                {
                    Rect toolbarRect = headerRect;
                    {
                        toolbarRect.width = EditorGUIUtility.labelWidth - 4f;
                    }
                    this.OnToobarSearchGUI(toolbarRect);
                }
            }

            this._treeView.multiColumnHeader.state.columns[0].width = this.GetZeroColumnWidth();

            if (this.ResizeToFitColumns)
            {
                this._treeView.multiColumnHeader.ResizeToFit();
            }

            if (this.ShowFooter && this.OnFooterGUI != null)
            {
                Rect footerRect = this.DrawFooterBackground(layout);

                if (this.OnFooterGUI != null)
                {
                    Rect footerGUIRect = footerRect;
                    {
                        footerGUIRect.xMin += 2f;
                        footerGUIRect.xMax -= 2f;
                    }

                    this.OnFooterGUI(footerGUIRect); 
                }

                layout.yMax -= footerRect.height;
            }

            this._treeView.OnGUI(layout);
        }

        /// <summary>
        /// Draws the <see cref="DataTable"/> using the current inspector layout.
        /// </summary>
        /// <param name="rows">Number of visible rows. If search field is enable, the control will occupy the first row.</param>
        public void DoLayout(int rows = 10)
        {
            float height = this._treeView.multiColumnHeader.height + (this._treeView.RowHeight * rows) + 3f;
            this.Do(EditorGUILayout.GetControlRect(false, height));
        }

        /// <summary>
        /// Saves the control state. 
        /// </summary>
        public void SaveState()
        {
            this._treeView.internalState.Save(this._treeView);
        }

        /// <summary>
        /// Delete previous control state and reset current state.
        /// </summary>
        public void DeleteState()
        {
            this._treeView.ResetState();
        }

        /// <summary>
        /// Set focus to this control.
        /// </summary>
        public void SetFocus()
        {
            this._treeView.SetFocusAndEnsureSelectedItem();
        }

        /// <summary>
        /// Select all rows.
        /// </summary>
        public void SelectAllRows()
        {
            this._treeView.SelectAllRows();
        }

        /// <summary>
        /// Move scroll view to row.
        /// </summary>
        /// <param name="row">Row index.</param>
        public void MoveToRow(int row)
        {
            this._treeView.MoveToRow(row);
        }

        DataTableRow[] ToDataTableRowArray(IList<TreeViewItem> list)
        {
            return list.Select(e => (e as InternalTreeViewItem).ToDataTableRow()).ToArray();
        }

        DataTableRow[] ToDataTableRowArray(IList<InternalTreeViewItem> list)
        {
            return list.Select(e => e.ToDataTableRow()).ToArray();
        }
        #endregion
    }
}