using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Argos.Framework.Input.Extensions;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Multiplatform Input Manager.
    /// </summary>
    /// <remarks>This input manager allow to define input maps for keyboard, mouse & gamepads (XBox360/One, PS4 and Nintendo Switch (Pro) Controllers), input actions, input axis, and allow to change input bindings in runtime.</remarks>
    [AddComponentMenu("Argos.Framework/Input/Input Manager"), DisallowMultipleComponent]
    public sealed class InputManager : MonoBehaviourSingleton<InputManager>
    {
        #region Constants
        const string MOUSE_X_NAME = "Mouse X";
        const string MOUSE_Y_NAME = "Mouse Y";
        const string MOUSE_Z_NAME = "Mouse ScrollWheel";

        const float MIN_MOUSE_X_DELTA = 0.75f;
        const float MIN_MOUSE_Y_DELTA = 0.75f;
        const float MIN_MOUSE_Z_DELTA = 0.5f;

        const float MIN_TIME_INPUT_CHECK_INTERVAL = 0.001f;

        /// <summary>
        /// Array of not assignable keys during the input assignations.
        /// </summary>
        /// <remarks>Used only for keyboard and mouse assignations.</remarks>
        static readonly KeyCode[] NOT_ASSIGNABLE_KEYS;

        /// <summary>
        /// Assignable buttons for any gamepad.
        /// </summary>
        static readonly KeyCode[] ASSIGNABLE_GENERIC_GAMEPAD_BUTTONS;

        /// <summary>
        /// Assignable buttons for XBox Controllers.
        /// </summary>
        static readonly KeyCode[] ASSIGNABLE_XBOX_BUTTONS = (KeyCode[])Enum.GetValues(typeof(XBoxControllerButtons));

        /// <summary>
        /// Assignable buttons for PS4 Controllers.
        /// </summary>
        static readonly KeyCode[] ASSIGNABLE_PS4_BUTTONS = (KeyCode[])Enum.GetValues(typeof(PS4ControllerButtons));

        /// <summary>
        /// Assignable buttons for Nintendo Switch Controller.
        /// </summary>
        static readonly KeyCode[] ASSIGNABLE_NINTENDO_SWITCH_BUTTONS = (KeyCode[])Enum.GetValues(typeof(NintendoSwitchProControllerButtons));
        #endregion

        #region Enums
        /// <summary>
        /// Input controller types.
        /// </summary>
        public enum InputType
        {
            KeyboardAndMouse,
            GenericGamepad,
            XBoxController,
            PS4Controller,
            NintendoSwitchProController
        }
        #endregion

        #region Internal vars
#pragma warning disable 649
        [SerializeField, HideInInspector]
        InputMapDictionary _inputMaps;
#pragma warning restore
        #endregion

        #region Public vars
        [Header("General settings")]
        public bool hideMouseCursorInGamepadMode = true;

        [Tooltip("Interval between input type identification checks."), Min(InputManager.MIN_TIME_INPUT_CHECK_INTERVAL)]
        public float checkInputTypeInterval = 0.02f;

        /// <summary>
        /// Enable gamepad vibration on available devices and platforms.
        /// </summary>
        public bool enableGamepadVibration = true;

        [Header("UI settings")]
        /// <summary>
        /// Time for double click detection on UI controls.
        /// </summary>
        public float doubleClickTime = 0.25f;

        [Header("Nintendo Switch Pro controller (PC only)")]
        [Tooltip("Use the Nintendo Switch Pro controller button layout or XBox button layout.\n\nWhen the Nintendo Switch Pro controller is active, this setting allow to use the Nintendo button layout. If this setting is false, uses the XBox button layout (swtich A B buttons, and X Y buttons to match the XBox controller button layout).\n\nThis setting not affect to XBox360/One, PS4 or generic controllers.")]
        public bool useNintendoButtonLayout = true;
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the input type is changed.
        /// </summary>
        public event Action<InputType> OnInputTypeChange;
        #endregion

        #region Properties
        /// <summary>
        /// Return the current input type is received.
        /// </summary>
        public InputType CurrentInputType { get; private set; }

        /// <summary>
        /// Return true if the mouse was move on the current frame.
        /// </summary>
        public bool HasMotionFromMouse
        {
            get
            {
                return Mathf.Abs(UnityEngine.Input.GetAxisRaw(InputManager.MOUSE_X_NAME)) > InputManager.MIN_MOUSE_X_DELTA ||
                       Mathf.Abs(UnityEngine.Input.GetAxisRaw(InputManager.MOUSE_Y_NAME)) > InputManager.MIN_MOUSE_Y_DELTA ||
                       Mathf.Abs(UnityEngine.Input.GetAxisRaw(InputManager.MOUSE_Z_NAME)) > InputManager.MIN_MOUSE_Z_DELTA;
            }
        }

        /// <summary>
        /// Return true if any key from keyboard or button from mouse is pressed down on current frame.
        /// </summary>
        public bool IsAnyKeyDown { get; private set; }

        /// <summary>
        /// Return true if the gamepad vibration is enable and available.
        /// </summary>
        /// <remarks>In KeyboardAndMouse mode this return false always.</remarks>
        public bool IsGamepadVibrationEnable { get { return this.enableGamepadVibration && this.CurrentInputType != InputType.KeyboardAndMouse; } }

        /// <summary>
        /// Access to Input Map dictionary.
        /// </summary>
        public SerializableDictionary<string, InputMapAsset> InputMaps { get { return this._inputMaps; } }
        #endregion

        #region Initializers
        static InputManager()
        {
            // Initialize KeyCode array constants:
            var list = new List<KeyCode>();
            var keyCodes = Enum.GetValues(typeof(KeyCode));
            {
                list.Add(KeyCode.Escape);
                list.Add(KeyCode.CapsLock);
                list.Add(KeyCode.Numlock);
                list.Add(KeyCode.Break);
                list.Add(KeyCode.Pause);
                list.Add(KeyCode.ScrollLock);
                list.Add(KeyCode.Print);
                list.Add(KeyCode.SysReq);
                list.Add(KeyCode.LeftWindows);
                list.Add(KeyCode.RightWindows);
                list.Add(KeyCode.Menu);
                list.Add(KeyCode.LeftApple);
                list.Add(KeyCode.RightApple);
                list.Add(KeyCode.LeftCommand);
                list.Add(KeyCode.RightCommand);
                list.Add(KeyCode.Help);

                for (int i = (int)KeyCode.JoystickButton0; i < keyCodes.Length; i++)
                {
                    list.Add((KeyCode)i);
                }

                InputManager.NOT_ASSIGNABLE_KEYS = list.ToArray();
            }
            list.Clear();
            {
                for (int i = (int)KeyCode.JoystickButton0; i < keyCodes.Length; i++)
                {
                    list.Add((KeyCode)i);
                    if ((KeyCode)i == KeyCode.JoystickButton19) break;
                }

                InputManager.ASSIGNABLE_GENERIC_GAMEPAD_BUTTONS = list.ToArray();
            }
            list.Clear();
        }

        public override void Awake()
        {
            if (Application.isPlaying)
            {
                Gamepad.Instance.TryToIndentifyGamepad();

                // Check in defined intervals the current active input:
                StartCoroutine(this.CheckCurrentInputTypeCoroutine());

                // Check for a generic joystick and initialize it for support Force Feedback:
                ForceFeedback.CheckForAvailableJoystick();
            }

            base.Awake();
        }

        private void OnApplicationQuit()
        {
            this.SetGamepadVibration(Vector2.zero);

            // Release a generic joystick and stop active Force Feedback effect:
            ForceFeedback.ReleaseJoystick();
        }
        #endregion

        #region Update logic
        private void Update()
        {
            if (Gamepad.Instance.UseNintendoButtonLayout != this.useNintendoButtonLayout)
            {
                Gamepad.Instance.UseNintendoButtonLayout = this.useNintendoButtonLayout;
            }

            this.IsAnyKeyDown = UnityEngine.Input.anyKeyDown;

            Gamepad.Instance.Update();
            this._inputMaps.Update();
        }
        #endregion

        #region Methods & Functions
        public override bool IsPersistentBetweenScenes()
        {
            return true;
        }

        /// <summary>
        /// Get the input map by their name.
        /// </summary>
        /// <param name="name">Input map name.</param>
        /// <returns>Return the <see cref="InputMapAsset"/> reference.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InputMapAsset GetInputMap(string name)
        {
            InputMapAsset map;
            if (this._inputMaps.TryGetValue(name, out map)) return map;

            throw new KeyNotFoundException($"[{this.GetClassName()}]: The input map '{name}' not exists.");
        }

        /// <summary>
        /// Get the axis value.
        /// </summary>
        /// <param name="map">Map where the axis is defined.</param>
        /// <param name="name">Name of the axis.</param>
        /// <returns>Return a Vector2 with the axis value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public Vector2 GetAxis(string map, string name)
        {
            return (Vector2)this.GetInputMap(map).GetAxis(name);
        }

        /// <summary>
        /// Get the action state.
        /// </summary>
        /// <param name="map">Map where the action is defined.</param>
        /// <param name="name">Name of the action.</param>
        /// <returns>Return true if the any event raised on the action.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool GetAction(string map, string name)
        {
            return this.GetInputMap(map).GetAction(name).State;
        }

        /// <summary>
        /// Return the key, mouse button or gamepad button pressed down in the last frame.
        /// </summary>
        /// <returns>Return the KeyCode value.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public KeyCode GetKeyCode(InputType type = InputType.KeyboardAndMouse)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (UnityEngine.Input.GetKeyDown(keyCode))
                {
                    switch (type)
                    {
                        case InputType.KeyboardAndMouse: return InputManager.NOT_ASSIGNABLE_KEYS.Contains(keyCode) ? KeyCode.None : keyCode;
                        case InputType.XBoxController: return InputManager.ASSIGNABLE_XBOX_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;
                        case InputType.PS4Controller: return InputManager.ASSIGNABLE_PS4_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;
                        case InputType.NintendoSwitchProController: return InputManager.ASSIGNABLE_NINTENDO_SWITCH_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;
                        case InputType.GenericGamepad: return InputManager.ASSIGNABLE_GENERIC_GAMEPAD_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;
                    }
                }
            }

            return KeyCode.None;
        }

        /// <summary>
        /// Set gamepad vibration intensity.
        /// </summary>
        /// <param name="strongEngine">Strong intensity (left engine).</param>
        /// <param name="weakEngine">Weak intensity (right engine).</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void SetGamepadVibration(float strongEngine, float weakEngine)
        {
            this.SetGamepadVibration(new Vector2(strongEngine, weakEngine));
        }

        /// <summary>
        /// Set gamepad vibration intensity.
        /// </summary>
        /// <param name="engines">Engines intensity (x = strong/left engine, y = weak/right engine).</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void SetGamepadVibration(Vector2 engines)
        {
            switch (Gamepad.Instance.Type)
            {
                case GamepadType.XBoxController:

                    XInput.SetVibration(this.IsGamepadVibrationEnable ? engines : Vector2.zero);
                    break;

                case GamepadType.Generic:

                    ForceFeedback.SetVibration(this.IsGamepadVibrationEnable ? engines : Vector2.zero);
                    break;
            }
        }

        /// <summary>
        /// Set XBox One controller triggers impulse.
        /// </summary>
        /// <param name="left">Left trigger.</param>
        /// <param name="right">Right trigger.</param>
        /// <remarks>This method only works on UWP builds. Not take effect from editor.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void SetGamepadTriggersImpulse(float left, float right)
        {
            this.SetGamepadTriggersImpulse(new Vector2(left, right));
        }

        /// <summary>
        /// Set XBox One controller triggers impulse.
        /// </summary>
        /// <param name="impulse">Triggers impulse values (x = left trigger, y = right trigger).</param>
        /// <remarks>This method only works on UWP builds. Not take effect from editor.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void SetGamepadTriggersImpulse(Vector2 impulse)
        {
            XInput.SetTriggersImpulse(this.IsGamepadVibrationEnable ? impulse : Vector2.zero);
        }
        #endregion

        #region Coroutines
        // Implemented as coroutine to run in unescaled time. Not posible with InvokeRepeating.
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        IEnumerator CheckCurrentInputTypeCoroutine()
        {
            var wait = new WaitForSecondsRealtime(Mathf.Max(this.checkInputTypeInterval, InputManager.MIN_TIME_INPUT_CHECK_INTERVAL));
            while (true)
            {
                var current = this.CurrentInputType;

                if (this.IsAnyKeyDown || this.HasMotionFromMouse || Gamepad.Instance.HasMotionFromAnyAxis)
                {
                    if (Gamepad.Instance.IsAnyButtonDown || Gamepad.Instance.HasMotionFromAnyAxis)
                    {
                        Gamepad.Instance.TryToIndentifyGamepad();

                        switch (Gamepad.Instance.Type)
                        {
                            case GamepadType.XBoxController:

                                this.CurrentInputType = InputType.XBoxController;
                                break;

                            case GamepadType.PS4Controller:

                                this.CurrentInputType = InputType.PS4Controller;
                                break;

                            case GamepadType.NintendoSwitchProController:

                                this.CurrentInputType = InputType.NintendoSwitchProController;
                                break;

                            default:

                                this.CurrentInputType = InputType.GenericGamepad;
                                break;
                        }

                        Cursor.visible = !this.hideMouseCursorInGamepadMode;
                        Cursor.lockState = this.hideMouseCursorInGamepadMode ? CursorLockMode.Locked : CursorLockMode.None;
                    }
                    else
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        this.CurrentInputType = InputType.KeyboardAndMouse;

                        this.SetGamepadVibration(Vector2.zero);
                        this.SetGamepadTriggersImpulse(Vector2.zero);
                    }

                    if (current != this.CurrentInputType)
                    {
                        this.OnInputTypeChange?.Invoke(this.CurrentInputType);
                    }

                    if (Application.isEditor && current != this.CurrentInputType)
                    {
                        print($"{this.GetClassName()}: Current Input type: {this.CurrentInputType}");
                    }
                }

                yield return wait;
            }
        }
        #endregion

        #region Event listeners
        private void OnEnable()
        {
            if (this._inputMaps == null)
            {
                this._inputMaps = new InputMapDictionary();
            }
        }

        private void Reset()
        {
            this._inputMaps = new InputMapDictionary();
        }
        #endregion
    }
}