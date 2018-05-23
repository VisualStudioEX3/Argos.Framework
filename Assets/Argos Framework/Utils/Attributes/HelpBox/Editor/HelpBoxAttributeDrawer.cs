using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Decorator drawer for HelpBox attribute.
    /// </summary>
    /// <remarks>
    /// Based on code at https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/#post-3014998
    /// </remarks>
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {
        #region Constants
        const float MIN_HEIGHT_WITH_ICON = 40f;
        #endregion
        
        #region Methods & Functions
        public override float GetHeight()
        {
            var helpBox = (HelpBoxAttribute)attribute;
            var style = EditorStyles.helpBox;
            var content = new GUIContent(helpBox.text);

            float width = EditorGUIUtility.currentViewWidth - EditorGUI.indentLevel;
            float height = style.CalcHeight(content, width) + EditorGUIUtility.standardVerticalSpacing;

            return helpBox.messageType == HelpBoxMessageType.None ? height : Mathf.Max(HelpBoxAttributeDrawer.MIN_HEIGHT_WITH_ICON, height);
        }
        #endregion

        #region Events
        public override void OnGUI(Rect position)
        {
            var helpBox = (HelpBoxAttribute)attribute;

            position.height -= EditorGUIUtility.standardVerticalSpacing;
            position = EditorGUI.IndentedRect(position);

            EditorGUI.HelpBox(position, helpBox.text, (MessageType)helpBox.messageType);
        } 
        #endregion
    }
}