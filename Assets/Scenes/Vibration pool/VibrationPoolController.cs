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

    void Update()
    {
        if (InputManager.Instance.GetAction("UI", "Submit"))
        {
            //InputManager.Instance.SetGamepadVibration(this.Actions[0]);
            StartCoroutine(this.VibrationTestCoroutine());
        }

        //if (InputManager.Instance.GetAction("UI", "Cancel"))
        //{
        //    InputManager.Instance.SetGamepadVibration(this.Actions[1]);
        //}

        //if (InputManager.Instance.GetAction("UI", "Default"))
        //{
        //    InputManager.Instance.SetGamepadVibration(this.Background, true);
        //}
    }

    [Button]
    void Test()
    {
        
    }

    IEnumerator VibrationTestCoroutine()
    {
        InputManager.Instance.SetGamepadVibration(Vector2.one);
        yield return new WaitForSeconds(3f);
        InputManager.Instance.SetGamepadVibration(Vector2.zero);
    }
}
