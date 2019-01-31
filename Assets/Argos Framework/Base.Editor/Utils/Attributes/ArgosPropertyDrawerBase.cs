using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    public abstract class ArgosPropertyDrawerBase : PropertyDrawer
    {
        #region Constants
        const string ERROR_MESSAGE = "<color=red>This variable type is not supported</color>"; 
        #endregion

        #region Internal vars
        GUIStyle _errorMessageStyle;
        #endregion

        #region Methods & Functions
        public abstract bool CheckPropertyType();

        public void PrintErrorMessage(Rect position, GUIContent label)
        {
            if (this._errorMessageStyle == null)
            {
                this._errorMessageStyle = new GUIStyle();
                this._errorMessageStyle.alignment = TextAnchor.MiddleRight;
                this._errorMessageStyle.richText = true;
            }

            EditorGUI.LabelField(position, label, new GUIContent(ArgosPropertyDrawerBase.ERROR_MESSAGE), this._errorMessageStyle);
        }
        #endregion
    }
}