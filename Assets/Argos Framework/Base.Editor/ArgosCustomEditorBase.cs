using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Argos.Framework.Helpers;

namespace Argos.Framework
{
    /// <summary>
    /// Customizable editor base class.
    /// </summary>
    /// <remarks>Implements custom events for render title, custom buttons or custom content in header inspector.</remarks>
    public abstract class ArgosCustomEditorBase : Editor
    {
        #region Structs
        struct ToolbarButton
        {
            public string Label;
            public Action Method;
        } 
        #endregion

        #region Internal vars
        List<ToolbarButton> _toolbar;
        #endregion

        #region Public vars
        /// <summary>
        /// Inspector title.
        /// </summary>
        public string HeaderTitle = "New editor base";

        /// <summary>
        /// Shows the title in inspector header?
        /// </summary>
        public bool ShowTitle = true;
        #endregion

        #region Properties
        /// <summary>
        /// Full accessible header area. Thats not include icon, asset name field, help, open and context menu buttons.
        /// </summary>
        public Rect HeaderCustomizableRect
        {
            get
            {
                return new Rect(44f, 24f, EditorGUIUtility.currentViewWidth - 94f, EditorGUIUtility.singleLineHeight);
            }
        }

        /// <summary>
        /// Title area.
        /// </summary>
        public Rect TitleRect
        {
            get
            {
                var rect = this.HeaderCustomizableRect;
                rect.width = EditorGUIUtility.labelWidth;
                return rect;
            }
        }

        /// <summary>
        /// Toolbar area.
        /// </summary>
        public Rect ToolbarRect
        {
            get
            {
                var rect = this.HeaderCustomizableRect;
                rect.xMin = rect.x + EditorGUIUtility.labelWidth + 5f;
                rect.yMin++;
                return rect;
            }
        }
        #endregion

        #region Event listeners
        /// <summary>
        /// Custom event for render any content in custom inspector header area.
        /// </summary>
        /// <param name="rect">Available area on inspector header.</param>
        public virtual void OnCustomHeaderGUI(Rect rect)
        {
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();

            this.DrawTitle();
            this.DrawToolbar();

            this.OnCustomHeaderGUI(this.HeaderCustomizableRect);
        }
        #endregion

        #region Methods & Functions
        void DrawTitle()
        {
            if (this.ShowTitle)
            {
                EditorGUI.LabelField(this.TitleRect, this.HeaderTitle, EditorStyles.miniLabel);
            }
        }

        void DrawToolbar()
        {
            if (this._toolbar != null && this._toolbar.Count > 0)
            {
                int index = GUI.Toolbar(this.ToolbarRect, -1, this._toolbar.Select(e => e.Label).ToArray(), EditorStyles.miniButton);

                if (index >= 0)
                {
                    this._toolbar[index].Method.Invoke();
                }
            }
        }

        /// <summary>
        /// Add button to inspector header toolbar.
        /// </summary>
        /// <param name="label">Button name.</param>
        /// <param name="method">Method associated.</param>
        /// <returns>The button index in toolbar.</returns>
        public int AddHeaderToolbarButton(string label, Action method)
        {
            if (this._toolbar == null)
            {
                this._toolbar = new List<ToolbarButton>();
            }
            this._toolbar.Add(new ToolbarButton() { Label = label, Method = method });

            return this._toolbar.Count - 1;
        }

        /// <summary>
        /// Edit the button label in header toolbar.
        /// </summary>
        /// <param name="buttonIndex">Button index of the existen button.</param>
        /// <param name="label">New label.</param>
        public void EditHeaderToolbarButtonLabel(int buttonIndex, string label)
        {
            if (Helpers.Math.IsClamped(buttonIndex, 0, this._toolbar.Count - 1))
            {
                var button = this._toolbar[buttonIndex];
                button.Label = label;
                this._toolbar[buttonIndex] = button; 
            }
            else
            {
                throw new IndexOutOfRangeException($"Button index {buttonIndex} is out of range.");
            }
        }
        #endregion
    }
}