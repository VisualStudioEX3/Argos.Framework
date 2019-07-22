using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Argos.Framework.Utils;

namespace Argos.Framework.Input
{
    #region Enums
    /// <summary>
    /// Gamepad controller type.
    /// </summary>
    public enum GamepadType
    {
        /// <summary>
        /// Generic gamepad or joystick.
        /// </summary>
        Generic,
        /// <summary>
        /// Xbox360 / Xbox One game controller.
        /// </summary>
        XBoxController,
        /// <summary>
        /// PS4 game controller.
        /// </summary>
        PS4Controller,
        /// <summary>
        /// Nintendo Switch Pro controller.
        /// </summary>
        NintendoSwitchProController
    }

    /// <summary>
    /// Unique gamepad map buttons.
    /// </summary>
    public enum GamepadButtons
    {
        None = -1,
        Button1,
        Button2,
        Button3,
        Button4,
        Start,
        Select,
        LeftStick,
        RightStick,
        LeftBumper,
        RightBumper,
        LeftTrigger,
        RightTrigger,
        DPadLeft,
        DPadRight,
        DPadUp,
        DPadDown
    }
    #endregion

    /// <summary>
    /// Button states.
    /// </summary>
    /// <remarks>Uses to virtualize axis states (triggers, DPad) as button states.</remarks>
    public struct ButtonStates
    {
        #region Internal vars
        bool _isPressed;
        bool _isDown;
        bool _isUp;
        #endregion

        #region Properties
        public bool IsPressed
        {
            get
            {
                return this._isPressed;
            }

            set
            {
                this._isDown = this._isUp = this._isPressed;
                this._isPressed = value;
            }
        }
        public bool IsDown { get { return !this._isDown && this._isPressed; } }
        public bool IsUp { get { return this._isUp && !this._isPressed; } }
        #endregion
    }

    /// <summary>
    /// Generic Gamepad Input Layout struct.
    /// </summary>
    /// <remarks>This is the layout reference for generic gamepad input values.</remarks>
    [Serializable]
    public struct GenericGamepadInputLayout
    {
        #region Enums
        /// <summary>
        /// Close enumeration with only Unity joystick button values (for all joysticks).
        /// </summary>
        public enum UnityJoystickButtons
        {
            JoystickButton0 = KeyCode.JoystickButton0,
            JoystickButton1 = KeyCode.JoystickButton1,
            JoystickButton2 = KeyCode.JoystickButton2,
            JoystickButton3 = KeyCode.JoystickButton3,
            JoystickButton4 = KeyCode.JoystickButton4,
            JoystickButton5 = KeyCode.JoystickButton5,
            JoystickButton6 = KeyCode.JoystickButton6,
            JoystickButton7 = KeyCode.JoystickButton7,
            JoystickButton8 = KeyCode.JoystickButton8,
            JoystickButton9 = KeyCode.JoystickButton9,
            JoystickButton10 = KeyCode.JoystickButton10,
            JoystickButton11 = KeyCode.JoystickButton11,
            JoystickButton12 = KeyCode.JoystickButton12,
            JoystickButton13 = KeyCode.JoystickButton13,
            JoystickButton14 = KeyCode.JoystickButton14,
            JoystickButton15 = KeyCode.JoystickButton15,
            JoystickButton16 = KeyCode.JoystickButton16,
            JoystickButton17 = KeyCode.JoystickButton17,
            JoystickButton18 = KeyCode.JoystickButton18,
            JoystickButton19 = KeyCode.JoystickButton19
        }
        #endregion

        #region Public vars
        /// <summary>
        /// Left Stick axes setup.
        /// </summary>
        [Header("Axes")]
        public Vector2Int leftStickAxes;

        /// <summary>
        /// Invert Y axis on Left Stick.
        /// </summary>
        public bool leftStickInvertY;

        /// <summary>
        /// Right Stick axes setup.
        /// </summary>
        [Space]
        public Vector2Int rightStickAxes;

        /// <summary>
        /// Invert Y axis on Right Stick.
        /// </summary>
        public bool rightStickInvertY;

        /// <summary>
        /// Some gamepads and joysticks defined the DPad as separated axes.
        /// </summary>
        [HelpBox("Some gamepads defined the DPad as separated axes:"), Space]
        public Vector2Int dPadAxes;

        /// <summary>
        /// Invert Y axis on DPad.
        /// </summary>
        public bool dPadInvertY;

