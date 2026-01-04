using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : DecoratorDrawer
    {
        #region Internal vars
        LabelAttribute _attribute;
        GUIStyle _style;
        float _currentViewWidth;
        #endregion

        #region Methods & Functions
        public override float GetHeight()
        {
            this._attribute = (LabelAttribute)attribute;
            this._style = this._attribute.miniLabel ? EditorStyles.wordWrappedMiniLabel : EditorStyles.wordWrappedLabel;
            this._style.richText = true;

            return this._style.CalcHeight(new GUIContent(this._attribute.text), _currentViewWidth);
        }
        #endregion

        #region Event listeners
        public override void OnGUI(Rect position)
        {
            _currentViewWidth = EditorGUIUtility.currentViewWidth;

            if (this._attribute.selectable)
            {
                EditorGUI.SelectableLabel(position, this._attribute.text, this._style);
            }
            else
            {
                EditorGUI.LabelField(position, this._attribute.text, this._style);
            }
        }
        #endregion
    }
}