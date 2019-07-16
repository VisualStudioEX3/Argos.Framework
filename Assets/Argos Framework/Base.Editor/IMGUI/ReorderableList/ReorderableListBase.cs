using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityReorderableList = UnityEditorInternal.ReorderableList;

namespace Argos.Framework.IMGUI
{
    /// <summary>
    /// Unity ReorderableList wrapper base class.
    /// </summary>
    /// <remarks>Use this class to easily implement custom ReorderableList classes with custom behaviours and styles.</remarks>
    public abstract class ReorderableListBase : IDisposable
    {
        #region Constants
        const float HEADER_NONE_HEIGHT = 3f;
        #endregion

        #region Enums
        /// <summary>
        /// ReorderableList Add button types.
        /// </summary>
        public enum ReorderableListAddButtonType
        {
            /// <summary>
            /// No show add element button.
            /// </summary>
            None,
            /// <summary>
            /// Shows default add element button.
            /// </summary>
            Default,
            /// <summary>
            /// Shows dropdown add element button.
            /// </summary>
            Dropdown
        }
        #endregion

        #region Internal vars
        UnityReorderableList _instance;
        UnityReorderableList.Defaults _defaultBehaviours;
        bool _disposed;
        #endregion

        #region Properties
        public SerializedObject SerializedObject { get { return this.Elements.serializedObject; } }
        /// <summary>
        /// Serialized Property that references the serialized element list.
        /// </summary>
        public SerializedProperty Elements { get { return this._instance.serializedProperty; } }
        /// <summary>
        /// Shortcut to access an element by index.
        /// </summary>
        /// <param name="index">Index of the element in list.</param>
        /// <returns>Return the reference to a specific element.</returns>
        public SerializedProperty this[int index] { get { return this.Elements.GetArrayElementAtIndex(index); } }
        /// <summary>
        /// Current element index selected.
        /// </summary>
        /// <remarks>The setted value is always clamped between -1 and the element count - 1.</remarks>
        public int Index { get { return this._instance.count == 0 ? -1 : this._instance.index; } set { this._instance.index = Mathf.Clamp(value, -1, this._instance.count - 1); } }
        /// <summary>
        /// Element count.
        /// </summary>
        public int Count { get { return this._instance.count; } }
        /// <summary>
        /// Get the height of the ReorderableList layout.
        /// </summary>
        public float Height { get { return this._instance.GetHeight(); } }
        /// <summary>
        /// This list allow to reorder elements by mouse dragging?
        /// </summary>
        public bool IsDraggable { get; private set; }
        /// <summary>
        /// This list shows a header?
        /// </summary>
        public bool DisplayHeader { get; private set; }
        /// <summary>
        /// The add button behaviour of this list.
        /// </summary>
        public ReorderableListAddButtonType DisplayAddButton { get; private set; }
        /// <summary>
        /// This list shows a removed element button?
        /// </summary>
        public bool DisplayRemoveButton { get; private set; }
        /// <summary>
        /// Force to draw the default background style. If this values is false, the list only render the controls but not the border and background containers. Useful to customize the entire control list style.
        /// </summary>
        public bool ShowDefaultBackground { get { return this._instance.showDefaultBackground; } set { this._instance.showDefaultBackground = value; } }
        #endregion

