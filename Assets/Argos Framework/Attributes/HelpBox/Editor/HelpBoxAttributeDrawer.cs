using UnityEngine;
using UnityEditor;

namespace Argos.Framework
{
    /// <summary>
    /// Decorator drawer for HelpBox attribute
    /// </summary>
    /// <remarks>
    /// Modified from code at https://forum.unity.com/threads/helpattribute-allows-you-to-use-helpbox-in-the-unity-inspector-window.462768/#post-3014998
    /// </remarks>
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {

        public override float GetHeight()
        {
            var helpBoxAttribute = attribute as HelpBoxAttribute;
            if (helpBoxAttribute == null)
                return base.GetHeight();
            var helpBoxStyle = (GUI.skin != null) ? GUI.skin.GetStyle("helpbox") : null;
            if (helpBoxStyle == null)
                return base.GetHeight();

            var content = new GUIContent(helpBoxAttribute.text);
            float minW, maxW;

            helpBoxStyle.CalcMinMaxWidth(content, out minW, out maxW);
            float width = EditorGUIUtility.currentViewWidth - EditorGUI.indentLevel * 30f;
            if (helpBoxAttribute.messageType != HelpBoxMessageType.None)
                width *= 0.85f;
            float height = helpBoxStyle.CalcHeight(new GUIContent(helpBoxAttribute.text), width);
            return height + 4 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position)
        {
            var helpBoxAttribute = attribute as HelpBoxAttribute;
            if (helpBoxAttribute == null)
                return;
            position.height -= EditorGUIUtility.standardVerticalSpacing;
            position = EditorGUI.IndentedRect(position);
            EditorGUI.HelpBox(position, helpBoxAttribute.text, GetMessageType(helpBoxAttribute.messageType));
        }

        private MessageType GetMessageType(HelpBoxMessageType helpBoxMessageType)
        {
            switch (helpBoxMessageType)
            {
                default:
                case HelpBoxMessageType.None:
                    return MessageType.None;
                case HelpBoxMessageType.Info:
                    return MessageType.Info;
                case HelpBoxMessageType.Warning:
                    return MessageType.Warning;
                case HelpBoxMessageType.Error:
                    return MessageType.Error;
            }
        }
    }
}