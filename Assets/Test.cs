using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Input;
using Argos.Framework.Input.Extensions;

public class Test : MonoBehaviour
{
    [HelpBox("Argos \nHelpbox \ntest adsfasdfasdfasdf", HelpBoxMessageType.Warning)]
    [MinMaxSlider(0f, 5f)]
    public MinMaxSlider ActionRange;

    [VectorRename("L", "R")]
    public Vector2 RenameVector2;

    public Vector2 NormalVector2;

    [VectorRename("L", "R", "H")]
    public Vector3 RenameVector3;

    public Vector3 NormalVector3;

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
