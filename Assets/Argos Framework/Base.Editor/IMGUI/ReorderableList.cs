using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityReorderableList = UnityEditorInternal.ReorderableList;

namespace Argos.Framework.IMGUI
{
    public abstract class ReorderableList : IDisposable
    {
        #region Constants
        const float HEADER_NONE_HEIGHT = 3f;
        #endregion

        #region Enums
        public enum ReorderableListAddButtonType
        {
            None,
            Default,
            Dropdown
        } 
        #endregion

        #region Internal vars
        UnityReorderableList _instance;
        UnityReorderableList.Defaults _defaultBehaviours;
        #endregion

        #region Properties
        public SerializedObject SerializedObject { get { return this.Elements.serializedObject; } }
        public SerializedProperty Elements { get { return this._instance.serializedProperty; } }
        public SerializedProperty this[int index] { get { return this.Elements.GetArrayElementAtIndex(index); } }
        public int Count { get { return this._instance.count; } }

        public bool IsDraggable { get; private set; }
        public bool DisplayHeader { get; set; }
        public ReorderableListAddButtonType DisplayAddButton { get; private set; }
        public bool DisplayRemoveButton { get; private set; }

        public bool ShowDefaultBackground { get { return this._instance.showDefaultBackground; } set { this._instance.showDefaultBackground = value; } }
        #endregion

        #region Constructors & Destructors
        public ReorderableList(SerializedProperty elements, bool isDraggable = true, bool displayHeader = false, ReorderableListAddButtonType displayAddButton = ReorderableListAddButtonType.Default, bool displayRemoveButton = true)
        {
            this._instance = new UnityReorderableList(elements.serializedObject, elements, isDraggable, displayHeader, displayAddButton != ReorderableListAddButtonType.None, displayRemoveButton);
            {
                this._instance.drawElementBackgroundCallback += this.OnElementBackgroundGUI;
                this._instance.drawElementCallback += this.OnElementGUI;
                this._instance.drawFooterCallback += this.OnFooterGUI;
                this._instance.drawHeaderCallback += this.OnHeaderGUI;
                this._instance.drawNoneElementCallback += this.OnNoneElementGUI;
                this._instance.elementHeightCallback += this.OnElementHeight;

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

        public void Dispose()
        {
            this._instance.drawElementBackgroundCallback -= this.OnElementBackgroundGUI;
            this._instance.drawElementCallback -= this.OnElementGUI;
            this._instance.drawFooterCallback -= this.OnFooterGUI;
            this._instance.drawHeaderCallback -= this.OnHeaderGUI;
            this._instance.drawNoneElementCallback -= this.OnNoneElementGUI;
            this._instance.elementHeightCallback -= this.OnElementHeight;

            this._instance.onAddCallback -= this.OnAddElementInternal;
            this._instance.onAddDropdownCallback -= this.OnAddDropdownInternal;
            this._instance.onCanAddCallback -= this.OnCanAddInternal;
            this._instance.onCanRemoveCallback -= this.OnCanRemoveInternal;
            this._instance.onChangedCallback -= this.OnChangedElementInternal;
            this._instance.onMouseUpCallback -= this.OnMouseUpElementInternal;
            this._instance.onRemoveCallback -= this.OnRemoveElementInternal;
            this._instance.onReorderCallbackWithDetails -= this.OnReorderElementInternal;
            this._instance.onSelectCallback -= this.OnSelectElementInternal;

            this._instance = null;
        } 
        #endregion

        #region Methods & Functions
        void ApplyInternalSetup()
        {
            if (this._defaultBehaviours == null)
            {
                this._defaultBehaviours = new UnityReorderableList.Defaults(); 
            }

            this._instance.headerHeight = this.OnHeaderHeight();
            this._instance.footerHeight = this.OnFooterHeight();
        }

        public void DoLayoutList()
        {
            this.ApplyInternalSetup();
            this._instance.DoLayoutList();
        }

        public void DoList(Rect rect)
        {
            this.ApplyInternalSetup();
            this._instance.DoList(rect);
        }

        public void AddNewElement()
        {
            this._defaultBehaviours.DoAddButton(this._instance);
        }

        public SerializedProperty AddNewElementAndReturnIt()
        {
            int index = this.Count;
            this.AddNewElement();
            this._instance.index = index;
            return this[index];
        }

        public void RemoveSelectedElement()
        {
            this._defaultBehaviours.DoRemoveButton(this._instance);
        }
        #endregion

        #region Event listeners
        public virtual void OnHeaderGUI(Rect rect)
        {

        }

        public virtual float OnHeaderHeight()
        {
            return this.DisplayHeader ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing : ReorderableList.HEADER_NONE_HEIGHT;
        }

        public virtual void OnFooterGUI(Rect rect)
        {
            this._defaultBehaviours.DrawFooter(rect, this._instance);
        }

        public virtual float OnFooterHeight()
        {
            return this._defaultBehaviours.footerBackground.fixedHeight;
        }

        public virtual void OnNoneElementGUI(Rect rect)
        {
            this._defaultBehaviours.DrawNoneElement(rect, this.IsDraggable);
        }

        public virtual void OnElementGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.PropertyField(rect, this[index]);
        }

        public virtual void OnElementBackgroundGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            this._defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, this.IsDraggable);
        }

        public virtual float OnElementHeight(int index)
        {
            return EditorGUI.GetPropertyHeight(this[index]);
        }

        public virtual bool OnCanAdd()
        {
            return true;
        }

        public virtual bool OnCanRemove()
        {
            return this.Count > 0;
        }

        public virtual void OnAddElement(SerializedProperty element)
        {
            this.AddNewElement();
        }

        public virtual void OnRemoveElement()
        {
            this.RemoveSelectedElement();
        }

        public virtual void OnAddDropdown(Rect buttonRect)
        {

        }

        public virtual void OnSelectElement(SerializedProperty element)
        {

        }

        public virtual void OnMouseUpElement(SerializedProperty element)
        {

        }

        public virtual void OnChangedElement(SerializedProperty element)
        {

        }

        public virtual void OnReorderElement(SerializedProperty element, int oldIndex, int newIndex)
        {
            
        }

        #region Internal Unity ReorderableList event listeners
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
            this.OnAddElement(this[this.Count - 1]);
        }

        void OnRemoveElementInternal(UnityReorderableList list)
        {
            this._instance = list;
            this.OnRemoveElement();
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
