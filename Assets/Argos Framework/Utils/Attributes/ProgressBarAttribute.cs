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
        public readonly string Label;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label show into the progressbar.</param>
        public ProgressBarAttribute(string label = "")
        {
            this.Label = label;
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
            EditorGUI.ProgressBar(EditorGUI.IndentedRect(position), property.floatValue, (attribute as ProgressBarAttribute).Label);
        }
        #endregion
    }
#endif 
}