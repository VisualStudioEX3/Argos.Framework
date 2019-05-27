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


public class CustomReorderableList : ReorderableList
{
    public CustomReorderableList(SerializedProperty elements) : base(elements, true, true, ReorderableListAddButtonType.Dropdown)
    {
    }

    public override void OnHeaderGUI(Rect rect)
    {
        EditorGUI.LabelField(rect, "Custom Reorderable List", EditorStyles.boldLabel);
    }

    public override float OnFooterHeight()
    {
        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    // FYI: Draw the footer hide the add and remove buttons. Maybe useful to draw custom buttons.
    public override void OnFooterGUI(Rect rect)
    {
        rect.width = 200f;
        EditorGUI.LabelField(rect, $"{this.Count} elements");
    }

    public override void OnAddDropdown(Rect buttonRect)
    {
        Debug.Log("Add Dropdown event");
    }

    // Maybe remove this event. Only raised when the phisical element/reference is changed (not its content values), and only raised when the element is reorderered. The OnReorderElement works fine for this task.
    //public override void OnChangedElement(SerializedProperty element)
    //{
    //    Debug.Log($"On Changed Element event: {element.displayName}");
    //}

    public override void OnReorderElement(SerializedProperty element, int oldIndex, int newIndex)
    {
        Debug.Log($"On Reorder Element event: {element.displayName}, old index {oldIndex}, new index {newIndex}");
    }

    public override float OnElementHeight(int index)
    {
        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3f;
    }

    public override void OnElementGUI(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = this[index];

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
}

[CustomEditor(typeof(ReorderableListTest))]
public class ReorderableListTestEditor : Editor
{
    CustomReorderableList _list;

    private void OnEnable()
    {
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

        this._list.DoLayoutList();

        this.serializedObject.ApplyModifiedProperties();
    }
}
