using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;
using Argos.Framework.Input;
using Argos.Framework.Input.Extensions;

public class VibrationTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            StartCoroutine(this.VibrateCoroutine());
        }
    }

    IEnumerator VibrateCoroutine()
    {
        print("Vibration coroutine in play mode...");
        Timer timer = new Timer();

        while (timer.Value < 1f)
        {
            XInput.SetVibration(Vector2.right);
            yield return null;
        }

        XInput.SetVibration(Vector2.zero);
        print("Stop coroutine.");
    }
}

[CustomEditor(typeof(VibrationTest))]
public class VibrationTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Editor Coroutine"))
        {
            this.StartCoroutine(this.VibrateCoroutine());
        }
    }

    IEnumerator VibrateCoroutine()
    {
        Timer timer = new Timer(Timer.TimerMode.EditorMode);

        while (timer.Value < 1f)
        {
            XInput.SetVibration(Vector2.up);
            yield return null;
        }

        XInput.SetVibration(Vector2.zero);
    }
}
