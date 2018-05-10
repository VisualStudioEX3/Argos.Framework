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
        public System.Action OnKeyPress;

        /// <summary>
        /// Action event for key down event.
        /// </summary>
        public System.Action OnKeyDown;

        /// <summary>
        /// Action event for key up event.
        /// </summary>
        public System.Action OnKeyUp;
        #endregion

        #region Properties
        /// <summary>
        /// Get the input state.
        /// </summary>
        public bool State { get; private set; }
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
            this.State = false;

            switch (this.KeyEvent)
            {
                case InputKeyEvent.Down:
                    if (UnityEngine.Input.GetKeyDown((KeyCode)this.Main) || UnityEngine.Input.GetKeyDown((KeyCode)this.Alternative) || this.GetGamepadButtonState())
                    {
                        this.State = true;
                        this.OnKeyDown?.Invoke();
                    }
                    break;
                case InputKeyEvent.Up:
                    if (UnityEngine.Input.GetKeyUp((KeyCode)this.Main) || UnityEngine.Input.GetKeyUp((KeyCode)this.Alternative) || this.GetGamepadButtonState())
                    {
                        this.State = true;
                        this.OnKeyUp?.Invoke();
                    }
                    break;
                default:
                    if (UnityEngine.Input.GetKey((KeyCode)this.Main) || UnityEngine.Input.GetKey((KeyCode)this.Alternative) || this.GetGamepadButtonState())
                    {
                        this.State = true;
                        this.OnKeyPress?.Invoke();
                    }
                    break;
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        bool GetGamepadButtonState()
        {
            GamepadButtonStates state = new GamepadButtonStates();

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
        #endregion

        #region Methods & Functions
        public override string ToString()
        {
            return $"KeyEvent: {this.KeyEvent}, Main key: {this.Main}, Alternative key: {this.Alternative}, Gamepad button: {this.GamepadButton}, State: {this.State}";
        }
        #endregion
    }
}