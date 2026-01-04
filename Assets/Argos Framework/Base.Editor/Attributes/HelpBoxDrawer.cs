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
    public class HelpBoxDrawer : DecoratorDrawer
    {
        #region Constants
        const float MIN_HEIGHT_WITH_ICON = 40f;
        #endregion

        #region Internal vars
        float _currentViewWidth;
        #endregion

        #region Methods & Functions
        public override float GetHeight()
        {
            var helpBox = (HelpBoxAttribute)attribute;

            float width = _currentViewWidth - EditorGUI.indentLevel;
            float height = EditorStyles.helpBox.CalcHeight(new GUIContent(helpBox.text), width) + EditorGUIUtility.standardVerticalSpacing;

            return helpBox.messageType == HelpBoxMessageType.None ? height : Mathf.Max(HelpBoxDrawer.MIN_HEIGHT_WITH_ICON, height);
        }
        #endregion

        #region Event listeners
        public override void OnGUI(Rect position)
        {
            _currentViewWidth = EditorGUIUtility.currentViewWidth;

            var helpBox = (HelpBoxAttribute)attribute;

            position.height -= EditorGUIUtility.standardVerticalSpacing;
            position = EditorGUI.IndentedRect(position);

            EditorGUI.HelpBox(position, helpBox.text, (MessageType)helpBox.messageType);
        }
        #endregion
    }
}