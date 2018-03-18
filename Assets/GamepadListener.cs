using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadListener : MonoBehaviour
{
    Text _text;

    private void Awake()
    {
        this._text = GetComponent<Text>();
    }

    void Update()
    {
        this._text.text = string.Empty;

        for (int i = 1; i < 11; i++)
        {
            this._text.text += $"Gamepad Axis #{i}: {Input.GetAxis($"Gamepad {i} Axis")}\n";
        }

        this._text.text += "\n";

        //for (int i = 0; i < 20; i++)
        //{
        //    this._text.text += $"JoystickButton{i}: {Input.GetKey((KeyCode)(i + KeyCode.JoystickButton0))}\n";
        //}

        //return;

        this._text.text += $"Button A: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.A)}\n";
        this._text.text += $"Button B: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.B)}\n";
        this._text.text += $"Button Y: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.Y)}\n";
        this._text.text += $"Button X: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.X)}\n";

        this._text.text += $"Button +: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.Plus)}\n";
        this._text.text += $"Button -: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.Minus)}\n";

        this._text.text += $"Button Left Stick: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.LeftStick)}\n";
        this._text.text += $"Button Right Stick: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.RightStick)}\n";

        this._text.text += $"Button L: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.L)}\n";
        this._text.text += $"Button R: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.R)}\n";

        this._text.text += $"Button ZL: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.ZL)}\n";
        this._text.text += $"Button ZR: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.ZR)}\n";

        //this._text.text += $"Button DPad Left: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.DPadLeft)}\n";
        //this._text.text += $"Button DPad Right: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.DPadRight)}\n";
        //this._text.text += $"Button DPad Down: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.DPadDown)}\n";
        //this._text.text += $"Button DPad Up: {Input.GetKey((KeyCode)Argos.Framework.Input.NintendoSwitchProControllerButtons.DPadUp)}\n";
    }
}
