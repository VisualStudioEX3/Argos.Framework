using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;
using Argos.Framework.IMGUI;

public class ReorderableListTest : MonoBehaviour
{
    [System.Serializable]
    public struct CustomData
    {
        public string name;
        [Range(0, 100)]
        public int age;
        public KeyCode key;
    }

    public List<CustomData> _list;

    private void Awake()
    {
        this._list = new List<CustomData>();
    }
}

public class CustomReorderableList : ReorderableListBase
{
    public CustomReorderableList(SerializedProperty elements) : base(elements, true, true, ReorderableListAddButtonType.Dropdown)
    {
        this.ShowDefaultBackground = false;
    }

    public override void OnHeaderGUI(Rect rect)
    {
        EditorGUI.LabelField(rect, "Custom Reorderable List", EditorStyles.boldLabel);
    }

    //public override float OnFooterHeight()
    //{
    //    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    //}

    //// FYI: Draw the footer hide the add and remove buttons. Maybe useful to draw custom buttons.
    //public override void OnFooterGUI(Rect rect)
    //{
    //    rect.width = 200f;
    //    EditorGUI.LabelField(rect, $"{this.Count} elements");
    //}

    public override void OnAddDropdown(Rect buttonRect)
    {
        Debug.Log("Add Dropdown event");

        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("Add Element 1"), false, this.OnDropdownOptionClick, new ReorderableListTest.CustomData() { name = "Element 1", age = 33, key = KeyCode.Space });
        menu.AddItem(new GUIContent("Add Element 2"), false, this.OnDropdownOptionClick, new ReorderableListTest.CustomData() { name = "Element 2", age = 15, key = KeyCode.Return });
        menu.AddItem(new GUIContent("Add Element 3"), false, this.OnDropdownOptionClick, new ReorderableListTest.CustomData() { name = "Element 3", age = 54, key = KeyCode.Escape });

        menu.ShowAsContext();
    }

    public override void OnDropdownOptionClick(object selection)
    {
        var elementData = (ReorderableListTest.CustomData)selection;

        SerializedProperty newElement = this.AddNewElement();
        newElement.FindPropertyRelative("name").stringValue = elementData.name;
        newElement.FindPropertyRelative("age").intValue = elementData.age;
        newElement.FindPropertyRelative("key").enumValueIndex = (int)elementData.key;

        newElement.serializedObject.ApplyModifiedProperties();
    }

    // Maybe remove this event. Only raised when the phisical element/reference is changed (not its content values), and only raised when the element is reorderered. The OnReorderElement works fine for this task.
    //public override void OnChangedElement(SerializedProperty element)
    //{
    //    Debug.Log($"On Changed Element event: {element.displayName}");
    //}

    public override void OnRemoveElement(SerializedProperty element)
    {
        Debug.Log($"On Remove Element event: {element.displayName}");
        base.OnRemoveElement(element);
    }

    public override void OnReorderElement(SerializedProperty element, int oldIndex, int newIndex)
    {
        Debug.Log($"On Reorder Element event: {element.displayName}, old index {oldIndex}, new index {newIndex}");
    }

    public override float OnElementHeight(SerializedProperty element, int index)
    {
        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3f;
    }

    public override void OnElementGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
    {
        //Debug.Log($"The element {element.displayName} with index {index} is focused {isFocused} or is active {isActive}");

        Rect nameRect = rect;
        nameRect.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(nameRect, element.FindPropertyRelative("name"));

        Rect ageRect = nameRect;
        ageRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(ageRect, element.FindPropertyRelative("age"));

        Rect keyRect = ageRect;
        keyRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(keyRect, element.FindPropertyRelative("key"));
    }

    public override void OnElementBackgroundGUI(Rect rect, SerializedProperty element, int index, bool isActive, bool isFocused)
    {
        EditorGUI.HelpBox(rect, string.Empty, MessageType.None);
    }
}

[CustomEditor(typeof(ReorderableListTest))]
public class ReorderableListTestEditor : Editor
{
    ReorderableList _defaultList;
    CustomReorderableList _list;

    private void OnEnable()
    {
        this._defaultList = new ReorderableList(this.serializedObject.FindProperty("_list"), "Default Reorderable List", false);

        this._list = new CustomReorderableList(this.serializedObject.FindProperty("_list"));
    }

    private void OnDisable()
    {
        this._list?.Dispose();
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        this.DrawDefaultInspectorWithoutScriptField();

        this._defaultList.DoLayoutList();
        this._list.DoLayoutList();

        this.serializedObject.ApplyModifiedProperties();
    }
}
