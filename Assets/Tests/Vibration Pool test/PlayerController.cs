using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Input;

public class PlayerController : MonoBehaviour
{
    InputMapAsset _ui;
    GamepadVibrationEffectController _vibrationEffect;

    void Start()
    {
        this._ui = InputManager.Instance.GetInputMap("UI");
        this._vibrationEffect = GetComponent<GamepadVibrationEffectController>();
    }

    void Update()
    {
        if (this._ui.GetAction("Submit"))
        {
            this._vibrationEffect.Play();
        }
    }
}
