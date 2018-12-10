using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using Argos.Framework;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Input action.
    /// </summary>
    /// <remarks>Check the state of a key, mouse button or gamepad button, and raise events.</remarks>
    [Serializable]
    public class InputAction
    {
        #region Enums
        /// <summary>
        /// Input action events.
        /// </summary>
        public enum InputKeyEvent
        {
            Pressed,
            Down,
            Up
        }
        #endregion

        #region Public vars
        /// <summary>
        /// Key event.
        /// </summary>
        public InputKeyEvent KeyEvent;
        /// <summary>
        /// Main key input.
        /// </summary>
        [Space]
        public KeyboardMouseCodes Main;
        /// <summary>
        /// Alternative key input.
        /// </summary>
        public KeyboardMouseCodes Alternative;
        /// <summary>
        /// Gamepad button input.
        /// </summary>
        /// <remarks>Reference the gamepad button map, not a direct KeyCode value.</remarks>
        public GamepadButtons GamepadButton;
        #endregion

        #region Events & delegates
        /// <summary>
        /// Action event for key press event.
        /// </summary>
        public event System.Action OnKeyPress;

        /// <summary>
        /// Action event for key down event.
        /// </summary>
        public event System.Action OnKeyDown;

        /// <summary>
        /// Action event for key up event.
        /// </summary>
        public event System.Action OnKeyUp;
        #endregion

        #region Properties
        /// <summary>
        /// Get the input state.
        /// </summary>
        public bool State { get; private set; }
        #endregion

        #region Operators
        /// <summary>
        /// Cast to bool.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator bool(InputAction value)
        {
            return value.State;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="main">Main key.</param>
        /// <param name="alternative">Alternative key.</param>
        /// <param name="keyEvent">Key event to check. By default is Pressed.</param>
        /// <param name="gamepadButton">Gamepad button to check. Reference the gamepad button map, not a direct KeyCode value.</param>
        /// <param name="onKeyPress">Optional. Event listener for key press event.</param>
        /// <param name="onKeyDown">Optional. Event listener for key down event.</param>
        /// <param name="onKeyUp">Optional. Event listener for key up event.</param>
        public InputAction(KeyboardMouseCodes main, KeyboardMouseCodes alternative, GamepadButtons gamepadButton, InputKeyEvent keyEvent = InputKeyEvent.Pressed, System.Action onKeyPress = null, System.Action onKeyDown = null, System.Action onKeyUp = null)
        {
            this.Main = main;
            this.Alternative = alternative;
            this.GamepadButton = gamepadButton;
            this.KeyEvent = keyEvent;

            this.State = false;

            this.OnKeyPress = onKeyPress;
            this.OnKeyDown = onKeyDown;
            this.OnKeyUp = onKeyUp;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="instance">Previous instance of an InputAction.</param>
        /// <remarks>Use this to fast clone struct.</remarks>
        public InputAction(InputAction instance) : this(instance.Main, instance.Alternative, instance.GamepadButton, instance.KeyEvent, instance.OnKeyPress, instance.OnKeyDown, instance.OnKeyUp)
        {
        }
        #endregion

        #region Update logic
        /// <summary>
        /// Update logic of the input action.
        /// </summary>
        /// <remarks>Check for the event status, and raised the event/delegates.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            this.State = (GetKeyState(this.Main) || GetKeyState(this.Alternative) || this.GetGamepadButtonState());

            if (this.State)
            {
                switch (this.KeyEvent)
                {
                    case InputKeyEvent.Pressed:

                        this.OnKeyPress?.Invoke();
                        break;

                    case InputKeyEvent.Down:

                        this.OnKeyDown?.Invoke();
                        break;

                    case InputKeyEvent.Up:

                        this.OnKeyUp?.Invoke();
                        break;
                }
            }
        }
        #endregion

        #region Methods & Functions
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        bool GetKeyState(KeyboardMouseCodes code)
        {
            switch (code)
            {
                case KeyboardMouseCodes.MouseWheelUp:

                    return UnityEngine.Input.mouseScrollDelta.y > 0f;

                case KeyboardMouseCodes.MouseWheelDown:

                    return UnityEngine.Input.mouseScrollDelta.y < 0f;

                default:

                    switch (this.KeyEvent)
                    {
                        case InputKeyEvent.Down:

                            return UnityEngine.Input.GetKeyDown((KeyCode)code);

                        case InputKeyEvent.Up:

                            return UnityEngine.Input.GetKeyUp((KeyCode)code);

                        default:

                            return UnityEngine.Input.GetKey((KeyCode)code);
                    }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        bool GetGamepadButtonState()
        {
            ButtonStates state = new ButtonStates();

            switch (this.GamepadButton)
            {
                case GamepadButtons.Button1:
                    state = Gamepad.Instance.Button1;
                    break;
                case GamepadButtons.Button2:
                    state = Gamepad.Instance.Button2;
                    break;
                case GamepadButtons.Button3:
                    state = Gamepad.Instance.Button3;
                    break;
                case GamepadButtons.Button4:
                    state = Gamepad.Instance.Button4;
                    break;
                case GamepadButtons.Start:
                    state = Gamepad.Instance.Start;
                    break;
                case GamepadButtons.Select:
                    state = Gamepad.Instance.Select;
                    break;
                case GamepadButtons.LeftStick:
                    state = Gamepad.Instance.LeftStickButton;
                    break;
                case GamepadButtons.RightStick:
                    state = Gamepad.Instance.RightStickButton;
                    break;
                case GamepadButtons.LeftBumper:
                    state = Gamepad.Instance.LeftBumper;
                    break;
                case GamepadButtons.RightBumper:
                    state = Gamepad.Instance.RightBumper;
                    break;
                case GamepadButtons.LeftTrigger:
                    state = Gamepad.Instance.LeftTrigger;
                    break;
                case GamepadButtons.RightTrigger:
                    state = Gamepad.Instance.RightTrigger;
                    break;
                case GamepadButtons.DPadLeft:
                    state = Gamepad.Instance.DPadLeft;
                    break;
                case GamepadButtons.DPadRight:
                    state = Gamepad.Instance.DPadRight;
                    break;
                case GamepadButtons.DPadUp:
                    state = Gamepad.Instance.DPadUp;
                    break;
                case GamepadButtons.DPadDown:
                    state = Gamepad.Instance.DPadDown;
                    break;
            }

            return this.KeyEvent == InputKeyEvent.Pressed && state.IsPressed ||
                   this.KeyEvent == InputKeyEvent.Down && state.IsDown ||
                   this.KeyEvent == InputKeyEvent.Up && state.IsUp;
        }

        public override string ToString()
        {
            return $"KeyEvent: {this.KeyEvent}, Main key: {this.Main}, Alternative key: {this.Alternative}, Gamepad button: {this.GamepadButton}, State: {this.State}";
        }
        #endregion
    }
}