using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.IMGUI
{
    /// <summary>
    /// Basic ReorderableList object with dictionary behaviour.
    /// </summary>
    /// <remarks>A basic implementation of Unity ReorderableList object with dictionary behaviour implemented: optional header caption, optional add and remove buttons, and default render serialized property element behaviour (draws the all child elements).</remarks>
    public sealed class ReorderableDictionary : ReorderableDictionaryBase, IDisposable
    {
        #region Internal vars
        string _headerCaption;
        bool _boldHeaderCaption;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="elements">Serialized Property reference to element list.</param>
        /// <param name="headerCaption">Optional header caption title. Leave empty for avoid draw header.</param>
        /// <param name="boldHeaderCaption">Draw the header caption with bold style?</param>
        /// <param name="isDraggable">Allow to reorder the elements by mouse dragging?</param>
        /// <param name="displayAddButton">Show the add element button?</param>
        /// <param name="displayRemoveButton">Show the remove element button?</param>
        public ReorderableDictionary(SerializedProperty elements, string headerCaption = "", bool boldHeaderCaption = false, bool isDraggable = true, bool displayAddButton = true, bool displayRemoveButton = true) :
            base(elements, isDraggable, !string.IsNullOrEmpty(headerCaption), displayAddButton, displayRemoveButton)
        {
            this._headerCaption = headerCaption;
            this._boldHeaderCaption = boldHeaderCaption;
        }
        #endregion

        #region Event listeners
        public sealed override void OnHeaderGUI(Rect rect)
        {
            EditorGUI.LabelField(rect, this._headerCaption, this._boldHeaderCaption ? EditorStyles.boldLabel : EditorStyles.label);
        }
        #endregion
    }
}