using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;

namespace Argos.Framework.Input
{
#if UNITY_STANDALONE_LINUX
    /// <summary>
    /// XBox 360/XBox One Controller map for Linux.
    /// </summary>
    /// <remarks>Not required driver setup (tested on Ubuntu 16.04 LTS).
    /// On Linux, only wireless controllers support using the D-Pad as buttons.</remarks>
    public enum XBoxControllerButtons
    {
       A = KeyCode.JoystickButton0,
       B = KeyCode.JoystickButton1,
       X = KeyCode.JoystickButton2,
       Y = KeyCode.JoystickButton3,
       LeftBumper = KeyCode.JoystickButton4,
       RightBumper = KeyCode.JoystickButton5,
       Back = KeyCode.JoystickButton6,
       Start = KeyCode.JoystickButton7,
       LeftStick = KeyCode.JoystickButton9,
       RightStick = KeyCode.JoystickButton10,
       DPadLeft = KeyCode.JoystickButton11,
       DPadRight = KeyCode.JoystickButton12,
       DPadUp = KeyCode.JoystickButton13,
       DPadDown = KeyCode.JoystickButton14
    }

    /// <summary>
    /// XBox 360/XBox One Controller axis map for Linux.
    /// </summary>
    /// <remarks>Not required driver setup (tested on Ubuntu 16.04 LTS).
    /// On Linux, only wired controllers support using the D-Pad as axes.</remarks>
    public enum XboxControllerAxes
    {
        LeftStickX = 1,
        LeftStickY = 2,
        RightStickX = 4,
        RightStickY = 5,
        LeftTrigger = 3, // 0 to 1 range, unpressed is 0
        RightTrigger = 6, // 0 to 1 range, unpressed is 0
        DPadX = 7, // On wireless controllers use JoystickButton11 for Left and JoystickButton12 for Right.
        DPadY = 8  // On wireless controllers use JoystickButton13 for Up and JoystickButton14 for Down.
    }
#else
    /// <summary>
    /// XBox 360/XBox One Controller button map for Windows and XBox One.
    /// </summary>
    /// <remarks>Not required driver setup on Windows 8/8.1 and 10. Windows 7 and early required driver setup: https://www.microsoft.com/accessories/en-gb/d/xbox-360-controller-for-windows 
    /// 
    /// In consoles (XBox One, PS4, Switch) the gamepad use this map.</remarks>
    public enum XBoxControllerButtons
    {
        A = KeyCode.JoystickButton0,
        B = KeyCode.JoystickButton1,
        X = KeyCode.JoystickButton2,
        Y = KeyCode.JoystickButton3,
        LeftBumper = KeyCode.JoystickButton4,
        RightBumper = KeyCode.JoystickButton5,
        Back = KeyCode.JoystickButton6,
        Start = KeyCode.JoystickButton7,
        LeftStick = KeyCode.JoystickButton8,
        RightStick = KeyCode.JoystickButton9
    }

    /// <summary>
    /// XBox 360/XBox One Controller axis map for Windows and XBox One.
    /// </summary>
    /// <remarks>LeftStick always is the X and Y axis on Unity Input Manager asset.
    /// 
    /// In consoles (XBox One, PS4, Switch) the gamepad use this map.</remarks>
    public enum XboxControllerAxes
    {
        LeftStickX = 1,
        LeftStickY = 2,
        RightStickX = 4,
        RightStickY = 5,
        LeftTrigger = 9, // 0 to 1 range, unpressed is 0
        RightTrigger = 10, // 0 to 1 range, unpressed is 0
        DPadX = 6,
        DPadY = 7
    }
#endif

    /// <summary>
    /// PS4 Controller button map.
    /// </summary>
    /// <remarks>PS4 Controller not supported natively on Linux.</remarks>
    public enum PS4ControllerButtons
    {
        Square = KeyCode.JoystickButton0,
        Cross = KeyCode.JoystickButton1,
        Circle = KeyCode.JoystickButton2,
        Triangle = KeyCode.JoystickButton3,
        L1 = KeyCode.JoystickButton4,
        R1 = KeyCode.JoystickButton5,
        L2 = KeyCode.JoystickButton6,
        R2 = KeyCode.JoystickButton7,
        Share = KeyCode.JoystickButton8,
        Options = KeyCode.JoystickButton9,
        L3 = KeyCode.JoystickButton10,
        R3 = KeyCode.JoystickButton11,
        TouchPad = KeyCode.JoystickButton13
    }

    /// <summary>
    /// PS4 Controller axis map.
    /// </summary>
    /// <remarks>PS4 Controller not supported natively on Linux.</remarks>
    public enum PS4ControllerAxes
    {
        LeftStickX = 1,
        LeftStickY = 2,
        RightStickX = 3,
        RightStickY = 4,
        L2 = 5,
        R2 = 6,
        DPadX = 7,
        DPadY = 8
    }

    /// <summary>
    /// Nintendo Switch Pro Controller button map.
    /// </summary>
    /// <remarks>Nintendo Switch Pro Controller only supported natively on Windows (Windows 10 at least) via Bluetooth.</remarks>
    public enum NintendoSwitchProControllerButtons
    {
        B = KeyCode.JoystickButton0,
        A = KeyCode.JoystickButton1,
        Y = KeyCode.JoystickButton2,
        X = KeyCode.JoystickButton3,
        L = KeyCode.JoystickButton4,
        R = KeyCode.JoystickButton5,
        Minus = KeyCode.JoystickButton8,
        Plus = KeyCode.JoystickButton9,
        LeftStick = KeyCode.JoystickButton10,
        RightStick = KeyCode.JoystickButton11,
        ZL = KeyCode.JoystickButton6,
        ZR = KeyCode.JoystickButton7
    }

    /// <summary>
    /// Nintendo Switch Pro Controller axis map.
    /// </summary>
    /// <remarks>Nintendo Switch Pro Controller only supported natively on Windows (Windows 10 at least) via Bluetooth.</remarks>
    public enum NintendoSwitchProControllerAxes
    {
        LeftStickX = 2,
        LeftStickY = 4,
        RightStickX = 7,
        RightStickY = 8,
        DPadX = 9,
        DPadY = 10
    }
}