        #region Constructors & Destructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="elements">Serialized Property reference to element list.</param>
        /// <param name="isDraggable">Allow to reorder the elements by mouse dragging?</param>
        /// <param name="displayHeader">Shows a header.</param>
        /// <param name="displayAddButton">Shows add element button.</param>
        /// <param name="displayRemoveButton">Shows remove element button.</param>
        public ReorderableListBase(SerializedProperty elements, bool isDraggable = true, bool displayHeader = false, ReorderableListAddButtonType displayAddButton = ReorderableListAddButtonType.Default, bool displayRemoveButton = true)
        {
            if (elements == null)
            {
                this.Dispose();
                throw new ArgumentNullException(nameof(elements), "(ReorderableListBase) Parameter can't be null!");
            }

            this._instance = new UnityReorderableList(elements.serializedObject, elements, isDraggable, displayHeader, displayAddButton != ReorderableListAddButtonType.None, displayRemoveButton);
            {
                this._instance.drawElementBackgroundCallback += this.OnElementBackgroundGUIInternal;
                this._instance.drawElementCallback += this.OnElementGUIInternal;
                this._instance.drawFooterCallback += this.OnFooterGUI;
                this._instance.drawHeaderCallback += this.OnHeaderGUI;
                this._instance.drawNoneElementCallback += this.OnNoneElementGUI;
                this._instance.elementHeightCallback += this.OnElementHeightInternal;

                if (displayAddButton == ReorderableListAddButtonType.Default)
                {
                    this._instance.onAddCallback += this.OnAddElementInternal;
                }
                else if (displayAddButton == ReorderableListAddButtonType.Dropdown)
                {
                    this._instance.onAddDropdownCallback += this.OnAddDropdownInternal;
                }

                this._instance.onCanAddCallback += this.OnCanAddInternal;
                this._instance.onCanRemoveCallback += this.OnCanRemoveInternal;
                this._instance.onChangedCallback += this.OnChangedElementInternal;
                this._instance.onMouseUpCallback += this.OnMouseUpElementInternal;
                this._instance.onRemoveCallback += this.OnRemoveElementInternal;
                this._instance.onReorderCallbackWithDetails += this.OnReorderElementInternal;
                this._instance.onSelectCallback += this.OnSelectElementInternal;
            }

            this.IsDraggable = isDraggable;
            this.DisplayHeader = displayHeader;
            this.DisplayAddButton = displayAddButton;
            this.DisplayRemoveButton = displayRemoveButton;
        }

        ~ReorderableListBase()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Release all internal event listeners and memory used before the instance is destroyed.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (this._instance != null)
                {
                    this._instance.drawElementBackgroundCallback -= this.OnElementBackgroundGUIInternal;
                    this._instance.drawElementCallback -= this.OnElementGUIInternal;
                    this._instance.drawFooterCallback -= this.OnFooterGUI;
                    this._instance.drawHeaderCallback -= this.OnHeaderGUI;
                    this._instance.drawNoneElementCallback -= this.OnNoneElementGUI;
                    this._instance.elementHeightCallback -= this.OnElementHeightInternal;

                    this._instance.onAddCallback -= this.OnAddElementInternal;
                    this._instance.onAddDropdownCallback -= this.OnAddDropdownInternal;
                    this._instance.onCanAddCallback -= this.OnCanAddInternal;
                    this._instance.onCanRemoveCallback -= this.OnCanRemoveInternal;
                    this._instance.onChangedCallback -= this.OnChangedElementInternal;
                    this._instance.onMouseUpCallback -= this.OnMouseUpElementInternal;
                    this._instance.onRemoveCallback -= this.OnRemoveElementInternal;
                    this._instance.onReorderCallbackWithDetails -= this.OnReorderElementInternal;
                    this._instance.onSelectCallback -= this.OnSelectElementInternal;
                }
                this._instance = null;

