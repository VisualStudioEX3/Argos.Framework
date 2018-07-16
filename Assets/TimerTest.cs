using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Argos.Framework;

public class TimerTest : MonoBehaviour
{
    public Timer TimerScaled, TimerUnScaled, TimerEditor;

    public void InitTimers()
    {
        this.TimerScaled = new Timer(Timer.TimerModes.ScaledTime, false);
        this.TimerUnScaled = new Timer(Timer.TimerModes.UnScaledTime, false);
        this.TimerEditor = new Timer(Timer.TimerModes.EditorMode, false);
    }

    void Start()
    {
        this.InitTimers();
    }
}

[CustomEditor(typeof(TimerTest))]
public class TimerTestEditor : Editor
{
    TimerTest _target;

    private void OnEnable()
    {
        this._target = (TimerTest)this.target;
        this._target.InitTimers();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Play"))
        {
            this._target.TimerScaled.Start();
            this._target.TimerUnScaled.Start();
            this._target.TimerEditor.Start();
        }
        else if (GUILayout.Button("Pause"))
        {
            this._target.TimerScaled.Pause();
            this._target.TimerUnScaled.Pause();
            this._target.TimerEditor.Pause();
        }
        else if (GUILayout.Button("Resume"))
        {
            this._target.TimerScaled.Resume();
            this._target.TimerUnScaled.Resume();
            this._target.TimerEditor.Resume();
        }
        else if (GUILayout.Button("Stop"))
        {
            this._target.TimerScaled.Stop();
            this._target.TimerUnScaled.Stop();
            this._target.TimerEditor.Stop();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField($"Timer scaled: {this._target.TimerScaled.Value}");
        EditorGUILayout.LabelField($"Timer unscaled: {this._target.TimerUnScaled.Value}");
        EditorGUILayout.LabelField($"Timer editor: {this._target.TimerEditor.Value}");

        this.Repaint();
    }
}