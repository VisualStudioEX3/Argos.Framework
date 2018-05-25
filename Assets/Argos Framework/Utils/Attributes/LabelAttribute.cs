using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework
{
    /// <summary>
    /// Use this PropertyAttribute to add a multi-line richtext label above some fields in the Inspector.
    /// </summary>
    public class LabelAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly string Text;
        public readonly bool MiniLabel;
        public readonly bool Selectable;
        #endregion

        #region MyRegion
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="miniLabel">Show as mini label style.</param>
        /// <param name="selectable">The content is selectable?</param>
        public LabelAttribute(string text, bool miniLabel = false, bool selectable = false)
        {
            this.Text = text;
            this.MiniLabel = miniLabel;
            this.Selectable = selectable;
        } 
        #endregion
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : DecoratorDrawer
    {
        #region Internal vars
        LabelAttribute _attribute;
        GUIStyle _style; 
        #endregion

        #region Methods & Functions
        public override float GetHeight()
        {
            this._attribute = (LabelAttribute)attribute;
            this._style = this._attribute.MiniLabel ? EditorStyles.wordWrappedMiniLabel : EditorStyles.wordWrappedLabel;
            this._style.richText = true;

            return this._style.CalcHeight(new GUIContent(this._attribute.Text), EditorGUIUtility.currentViewWidth);
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position)
        {
            if (this._attribute.Selectable)
            {
                EditorGUI.SelectableLabel(position, this._attribute.Text, this._style);
            }
            else
            { 
                EditorGUI.LabelField(position, this._attribute.Text, this._style); 
            }
        }
        #endregion
    }
#endif 
}