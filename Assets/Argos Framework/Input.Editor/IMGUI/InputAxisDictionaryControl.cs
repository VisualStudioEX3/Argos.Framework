using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework.IMGUI;

namespace Argos.Framework.Input
{
    public sealed class InputAxisDictionaryControl : ReorderableDictionaryBase
    {
        #region Constants
        const string NEW_ITEM_LABEL = "New Input Axis name";
        const string PROPERTY_ITEM_KEY = "key";
        const string PROPERTY_ITEM_VALUE = "value";

        readonly static Color COLOR_DARK = new Color(0.1f, 0.1f, 0.1f, 1f);
        readonly static Color COLOR_LIGHT = new Color(0.4f, 0.4f, 0.4f, 1f);
        #endregion

        #region Structs
        struct ElementGUITool
        {
            #region Internal vars
            int _lastElement; 
            #endregion

            #region Public vars
            public SerializedProperty keyProperty;
            public SerializedProperty valueProperty;

            public float keyHeight;
            public float valueHeight;

            public float totalHeight;
            #endregion

            #region Methods & Functions
            public void Update(SerializedProperty element, bool isLastElement)
            {
                if (this._lastElement != element.GetHashCode())
                {
                    this._lastElement = element.GetHashCode();

                    this.keyProperty = element.FindPropertyRelative(InputAxisDictionaryControl.PROPERTY_ITEM_KEY);
                    this.valueProperty = element.FindPropertyRelative(InputAxisDictionaryControl.PROPERTY_ITEM_VALUE);

                    this.keyHeight = EditorGUI.GetPropertyHeight(this.keyProperty);
                    this.valueHeight = EditorGUI.GetPropertyHeight(this.valueProperty);

                    this.totalHeight = this.keyHeight + this.valueHeight + (isLastElement ? 0f : EditorGUIUtility.singleLineHeight);
                }
            }

            public Rect DrawKeyField(Rect rect)
            {
                rect.y++;
                rect.height = this.keyHeight;

                EditorGUI.DelayedTextField(rect, this.keyProperty, new GUIContent("Name"));

                rect.y += this.keyHeight;

                return rect;
            }

            public Rect DrawValueField(Rect rect)
            {
                rect.height = this.valueHeight;

                EditorGUI.PropertyField(rect, this.valueProperty, true);

                rect.y += this.valueHeight;

                return rect;
            }
            #endregion
        }
        #endregion

        #region Internal vars
        ElementGUITool _elementGUI; 
        #endregion

        #region Constructors
        public InputAxisDictionaryControl(SerializedProperty elements) :
            base(elements, true, true, true, true, InputAxisDictionaryControl.NEW_ITEM_LABEL)
        {
        }
        #endregion

        #region Methods & Functions
        void DrawSeparator(Rect rect)
        {
            bool invert = true;

            rect.y += 6f;
            rect.height = 1f;
            EditorGUI.DrawRect(rect, invert ? InputAxisDictionaryControl.COLOR_DARK : InputAxisDictionaryControl.COLOR_LIGHT);
            rect.y++;
            EditorGUI.DrawRect(rect, !invert ? InputAxisDictionaryControl.COLOR_DARK : InputAxisDictionaryControl.COLOR_LIGHT);
        } 
        #endregion

        #region Event listeners
        public override void OnHeaderGUI(Rect rect)
        {
            EditorGUI.LabelField(rect, "Axes");
        }

        public override float OnElementHeight(SerializedProperty element, int index)
        {
            this._elementGUI.Update(element, index == this.Count - 1);
            return this._elementGUI.totalHeight;
        }

        public override void OnElementGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
        {
            bool isLastElement = index == this.Count - 1;

            this._elementGUI.Update(element, isLastElement);

            rect = this._elementGUI.DrawKeyField(rect);
            rect = this._elementGUI.DrawValueField(rect);

            if (!isLastElement && this._elementGUI.valueProperty.isExpanded)
            {
                this.DrawSeparator(rect); 
            }

            this.CheckElementKeyValue(element, index);
        }
        #endregion
    }
}