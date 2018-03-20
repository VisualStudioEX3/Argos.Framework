using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Input;

public class Test : MonoBehaviour
{
    void Start()
    {
        InputManager.Instance.GetInputMap("UI").GetAction("Submit").OnKeyDown += this.OnEventSubmitTest;
        InputManager.Instance.GetInputMap("UI").GetAction("Cancel").OnKeyDown += this.OnEventCancelTest;
    }

    void Update()
    {
        XInput.SetVibration(Mathf.Abs(InputManager.Instance.GetAxis("Player", "Movement").x), Mathf.Abs(InputManager.Instance.GetAxis("Player", "Movement").y));
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
