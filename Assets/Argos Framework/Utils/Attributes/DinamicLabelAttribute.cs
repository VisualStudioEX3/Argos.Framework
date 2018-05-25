using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a string variable in a script be a dinamic multi-line richtext label.
    /// </summary>
    public class DinamicLabelAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly bool MiniLabel;
        public readonly bool Selectable;
        #endregion

        #region MyRegion
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="miniLabel">Show as mini label style.</param>
        /// <param name="selectable">The content is selectable?</param>
        public DinamicLabelAttribute(bool miniLabel = false, bool selectable = false)
        {
            this.MiniLabel = miniLabel;
            this.Selectable = selectable;
        } 
        #endregion
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DinamicLabelAttribute))]
    public class DinamicLabelDrawer : PropertyDrawer
    {
        #region Internal vars
        DinamicLabelAttribute _attribute;
        GUIStyle _style;
        #endregion

        #region Methods & Functions
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            this._attribute = (DinamicLabelAttribute)attribute;
            this._style = this._attribute.MiniLabel ? EditorStyles.wordWrappedMiniLabel : EditorStyles.wordWrappedLabel;
            this._style.richText = true;

            return this._style.CalcHeight(new GUIContent(property.stringValue), EditorGUIUtility.currentViewWidth);
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (this._attribute.Selectable)
            {
                EditorGUI.SelectableLabel(position, property.stringValue, this._style);
            }
            else
            { 
                EditorGUI.LabelField(position, property.stringValue, this._style); 
            }
        }
        #endregion
    }
#endif 
}