        /// <summary>
        /// Some gamepads and joysticks defined the triggers as separated axes.
        /// </summary>
        [HelpBox("Some gamepads defined the triggers as separated axes:"), Space, CustomVector("L", "R")]
        public Vector2Int triggersAxes;

        [Tooltip("XBox A, PS4 Cross or Nintendo Switch A\n(B on inverted layout) button."), Header("Buttons")]
        public UnityJoystickButtons button1;
        [Tooltip("XBox B, PS4 Circle or Nintendo Switch B\n(A on inverted layout) button.")]
        public UnityJoystickButtons button2;
        [Tooltip("XBox X, PS4 Square or Nintendo Switch X\n(Y on inverted layout) button.")]
        public UnityJoystickButtons button3;
        [Tooltip("XBox Y, PS4 Triangle or Nintendo Switch Y\n(X on inverted layout) button.")]
        public UnityJoystickButtons button4;

        [Tooltip("XBox Start/Menu, PS4 Options or Nintendo Switch + button."), Space]
        public UnityJoystickButtons start;
        [Tooltip("XBox Back/View, PS4 Share or Nintendo Switch - button.")]
        public UnityJoystickButtons select;

        [Tooltip("XBox LB, PS4 L1 or Nintendo Switch L button."), Space]
        public UnityJoystickButtons leftBumper;
        [Tooltip("XBox RB, PS4 R1 or Nintendo Switch R button.")]
        public UnityJoystickButtons rightBumper;

        [Tooltip("XBox Left Stick, PS4 L3 or Nintendo Switch Left Stick button."), Space]
        public UnityJoystickButtons leftStick;
        [Tooltip("XBox Right Stick, PS4 R3 or Nintendo Switch Right Stick button.")]
        public UnityJoystickButtons rightStick;

        [HelpBox("Some gamepads defined the triggers as buttons:"), Tooltip("XBox LT, PS4 L2 or Nintendo Switch ZL button."), Space]
        public UnityJoystickButtons leftTrigger;
        [Tooltip("XBox RT, PS4 R2 or Nintendo Switch ZR button.")]
        public UnityJoystickButtons rightTrigger;

        [HelpBox("Some gamepads defined the DPad as buttons:"), Space]
        public UnityJoystickButtons DPadLeft;
        public UnityJoystickButtons DPadRight;
        public UnityJoystickButtons DPadUp;
        public UnityJoystickButtons DPadDown;
        #endregion
    }

    /// <summary>
    /// Gamepad mapper.
    /// </summary>
    /// <remarks>
    /// Multiplatform single player gamepad mapper for works natively with the XBox 360/One, PS4 controllers and Nintendo Switch Pro controller maps on PC (Windows, Linux & Mac), and support for manual setup of any generic gamepad or joystick.
    /// FYI: The PS4 Controller is not supported natively on Linux, and the XBox 360 controller (and wired XBox One 1º Gen) not supported on OSX. Maybe add support for third party drivers for these cases in the future.
    /// </remarks>
    public sealed class Gamepad : Singleton<Gamepad>
    {
        #region Constants
        public const int MAX_AXIS_INDEX = 10;
        public const int MIN_AXIS_INDEX = 1;

        const float AXIS_DELTA = 0.5f;

        static readonly string[] XBOX_CONTROLLER_CHECK_NAMES = new string[] { "xbox", "xinput" };                                                   // Windows and Linux.
        static readonly string[] PS4_CONTROLLER_CHECK_NAMES = new string[] { "wireless controller", "sony" };                                       // Windows and OSX.
        static readonly string[] NINTENDO_SWITCH_CONTROLLER_CHECK_NAMES = new string[] { "pro controller", "wireless gamepad" };                    // Windows (bluetooth only).
        #endregion

        #region Internal vars
        string[] _axisNames;
        #endregion

        #region Properties
        /// <summary>
        /// Gamepad type.
        /// </summary>
        /// <remarks>Set the predefined map for XBox360/One or PS4 controller, or set as unknown gamepad for manual setup.</remarks>
        public GamepadType Type { get; private set; }

        /// <summary>
        /// Generic gamepad setup shortcut.
        /// </summary>
        public GenericGamepadInputLayout GenericGamepadSetup
        {
            get
            {
                if (InputManager.Instance && InputManager.Instance.genericGamepadSetup)
                {
                    return InputManager.Instance.genericGamepadSetup.map; 
                }
                else
                {
                    return new GenericGamepadInputLayout();
                }
            }
        }

