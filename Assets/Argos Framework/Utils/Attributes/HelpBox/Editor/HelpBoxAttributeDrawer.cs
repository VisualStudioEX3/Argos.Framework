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
            var helpBoxAttribute = (HelpBoxAttribute)attribute;
            var helpBoxStyle = GUI.skin.GetStyle("helpbox");
            var content = new GUIContent(helpBoxAttribute.text);

            float width = EditorGUIUtility.currentViewWidth - EditorGUI.indentLevel;
            float height = helpBoxStyle.CalcHeight(content, width) + (EditorGUIUtility.standardVerticalSpacing * 2f);

            if (helpBoxAttribute.messageType == HelpBoxMessageType.None)
            {
                return height;
            }
            else
            {
                return height < HelpBoxAttributeDrawer.MIN_HEIGHT_WITH_ICON ? HelpBoxAttributeDrawer.MIN_HEIGHT_WITH_ICON : height;
            }
        } 
        #endregion

        #region Events
        public override void OnGUI(Rect position)
        {
            var helpBoxAttribute = (HelpBoxAttribute)attribute;

            position.height -= EditorGUIUtility.standardVerticalSpacing;
            position = EditorGUI.IndentedRect(position);

            EditorGUI.HelpBox(position, helpBoxAttribute.text, (MessageType)helpBoxAttribute.messageType);
        } 
        #endregion
    }
}