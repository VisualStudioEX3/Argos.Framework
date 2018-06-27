using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Input;
using Argos.Framework.Utils;

public class VibrationPoolController : MonoBehaviour
{
    public GamepadVibrationEffectAsset Background;
    public GamepadVibrationEffectAsset[] Actions;

    public bool UseCurves;
    public AnimationCurve Left;
    public AnimationCurve Right;

    void Update()
    {
        if (InputManager.Instance.GetAction("UI", "Submit"))
        {
            StartCoroutine(this.VibrationTestCoroutine());
        }
    }

    IEnumerator VibrationTestCoroutine()
    {
        if (!this.UseCurves)
        {
            InputManager.Instance.SetGamepadVibration(Vector2.one);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            var timer = new Timer();
            float duration = this.Left.keys[this.Left.length - 1].time;
            print(duration);
            while (timer.Value < duration)
            {
                InputManager.Instance.SetGamepadVibration(this.Left.Evaluate(timer.Value), this.Right.Evaluate(timer.Value));
                yield return null;
            }
        }

        InputManager.Instance.SetGamepadVibration(Vector2.zero);
    }
}
