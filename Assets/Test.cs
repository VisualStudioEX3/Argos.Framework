using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Input;
using Argos.Framework.Input.Extensions;
using System;

public class Test : MonoBehaviour
{
    [Serializable]
    public struct TestStruct
    {
        [HelpBox("Struct element helpbox", HelpBoxMessageType.Info)]
        public int Field1;

        [MinMaxSlider(0f, 5f)]
        public Vector2Int Field2;

        [ReadOnly]
        public float Field3;

        [Scene]
        public string Field4;

        [ProgressBar("ProgressBar field!")]
        public float Field5;
    }

    [HelpBox("Argos Helpbox\nThis is a large text to test the line wrap content in this helpbox for the lulz!", HelpBoxMessageType.Warning)]
    [MinMaxSlider(0f, 5f)]
    public Vector2 ActionRange;

    [Range(0f, 1f)]
    public float Testf;

    [Scene]
    public string SceneField;

    [ProgressBar("ProgressBar test!")]
    public float ProgressBar = 0.3f;

    [Tag]
    public string TagField;

    [Layer]
    public int LayerField;

    public LayerMask LayerMaskField;

    public TestStruct Test2;
    
    [Button]
    public void TestButton()
    {

    }

    [Button("Test Button with custom label")]
    public void TestButton2()
    {

    }

    [Button("Test Button with custom label and tooltip", "Tooltip message test")]
    public void TestButton3()
    {

    }

    void Start()
    {
        InputManager.Instance.GetInputMap("UI").GetAction("Submit").OnKeyDown += this.OnEventSubmitTest;
        InputManager.Instance.GetInputMap("UI").GetAction("Cancel").OnKeyDown += this.OnEventCancelTest;
    }

    void Update()
    {
        InputManager.Instance.SetGamepadVibration(InputManager.Instance.GetAxis("Player", "Movement"));
    }

    void OnEventSubmitTest()
    {
        print("Submit!");
    }

    void OnEventCancelTest()
    {
        print("Cancel!");
    }
}
