using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;
using Argos.Framework.IMGUI;

namespace Argos.Framework.Input
{
    public sealed class InputMapDictionaryControl : ReorderableDictionaryBase
    {
        #region Constants
        const float FIELD_WIDTH_MULTIPLIER = 0.4975f;
        const string NEW_ITEM_LABEL = "New Input Map name";
        const string PROPERTY_ITEM_KEY = "key";
        const string PROPERTY_ITEM_VALUE = "value";
        #endregion

        #region Constructors
        public InputMapDictionaryControl(SerializedProperty elements) :
            base(elements, true, false, true, true, InputMapDictionaryControl.NEW_ITEM_LABEL)
        {
        }
        #endregion

        #region Event listeners
        public override float OnElementHeight(SerializedProperty element, int index)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnElementGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
        {
            Rect nameFieldRect = rect;
            nameFieldRect.width *= InputMapDictionaryControl.FIELD_WIDTH_MULTIPLIER;
            nameFieldRect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.DelayedTextField(nameFieldRect, element.FindPropertyRelative(InputMapDictionaryControl.PROPERTY_ITEM_KEY), GUIContent.none);

            Rect inputMapRect = nameFieldRect;
            inputMapRect.x = rect.xMax - nameFieldRect.width;
            EditorGUI.ObjectField(inputMapRect, element.FindPropertyRelative(InputMapDictionaryControl.PROPERTY_ITEM_VALUE), GUIContent.none);

            this.CheckElementKeyValue(element, index);
        }
        #endregion
    }
}