using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework.IMGUI
{
    /// <summary>
    /// Popup Window for allow to type string, int or float values and received on event.
    /// </summary>
    public static class InputPopupWindow
    {
        #region Constants
        const string ACCEPT_BUTTON_CAPTION = "Save"; 
        #endregion

        #region Static members
        public static void ShowInputStringPopup(Rect activatorRect, string label, Action<string> onAcceptCallback, string acceptButtonCaption = InputPopupWindow.ACCEPT_BUTTON_CAPTION)
        {
            PopupWindow.Show(activatorRect, new InputPopupWindow<string>(label, onAcceptCallback, acceptButtonCaption));
        }

        public static void ShowInputIntPopup(Rect activatorRect, string label, Action<int> onAcceptCallback, string acceptButtonCaption = InputPopupWindow.ACCEPT_BUTTON_CAPTION)
        {
            PopupWindow.Show(activatorRect, new InputPopupWindow<int>(label, onAcceptCallback, acceptButtonCaption));
        }

        public static void ShowInputFloatPopup(Rect activatorRect, string label, Action<float> onAcceptCallback, string acceptButtonCaption = InputPopupWindow.ACCEPT_BUTTON_CAPTION)
        {
            PopupWindow.Show(activatorRect, new InputPopupWindow<float>(label, onAcceptCallback, acceptButtonCaption));
        }
        #endregion
    }

    public sealed class InputPopupWindow<T> : PopupWindowContent
    {
        #region Internal vars
        string _label;
        Action<T> _onAcceptCallback;
        string _acceptButtonCaption;
        T _value;
        #endregion

        #region Constructors
        public InputPopupWindow(string label, Action<T> onAcceptCallback, string acceptButtonCaption)
        {
            this._label = label;
            this._acceptButtonCaption = acceptButtonCaption;
            this._onAcceptCallback = onAcceptCallback;
        }
        #endregion

        #region Event listeners
        public override Vector2 GetWindowSize()
        {
            return new Vector2(400f, 48f);
        }

        public override void OnGUI(Rect rect)
        {
            Rect baseRect = rect;
            baseRect.x += 4f;
            baseRect.y += 5f;
            baseRect.width -= baseRect.x * 0.5f;

            Rect fieldRect = baseRect;
            fieldRect.height = EditorGUIUtility.singleLineHeight;

            if (typeof(T) == typeof(int))
            {
                this._value = (T)(object)EditorGUI.IntField(fieldRect, this._label, (int)(object)this._value);
            }
            else if (typeof(T) == typeof(float))
            {
                this._value = (T)(object)EditorGUI.FloatField(fieldRect, this._label, (float)(object)this._value);
            }
            else
            {
                this._value = (T)(object)EditorGUI.TextField(fieldRect, this._label, (string)(object)this._value);
            }

            Rect buttonRect = baseRect;
            buttonRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            buttonRect.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (GUI.Button(buttonRect, this._acceptButtonCaption))
            {
                this._onAcceptCallback?.Invoke(this._value);
                this.editorWindow.Close();
            }
        }
        #endregion
    }
}