                this._disposed = true;
            }
        }
        #endregion

        #region Methods & Functions
        void ApplyInternalSetup()
        {
            this._defaultBehaviours = this._defaultBehaviours ?? new UnityReorderableList.Defaults();

            this._instance.headerHeight = this.OnHeaderHeight();
            this._instance.footerHeight = this.OnFooterHeight();
        }

        /// <summary>
        /// Render the Reorderable List using the inspector layout.
        /// </summary>
        public void DoLayoutList()
        {
            this.ApplyInternalSetup();
            this._instance.DoLayoutList();
        }

        /// <summary>
        /// Render the Reorderable List.
        /// </summary>
        /// <param name="rect">Layout rect.</param>
        public void DoList(Rect rect)
        {
            this.ApplyInternalSetup();
            this._instance.DoList(rect);
        }

        /// <summary>
        /// Add new element to list.
        /// </summary>
        /// <returns>Return the reference to the new element.</returns>
        public SerializedProperty AddNewElement()
        {
            int index = this.Count;
            this._defaultBehaviours.DoAddButton(this._instance);
            this._instance.index = index;
            return this[index];
        }

        /// <summary>
        /// Remove the selected element.
        /// </summary>
        public void RemoveSelectedElement()
        {
            this._defaultBehaviours.DoRemoveButton(this._instance);
        }
        #endregion

        #region Event listeners
        /// <summary>
        /// Override this event listener to customize the header content.
        /// </summary>
        /// <param name="rect">Layout header rect.</param>
        public virtual void OnHeaderGUI(Rect rect)
        {

        }

        /// <summary>
        /// Override this event listener to customize the height of header layout.
        /// </summary>
        /// <returns>Return the height of header layout. By default return the single line + standard vertical space.</returns>
        public virtual float OnHeaderHeight()
        {
            return this.DisplayHeader ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : ReorderableListBase.HEADER_NONE_HEIGHT;
        }

        /// <summary>
        /// Override this event listener to customize the footer content. Leave empty for non draw footer.
        /// </summary>
        /// <param name="rect">Layout footer rect.</param>
        /// <remarks>If the footer is drawed, the add and remove buttons will not showed.</remarks>
        public virtual void OnFooterGUI(Rect rect)
        {
            this._defaultBehaviours.DrawFooter(rect, this._instance);
        }

        /// <summary>
        /// Override this event listener to customize the height of footer layout.
        /// </summary>
        /// <returns>Return the height of footer layout.</returns>
        /// <remarks>If the footer is drawed, the add and remove buttons will not showed.</remarks>
        public virtual float OnFooterHeight()
        {
            return this._defaultBehaviours.footerBackground.fixedHeight;
        }

        /// <summary>
        /// Override this event listener to customize the appearance of empty list element.
        /// </summary>
        /// <param name="rect">Layout rect.</param>
        public virtual void OnNoneElementGUI(Rect rect)
        {
            this._defaultBehaviours.DrawNoneElement(rect, this.IsDraggable);
        }

        /// <summary>
        /// Override this event listener to customize the appearance and content of the element list. You can define a style and wich content must be render per element. By default the element render all Serialized Property child elements.
        /// </summary>
        /// <param name="rect">Element layout rect.</param>
        /// <param name="element">Reference to serialized element.</param>
        /// <param name="index">Index of the element in list.</param>
        /// <param name="isActive">Return if the element is active.</param>
        /// <param name="isFocused">Return if the element has the focus.</param>
        public virtual void OnElementGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
        {
            int indent = (element.hasChildren && element.propertyType != SerializedPropertyType.String) ? 1 : 0;

            EditorGUI.indentLevel += indent;
            EditorGUI.PropertyField(rect, element, true);
            EditorGUI.indentLevel -= indent;
        }

        /// <summary>
        /// Override this event listener to customize the appearance of the element background. You can define a style per element.
        /// </summary>
        /// <param name="rect">Element layout rect.</param>
        /// <param name="element">Reference to serialized element.</param>
        /// <param name="index">Index of element in list.</param>
        /// <param name="isActive">Return if the element is active.</param>
        /// <param name="isFocused">Return if the element has the focus.</param>
        public virtual void OnElementBackgroundGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
        {
            this._defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, this.IsDraggable);
        }

        /// <summary>
        /// Override this event listener to customize the height of element layout. By default is the property height, include all unfolded child elements.
        /// </summary>
        /// <param name="element">Reference to serialized element.</param>
        /// <param name="index">Index of element in list.</param>
        /// <returns></returns>
        public virtual float OnElementHeight(SerializedProperty element, int index)
        {
            return EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.standardVerticalSpacing;
        }

        /// <summary>
        /// Override this event listener to customize the behaviour when the list determine when can add a new element (e.g. setup a limit to max elements in the list).
        /// </summary>
        /// <returns>Return true when is allowed to add a new element. By default always return true.</returns>
        public virtual bool OnCanAdd()
        {
            return true;
        }

        /// <summary>
        /// Override this event listener to customize the behaviour when the list determine when can remove an existent element (e.g. setup a minimal limit of elements in the list).
        /// </summary>
        /// <returns>Return true when is allowed to remove the selected element. By default always return true if the list content any element.</returns>
        public virtual bool OnCanRemove()
        {
            return this.Count > 0;
        }

        /// <summary>
        /// Override this event listener to customize the behaviour when the list trying to add a new element. Use this to implement additional behaviours when adding a new element (e.g. a custom wizzard dialog to fill the data).
        /// </summary>
        /// <remarks>This event listener is enable only when the <see cref="DisplayAddButton"/> behaviour is <see cref="ReorderableListAddButtonType.Default"/>.</remarks>
        public virtual void OnAddElement()
        {
            this.AddNewElement();
        }

        /// <summary>
        /// Override this event listener to customize the behaviour when the list trying to remove an existent element. Use this to implement additional behaviours when removing an existent element (e.g. a confirmation dialog).
        /// </summary>
        /// <param name="element">Reference to serialized element to remove.</param>
        public virtual void OnRemoveElement(SerializedProperty element)
        {
            this.RemoveSelectedElement();
        }

        /// <summary>
        /// Override this event listener to customize the behaviour when the list trying to add a new element. Similar to <see cref="OnAddElement"/>, this event listener allow to setup a dropdown menu to define different options to setup a new element. Use with <see cref="OnDropdownOptionClick(object)"/> virtual method to manage the selections of each selectable element in custom dropdown.
        /// </summary>
        /// <param name="buttonRect">Add button area.</param>
        /// <remarks>This event listener is enable only when the <see cref="DisplayAddButton"/> behaviour is <see cref="ReorderableListAddButtonType.Dropdown"/>.</remarks>
        public virtual void OnAddDropdown(Rect buttonRect)
        {

        }

        /// <summary>
        /// Override this event listener to customize the response behaviour of <see cref="OnAddDropdown(Rect)"/> event listener when click on any selectable option. Use this to assing a callback when create new item on custom dropdown.
        /// </summary>
        /// <param name="selection">The user defined data passed on callback.</param>
        /// <remarks>You can define your own method to used on the callback. The only requierment is has an one parameter of type <see cref="object"/>.</remarks>
        public virtual void OnDropdownOptionClick(object selection)
        {

        }

        /// <summary>
        /// Override this event listener to add response when an element is selected.
        /// </summary>
        /// <param name="element">Reference to serialized selected element.</param>
        public virtual void OnSelectElement(SerializedProperty element)
        {

        }

        /// <summary>
        /// Override this event listener to customize the response when the user release the mouse click over an element.
        /// </summary>
        /// <param name="element">Reference to serialized element where the mouse up event is ocurred.</param>
        public virtual void OnMouseUpElement(SerializedProperty element)
        {

        }

        /// <summary>
        /// Override this event listener to notified when an element reference is changed or reordered. This event not raised when changed any data of the element.
        /// </summary>
        /// <param name="element">Reference to serialized element was changed.</param>
        public virtual void OnChangedElement(SerializedProperty element)
        {

        }

        /// <summary>
        /// Override this event listener to notified when an element is reordered in the list by mouse dragging.
        /// </summary>
        /// <param name="element">Reference to serialized element reordered.</param>
        /// <param name="oldIndex">Return the old index in the list.</param>
        /// <param name="newIndex">Return the new index in the list.</param>
        public virtual void OnReorderElement(SerializedProperty element, int oldIndex, int newIndex)
        {

        }

        #region Internal Unity ReorderableList event listeners
        void OnElementGUIInternal(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index >= 0 && this.Count > 0)
            {
                this.OnElementGUI(rect, this[index], index, isActive, isFocused);
            }
        }

        void OnElementBackgroundGUIInternal(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index >= 0)
            {
                this.OnElementBackgroundGUI(rect, this[index], index, isActive, isFocused);
            }
        }

        float OnElementHeightInternal(int index)
        {
            return this.OnElementHeight(this[index], index);
        }

        bool OnCanAddInternal(UnityReorderableList list)
        {
            return this.OnCanAdd();
        }

        bool OnCanRemoveInternal(UnityReorderableList list)
        {
            return this.OnCanRemove();
        }

        void OnAddElementInternal(UnityReorderableList list)
        {
            this._instance = list;
            this.OnAddElement();
        }

        void OnRemoveElementInternal(UnityReorderableList list)
        {
            this._instance = list;
            this.OnRemoveElement(this[list.index]);
        }

        void OnAddDropdownInternal(Rect buttonRect, UnityReorderableList list)
        {
            this._instance = list;
            this.OnAddDropdown(buttonRect);
        }

        void OnSelectElementInternal(UnityReorderableList list)
        {
            this._instance = list;
            if (list.index >= 0)
            {
                this.OnSelectElement(this[list.index]);
            }
        }

        void OnMouseUpElementInternal(UnityReorderableList list)
        {
            this._instance = list;
            if (list.index >= 0)
            {
                this.OnChangedElement(this[list.index]);
            }
        }

        void OnChangedElementInternal(UnityReorderableList list)
        {
            this._instance = list;
            if (list.index >= 0)
            {
                this.OnChangedElement(this[list.index]);
            }
        }

        void OnReorderElementInternal(UnityReorderableList list, int oldIndex, int newIndex)
        {
            this._instance = list;
            this.OnReorderElement(this[newIndex], oldIndex, newIndex);
        }
        #endregion
        #endregion
    }
}
