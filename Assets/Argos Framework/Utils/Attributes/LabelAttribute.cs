using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework
{
    /// <summary>
    /// Use this PropertyAttribute to add a multi-label above some fields in the Inspector.
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
        #region Methods & Functions
        public override float GetHeight()
        {
            var label = (LabelAttribute)attribute;
            GUIStyle style = label.MiniLabel ? EditorStyles.wordWrappedMiniLabel : EditorStyles.wordWrappedLabel;

            return style.CalcHeight(new GUIContent(label.Text), EditorGUIUtility.currentViewWidth);
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position)
        {
            var label = (LabelAttribute)attribute;
            GUIStyle style = label.MiniLabel ? EditorStyles.wordWrappedMiniLabel : EditorStyles.wordWrappedLabel;

            if (label.Selectable)
            {
                EditorGUI.SelectableLabel(position, label.Text, style);
            }
            else
            { 
                EditorGUI.LabelField(position, label.Text, style); 
            }
        }
        #endregion
    }
#endif 
}