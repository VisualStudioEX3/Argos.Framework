using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.IMGUI
{
    /// <summary>
    /// Popup Window to ease create modal input box window to enter values.
    /// </summary>
    /// <remarks>Only support the following types: int, float and string. If you try to initialize with other type, the constructor throw an exception.</remarks>
    public sealed class InputPopupWindow<T> : PopupWindowContent
    {
        #region Constants
        const string ACCEPT_BUTTON_CAPTION = "Save";
        #endregion

        #region Internal vars
        Rect _activatorRect;
        string _label;
        Action<T> _onAcceptCallback;
        string _acceptButtonCaption;
        T _value;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Field caption label.</param>
        /// <param name="onAcceptCallback">Event to receive the value typed when press the accept button. Not raised when the popup window closes when lost focus.</param>
        public InputPopupWindow(string label, Action<T> onAcceptCallback) : this(label, onAcceptCallback, string.Empty)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Field caption label.</param>
        /// <param name="onAcceptCallback">Event to receive the value typed when press the accept button. Not raised when the popup window closes when lost focus.</param>
        /// <param name="acceptButtonCaption">Custom accept button label caption. Left empty to default caption value, "Save".</param>
        public InputPopupWindow(string label, Action<T> onAcceptCallback, string acceptButtonCaption)
        {
            if (typeof(T) == typeof(string) || typeof(T) == typeof(int) || typeof(T) == typeof(float))
            {
                this._label = label;
                this._acceptButtonCaption = !string.IsNullOrEmpty(acceptButtonCaption) ? acceptButtonCaption : InputPopupWindow<T>.ACCEPT_BUTTON_CAPTION;
                this._onAcceptCallback = onAcceptCallback;
            }
            else
            {
                throw new InvalidCastException("InputPopupWindow<T>: Error to create instance. The generic type passed is not supported. Only support the following types: int, float and string.");
            }
        }
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Shows the popup window.
        /// </summary>
        /// <param name="activatorRect">The button rect that invokes the popup. Use for the popup window location.</param>
        public void Show(Rect activatorRect)
        {
            this.Show(activatorRect, Vector2.zero);
        }

        /// <summary>
        /// Shows the popup window.
        /// </summary>
        /// <param name="activatorRect">The button rect that invokes the popup. Use for the popup window location.</param>
        /// <param name="offset">Popup window offset location.</param>
        public void Show(Rect activatorRect, Vector2 offset)
        {
            this._value = default(T);
            activatorRect.position += offset;
            PopupWindow.Show(activatorRect, this);
        } 
        #endregion

        #region Event listeners
        public sealed override Vector2 GetWindowSize()
        {
            return new Vector2(400f, 48f);
        }

        public sealed override void OnGUI(Rect rect)
        {
            EditorGUILayout.GetControlRect(false, 1f); // Fix layout position of rest of controls.

            if (typeof(T) == typeof(int))
            {
                this._value = (T)(object)EditorGUILayout.IntField(this._label, (int)(object)this._value);
            }
            else if (typeof(T) == typeof(float))
            {
                this._value = (T)(object)EditorGUILayout.FloatField(this._label, (float)(object)this._value);
            }
            else
            {
                this._value = (T)(object)EditorGUILayout.TextField(this._label, (string)(object)this._value);
            }

            if (GUILayout.Button(this._acceptButtonCaption))
            {
                this._onAcceptCallback?.Invoke(this._value);
                this.editorWindow.Close();
            }
        }
        #endregion
    }
}