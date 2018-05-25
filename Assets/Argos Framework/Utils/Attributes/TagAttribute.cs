using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a string variable in a script be restricted to tag values.
    /// </summary>
    public class TagAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagDrawer : PropertyDrawer
    {
        #region Constants
        const string DEFAULT_TAG = "Untagged";
        #endregion

        #region Events
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TagField(position, label, string.IsNullOrEmpty(property.stringValue.Trim()) ? TagDrawer.DEFAULT_TAG : property.stringValue);
        }
        #endregion
    }
#endif 
}