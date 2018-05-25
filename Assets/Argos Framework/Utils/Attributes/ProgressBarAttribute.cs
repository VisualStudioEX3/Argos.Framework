using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region UNITY_EDITOR
using UnityEditor;
#endregion

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a float variable in a script be a progressbar.
    /// </summary>
    public class ProgressBarAttribute : PropertyAttribute
    {
        #region Public vars
        public readonly string Message;
        public readonly bool ShowLabel;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message show into the progressbar.</param>
        /// <param name="showLabel">Show field prefix label.</param>
        public ProgressBarAttribute(string message = "", bool showLabel = false)
        {
            this.Message = message;
            this.ShowLabel = showLabel;
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarDrawer : PropertyDrawer
    {
        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var progressBarAttribute = (ProgressBarAttribute)attribute;
            Rect rect = progressBarAttribute.ShowLabel ? EditorGUI.PrefixLabel(position, label) : EditorGUI.IndentedRect(position);

            EditorGUI.ProgressBar(rect, property.floatValue, progressBarAttribute.Message);
        }
        #endregion
    }
#endif 
}