        /// <summary>
        /// Use the Nintendo Switch Pro controller button layout or XBox button layout.
        /// </summary>
        /// <remarks>When the Nintendo Switch Pro controller is active, this setting allow to use the Nintendo button layout. If this setting is false, uses the XBox button layout (swtich A B buttons, and X Y buttons to match the XBox controller button layout).
        /// This setting not affect to XBox360/One, PS4 or generic controllers.</remarks>
        public bool UseNintendoButtonLayout = true;

        /// <summary>
        /// Read the left stick.
        /// </summary>
        /// <remarks>Read predefined XBox360/One and PS4 left sticks. For generic/unknown gamepads, set the axes X and Y on <see cref="LeftStickAxesSetup"/> param.</remarks>
        public Vector2 LeftStick { get; private set; }

        /// <summary>
        /// Read the right stick.
        /// </summary>
        /// <remarks>Read predefined XBox360/One and PS4 right sticks. For generic/unknown gamepads, set the axes X and Y on <see cref="RightStickAxesSetup"/> param.</remarks>
        public Vector2 RightStick { get; private set; }

        /// <summary>
        /// Read the directional pad.
        /// </summary>
        public Vector2 DPad { get; private set; }

        public ButtonStates Button1 { get; private set; }
        public ButtonStates Button2 { get; private set; }
        public ButtonStates Button3 { get; private set; }
        public ButtonStates Button4 { get; private set; }

        public ButtonStates Start { get; private set; }
        public ButtonStates Select { get; private set; }

        public ButtonStates LeftBumper { get; private set; }
        public ButtonStates RightBumper { get; private set; }

        public ButtonStates LeftTrigger { get; private set; }
        public ButtonStates RightTrigger { get; private set; }

        public ButtonStates LeftStickButton { get; private set; }
        public ButtonStates RightStickButton { get; private set; }

        public ButtonStates DPadLeft { get; private set; }
        public ButtonStates DPadRight { get; private set; }
        public ButtonStates DPadUp { get; private set; }
        public ButtonStates DPadDown { get; private set; }

        public bool IsAnyButtonPressed
        {
            get
            {
                return this.Button1.IsPressed ||
                       this.Button2.IsPressed ||
                       this.Button3.IsPressed ||
                       this.Button4.IsPressed ||
                       this.Start.IsPressed ||
                       this.Select.IsPressed ||
                       this.LeftBumper.IsPressed ||
                       this.RightBumper.IsPressed ||
                       this.LeftTrigger.IsPressed ||
                       this.RightTrigger.IsPressed ||
                       this.LeftStickButton.IsPressed ||
                       this.RightStickButton.IsPressed ||
                       this.DPadLeft.IsPressed ||
                       this.DPadRight.IsPressed ||
                       this.DPadUp.IsPressed ||
                       this.DPadDown.IsPressed;
            }
        }

        public bool IsAnyButtonDown
        {
            get
            {
                return this.Button1.IsDown ||
                       this.Button2.IsDown ||
                       this.Button3.IsDown ||
                       this.Button4.IsDown ||
                       this.Start.IsDown ||
                       this.Select.IsDown ||
                       this.LeftBumper.IsDown ||
                       this.RightBumper.IsDown ||
                       this.LeftTrigger.IsDown ||
                       this.RightTrigger.IsDown ||
                       this.LeftStickButton.IsDown ||
                       this.RightStickButton.IsDown ||
                       this.DPadLeft.IsDown ||
                       this.DPadRight.IsDown ||
                       this.DPadUp.IsDown ||
                       this.DPadDown.IsDown;
            }
        }

        public bool HasMotionFromAnyAxis { get { return this.LeftStick != Vector2.zero || this.RightStick != Vector2.zero || this.DPad != Vector2.zero; } }
        #endregion

        #region Constructors
        public Gamepad()
        {
            this._axisNames = new string[Gamepad.MAX_AXIS_INDEX];
            for (int i = 0; i < this._axisNames.Length; i++)
            {
                this._axisNames[i] = $"Gamepad {i + 1} Axis";
            }
        }
        #endregion

        #region Update logic
        /// <summary>
        /// Update gamepad values.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            this.ReadLeftStick();
            this.ReadRightStick();
            this.ReadDPad();
            this.ReadTriggers();
            this.ReadButtons();
        }
        #endregion

        #region Methods & Functions
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void ReadButtons()
        {
            ButtonStates state;

            KeyCode button1,
                    button2,
                    button3,
                    button4,
                    start,
                    select,
                    leftBumper,
                    rightBumper,
                    leftStickButton,
                    rightStickButton;

            switch (this.Type)
            {
                case GamepadType.XBoxController:

                    button1 = (KeyCode)XBoxControllerButtons.A;
                    button2 = (KeyCode)XBoxControllerButtons.B;
                    button3 = (KeyCode)XBoxControllerButtons.X;
                    button4 = (KeyCode)XBoxControllerButtons.Y;
                    start = (KeyCode)XBoxControllerButtons.Start;
                    select = (KeyCode)XBoxControllerButtons.Back;
                    leftBumper = (KeyCode)XBoxControllerButtons.LeftBumper;
                    rightBumper = (KeyCode)XBoxControllerButtons.RightBumper;
                    leftStickButton = (KeyCode)XBoxControllerButtons.LeftStick;
                    rightStickButton = (KeyCode)XBoxControllerButtons.RightStick;
                    break;

                case GamepadType.PS4Controller:

                    button1 = (KeyCode)PS4ControllerButtons.Cross;
                    button2 = (KeyCode)PS4ControllerButtons.Circle;
                    button3 = (KeyCode)PS4ControllerButtons.Square;
                    button4 = (KeyCode)PS4ControllerButtons.Triangle;
                    start = (KeyCode)PS4ControllerButtons.Options;
                    select = (KeyCode)PS4ControllerButtons.Share;
                    leftBumper = (KeyCode)PS4ControllerButtons.L1;
                    rightBumper = (KeyCode)PS4ControllerButtons.R1;
                    leftStickButton = (KeyCode)PS4ControllerButtons.L3;
                    rightStickButton = (KeyCode)PS4ControllerButtons.R3;
                    break;

                case GamepadType.NintendoSwitchProController:

                    button1 = this.UseNintendoButtonLayout ? (KeyCode)NintendoSwitchProControllerButtons.A : (KeyCode)NintendoSwitchProControllerButtons.B;
                    button2 = this.UseNintendoButtonLayout ? (KeyCode)NintendoSwitchProControllerButtons.B : (KeyCode)NintendoSwitchProControllerButtons.A;
                    button3 = this.UseNintendoButtonLayout ? (KeyCode)NintendoSwitchProControllerButtons.X : (KeyCode)NintendoSwitchProControllerButtons.Y;
                    button4 = this.UseNintendoButtonLayout ? (KeyCode)NintendoSwitchProControllerButtons.Y : (KeyCode)NintendoSwitchProControllerButtons.X;
                    start = (KeyCode)NintendoSwitchProControllerButtons.Plus;
                    select = (KeyCode)NintendoSwitchProControllerButtons.Minus;
                    leftBumper = (KeyCode)NintendoSwitchProControllerButtons.ZL;
                    rightBumper = (KeyCode)NintendoSwitchProControllerButtons.ZR;
                    leftStickButton = (KeyCode)NintendoSwitchProControllerButtons.LeftStick;
                    rightStickButton = (KeyCode)NintendoSwitchProControllerButtons.RightStick;
                    break;

                default:

                    button1 = (KeyCode)this.GenericGamepadSetup.button1;
                    button2 = (KeyCode)this.GenericGamepadSetup.button2;
                    button3 = (KeyCode)this.GenericGamepadSetup.button3;
                    button4 = (KeyCode)this.GenericGamepadSetup.button4;
                    start = (KeyCode)this.GenericGamepadSetup.start;
                    select = (KeyCode)this.GenericGamepadSetup.select;
                    leftBumper = (KeyCode)this.GenericGamepadSetup.leftBumper;
                    rightBumper = (KeyCode)this.GenericGamepadSetup.rightBumper;
                    leftStickButton = (KeyCode)this.GenericGamepadSetup.leftStick;
                    rightStickButton = (KeyCode)this.GenericGamepadSetup.rightStick;
                    break;

            }

            state = this.Button1;
            this.ReadButtonState(button1, ref state);
            this.Button1 = state;

            state = this.Button2;
            this.ReadButtonState(button2, ref state);
            this.Button2 = state;

            state = this.Button3;
            this.ReadButtonState(button3, ref state);
            this.Button3 = state;

            state = this.Button4;
            this.ReadButtonState(button4, ref state);
            this.Button4 = state;

            state = this.Start;
            this.ReadButtonState(start, ref state);
            this.Start = state;

            state = this.Select;
            this.ReadButtonState(select, ref state);
            this.Select = state;

            state = this.LeftBumper;
            this.ReadButtonState(leftBumper, ref state);
            this.LeftBumper = state;

            state = this.RightBumper;
            this.ReadButtonState(rightBumper, ref state);
            this.RightBumper = state;

            state = this.LeftStickButton;
            this.ReadButtonState(leftStickButton, ref state);
            this.LeftStickButton = state;

            state = this.RightStickButton;
            this.ReadButtonState(rightStickButton, ref state);
            this.RightStickButton = state;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void ReadLeftStick()
        {
            string xAxis, yAxis;
            float invertY;

            switch (this.Type)
            {
                case GamepadType.XBoxController:

                    xAxis = this.GetAxisName((int)XboxControllerAxes.LeftStickX);
                    yAxis = this.GetAxisName((int)XboxControllerAxes.LeftStickY);
                    invertY = -1f;
                    break;

                case GamepadType.PS4Controller:

                    xAxis = this.GetAxisName((int)PS4ControllerAxes.LeftStickX);
                    yAxis = this.GetAxisName((int)PS4ControllerAxes.LeftStickY);
                    invertY = -1f;
                    break;

                case GamepadType.NintendoSwitchProController:

                    xAxis = this.GetAxisName((int)NintendoSwitchProControllerAxes.LeftStickX);
                    yAxis = this.GetAxisName((int)NintendoSwitchProControllerAxes.LeftStickY);
                    invertY = -1f;
                    break;

                default:

                    xAxis = this.GetAxisName(this.GenericGamepadSetup.leftStickAxes.x);
                    yAxis = this.GetAxisName(this.GenericGamepadSetup.leftStickAxes.y);
                    invertY = this.GenericGamepadSetup.leftStickInvertY ? -1f : 1f;
                    break;
            }

            this.LeftStick = new Vector2(UnityEngine.Input.GetAxis(xAxis),
                                         UnityEngine.Input.GetAxis(yAxis) * invertY);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void ReadRightStick()
        {
            string xAxis, yAxis;
            float invertY;

            switch (this.Type)
            {
                case GamepadType.XBoxController:

                    xAxis = this.GetAxisName((int)XboxControllerAxes.RightStickX);
                    yAxis = this.GetAxisName((int)XboxControllerAxes.RightStickY);
                    invertY = -1f;
                    break;

                case GamepadType.PS4Controller:

                    xAxis = this.GetAxisName((int)PS4ControllerAxes.RightStickX);
                    yAxis = this.GetAxisName((int)PS4ControllerAxes.RightStickY);
                    invertY = -1f;
                    break;

                case GamepadType.NintendoSwitchProController:

                    xAxis = this.GetAxisName((int)NintendoSwitchProControllerAxes.RightStickX);
                    yAxis = this.GetAxisName((int)NintendoSwitchProControllerAxes.RightStickY);
                    invertY = -1f;
                    break;

                default:

                    xAxis = this.GetAxisName(this.GenericGamepadSetup.rightStickAxes.x);
                    yAxis = this.GetAxisName(this.GenericGamepadSetup.rightStickAxes.y);
                    invertY = this.GenericGamepadSetup.rightStickInvertY ? -1f : 1f;
                    break;
            }

            this.RightStick = new Vector2(UnityEngine.Input.GetAxis(xAxis),
                                          UnityEngine.Input.GetAxis(yAxis) * invertY);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void ReadDPad()
        {
            string xAxis, yAxis;
            float invertY;

            switch (this.Type)
            {
                case GamepadType.XBoxController:

                    xAxis = this.GetAxisName((int)XboxControllerAxes.DPadX);
                    yAxis = this.GetAxisName((int)XboxControllerAxes.DPadY);
                    invertY = 1f;

                    // Windows always. On Linux, check first the axes (for wired controllers):
                    this.DPad = new Vector2(UnityEngine.Input.GetAxis(xAxis),
                                            UnityEngine.Input.GetAxis(yAxis) * invertY);

#if UNITY_STANDALONE_LINUX
                    // In Linux, if axes not return values (maybe is a wireless controller), check buttons:
                    if (this.DPad == Vector2.zero)
                    {
                        if (UnityEngine.Input.GetKeyDown((KeyCode)XBoxControllerButtons.DPadLeft))
                        {
                            this.DPad = Vector2.left;
                        }
                        else if (UnityEngine.Input.GetKeyDown((KeyCode)XBoxControllerButtons.DPadRight))
                        {
                            this.DPad = Vector2.right;
                        }
                        else if (UnityEngine.Input.GetKeyDown((KeyCode)XBoxControllerButtons.DPadUp))
                        {
                            this.DPad = Vector2.up;
                        }
                        else if (UnityEngine.Input.GetKeyDown((KeyCode)XBoxControllerButtons.DPadDown))
                        {
                            this.DPad = Vector2.down;
                        }
                    }
#endif
                    break;

                case GamepadType.PS4Controller:

                    xAxis = this.GetAxisName((int)PS4ControllerAxes.DPadX);
                    yAxis = this.GetAxisName((int)PS4ControllerAxes.DPadY);
                    invertY = 1f;

                    this.DPad = new Vector2(UnityEngine.Input.GetAxis(xAxis),
                                            UnityEngine.Input.GetAxis(yAxis) * invertY);
                    break;

                case GamepadType.NintendoSwitchProController:

                    xAxis = this.GetAxisName((int)NintendoSwitchProControllerAxes.DPadX);
                    yAxis = this.GetAxisName((int)NintendoSwitchProControllerAxes.DPadY);
                    invertY = 1f;

                    this.DPad = new Vector2(UnityEngine.Input.GetAxis(xAxis),
                                            UnityEngine.Input.GetAxis(yAxis) * invertY);
                    break;

                default:

                    // Check first the axes:
                    xAxis = this.GetAxisName(this.GenericGamepadSetup.dPadAxes.x);
                    yAxis = this.GetAxisName(this.GenericGamepadSetup.dPadAxes.y);
                    invertY = this.GenericGamepadSetup.dPadInvertY ? -1f : 1f;

                    this.DPad = new Vector2(UnityEngine.Input.GetAxis(xAxis),
                                            UnityEngine.Input.GetAxis(yAxis) * invertY);

                    // If axes not return values, check buttons:
                    if (this.DPad == Vector2.zero)
                    {
                        if (UnityEngine.Input.GetKeyDown((KeyCode)this.GenericGamepadSetup.DPadLeft))
                        {
                            this.DPad = Vector2.left;
                        }
                        else if (UnityEngine.Input.GetKeyDown((KeyCode)this.GenericGamepadSetup.DPadRight))
                        {
                            this.DPad = Vector2.right;
                        }
                        else if (UnityEngine.Input.GetKeyDown((KeyCode)this.GenericGamepadSetup.DPadUp))
                        {
                            this.DPad = Vector2.up;
                        }
                        else if (UnityEngine.Input.GetKeyDown((KeyCode)this.GenericGamepadSetup.DPadDown))
                        {
                            this.DPad = Vector2.down;
                        }
                    }
                    break;
            }

            // Process button states:
            ButtonStates left = this.DPadLeft;
            {
                left.IsPressed = this.DPad.x < -Gamepad.AXIS_DELTA;
            }
            this.DPadLeft = left;

            ButtonStates right = this.DPadRight;
            {
                right.IsPressed = this.DPad.x > Gamepad.AXIS_DELTA;
            }
            this.DPadRight = right;

            ButtonStates up = this.DPadUp;
            {
                up.IsPressed = this.DPad.y > Gamepad.AXIS_DELTA;
            }
            this.DPadUp = up;

            ButtonStates down = this.DPadDown;
            {
                down.IsPressed = this.DPad.y < -Gamepad.AXIS_DELTA;
            }
            this.DPadDown = down;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void ReadTriggers()
        {
            ButtonStates left = this.LeftTrigger;
            ButtonStates right = this.RightTrigger;

            switch (this.Type)
            {
                case GamepadType.XBoxController:

                    this.ReadAxisTriggerState((int)XboxControllerAxes.LeftTrigger, ref left);
                    this.ReadAxisTriggerState((int)XboxControllerAxes.RightTrigger, ref right);

                    break;

                case GamepadType.PS4Controller:

                    this.ReadButtonState((KeyCode)PS4ControllerButtons.L2, ref left);
                    this.ReadButtonState((KeyCode)PS4ControllerButtons.R2, ref right);

                    break;

                case GamepadType.NintendoSwitchProController:

                    this.ReadButtonState((KeyCode)NintendoSwitchProControllerButtons.ZL, ref left);
                    this.ReadButtonState((KeyCode)NintendoSwitchProControllerButtons.ZR, ref right);

                    break;

                default:

                    // Check first the axes, if axes not return values, check buttons:
                    if (!this.ReadAxisTriggerState((int)this.GenericGamepadSetup.triggersAxes.x, ref left))
                    {
                        this.ReadButtonState((KeyCode)this.GenericGamepadSetup.leftTrigger, ref left);
                    }

                    if (!this.ReadAxisTriggerState((int)this.GenericGamepadSetup.triggersAxes.y, ref right))
                    {
                        this.ReadButtonState((KeyCode)this.GenericGamepadSetup.rightTrigger, ref right);
                    }
                    break;
            }

            this.LeftTrigger = left;
            this.RightTrigger = right;
        }

        /// <summary>
        /// Read and process the axis as trigger button states.
        /// </summary>
        /// <param name="axis">Axis index from 1 to 10.</param>
        /// <param name="states">States variable reference.</param>
        /// <returns>Return true if any state are true.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        bool ReadAxisTriggerState(int axis, ref ButtonStates states)
        {
            if (!Utils.MathUtility.IsClamped(axis, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX)) return false;

            // KeyDown and KeyUp logic run in GamepadButtonStates code:
            states.IsPressed = UnityEngine.Input.GetAxis(this.GetAxisName(axis)) > Gamepad.AXIS_DELTA;

            return states.IsPressed || states.IsDown || states.IsUp;
        }

        /// <summary>
        /// Read button states.
        /// </summary>
        /// <param name="button">Gamepad button to read.</param>
        /// <param name="states">States variable reference.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        void ReadButtonState(KeyCode button, ref ButtonStates states)
        {
            if (!Utils.MathUtility.IsClamped((int)button, (int)KeyCode.JoystickButton0, (int)KeyCode.JoystickButton19)) return;

            // KeyDown and KeyUp logic run in GamepadButtonStates code:
            states.IsPressed = UnityEngine.Input.GetKey(button);
        }

        /// <summary>
        /// Return the axis name defined on Unity Input Manager.
        /// </summary>
        /// <param name="index">Axis index, a value between 1 and 10 (this value is clamped).</param>
        /// <returns>Return the formated string with the axis name.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        string GetAxisName(int index)
        {
            return this._axisNames[Mathf.Clamp(index, Gamepad.MIN_AXIS_INDEX, Gamepad.MAX_AXIS_INDEX) - 1];
        }

        /// <summary>
        /// Return a vector with the key down axis values.
        /// </summary>
        /// <param name="xAxis">Horizontal axis.</param>
        /// <param name="yAxis">Vertical axis.</param>
        /// <returns>Vector2.right (1, 0) when keydown on x axis and Vector2.up (0, 1) when keydown on y axis.</returns>
        Vector2 GetAxisKeyDownVector(string xAxis, string yAxis)
        {
            return new Vector2()
            {
                x = UnityEngine.Input.GetButtonDown(xAxis) ? 1f : 0f,
                y = UnityEngine.Input.GetButtonDown(yAxis) ? 1f : 0f
            };
        }

        /// <summary>
        /// Trying to indetify the gamepad.
        /// </summary>
        /// <returns>Return the type of gamepad (if can identify it, if not, return as generic).</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void TryToIndentifyGamepad()
        {
            foreach (var name in UnityEngine.Input.GetJoystickNames())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (StringUtility.CheckForString(name.ToLower(), Gamepad.XBOX_CONTROLLER_CHECK_NAMES))
                    {
                        this.Type = GamepadType.XBoxController;
                        return;
                    }
                    else if (StringUtility.CheckForString(name.ToLower(), Gamepad.PS4_CONTROLLER_CHECK_NAMES))
                    {
                        this.Type = GamepadType.PS4Controller;
                        return;
                    }
                    else if (StringUtility.CheckForString(name.ToLower(), Gamepad.NINTENDO_SWITCH_CONTROLLER_CHECK_NAMES))
                    {
                        this.Type = GamepadType.NintendoSwitchProController;
                        return;
                    }
                }
            }

            this.Type = GamepadType.Generic;
        }
        #endregion
    }
}