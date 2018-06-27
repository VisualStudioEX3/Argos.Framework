#if UNITY_STANDALONE || UNITY_EDITOR
#define UNITY_DESKTOP
#endif

#if UNITY_XBOXONE || UNITY_PS4 || UNITY_SWITCH
#define UNITY_CONSOLE
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Argos.Framework.Input.Extensions;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using Argos.Framework;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Multiplatform Input Manager.
    /// </summary>
    /// <remarks>This input manager allow to define input maps for keyboard, mouse & gamepads (XBox360/One, PS4 and Nintendo Switch (Pro) Controllers), input actions, input axis, and allow to change input bindings in runtime.</remarks>
    [AddComponentMenu("Argos.Framework/Input/Input Manager"), DisallowMultipleComponent]
    public sealed class InputManager : MonoBehaviour
    {
        #region Singleton
        public static InputManager Instance { get; private set; } 
        #endregion

        #region Constants
        const string MOUSE_X_NAME = "Mouse X";
        const string MOUSE_Y_NAME = "Mouse Y";
        const string MOUSE_Z_NAME = "Mouse ScrollWheel";

        const float MIN_MOUSE_X_DELTA = 0.75f;
        const float MIN_MOUSE_Y_DELTA = 0.75f;
        const float MIN_MOUSE_Z_DELTA = 0.5f; 
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

        #region Structs
        [Serializable]
        public struct InputMapData
        {
            public string Name;
            public InputMapAsset Data;
        } 
        #endregion

        #region Static members
        /// <summary>
        /// Array of not assignable keys during the input assignations.
        /// </summary>
        /// <remarks>Used onlty for keyboard and mouse assignations.</remarks>
        static readonly KeyCode[] NOT_ASSIGNABLE_KEYS = { KeyCode.Escape,
                                                          KeyCode.CapsLock,
                                                          KeyCode.Numlock,
                                                          KeyCode.Break,
                                                          KeyCode.Pause,
                                                          KeyCode.ScrollLock,
                                                          KeyCode.Print,
                                                          KeyCode.SysReq,
                                                          KeyCode.LeftWindows,
                                                          KeyCode.RightWindows,
                                                          KeyCode.Menu,
                                                          KeyCode.LeftApple,
                                                          KeyCode.RightApple,
                                                          KeyCode.LeftCommand,
                                                          KeyCode.RightCommand,
                                                          KeyCode.Help,
                                                          /* Gamepad inputs */
                                                          KeyCode.JoystickButton1,
                                                          KeyCode.JoystickButton2,
                                                          KeyCode.JoystickButton3,
                                                          KeyCode.JoystickButton4,
                                                          KeyCode.JoystickButton5,
                                                          KeyCode.JoystickButton6,
                                                          KeyCode.JoystickButton7,
                                                          KeyCode.JoystickButton8,
                                                          KeyCode.JoystickButton9,
                                                          KeyCode.JoystickButton10,
                                                          KeyCode.JoystickButton11,
                                                          KeyCode.JoystickButton12,
                                                          KeyCode.JoystickButton13,
                                                          KeyCode.JoystickButton14,
                                                          KeyCode.JoystickButton15,
                                                          KeyCode.JoystickButton16,
                                                          KeyCode.JoystickButton17,
                                                          KeyCode.JoystickButton19,
                                                          KeyCode.Joystick1Button1,
                                                          KeyCode.Joystick1Button2,
                                                          KeyCode.Joystick1Button3,
                                                          KeyCode.Joystick1Button4,
                                                          KeyCode.Joystick1Button5,
                                                          KeyCode.Joystick1Button6,
                                                          KeyCode.Joystick1Button7,
                                                          KeyCode.Joystick1Button8,
                                                          KeyCode.Joystick1Button9,
                                                          KeyCode.Joystick1Button10,
                                                          KeyCode.Joystick1Button11,
                                                          KeyCode.Joystick1Button12,
                                                          KeyCode.Joystick1Button13,
                                                          KeyCode.Joystick1Button14,
                                                          KeyCode.Joystick1Button15,
                                                          KeyCode.Joystick1Button16,
                                                          KeyCode.Joystick1Button17,
                                                          KeyCode.Joystick1Button19,
                                                          KeyCode.Joystick2Button1,
                                                          KeyCode.Joystick2Button2,
                                                          KeyCode.Joystick2Button3,
                                                          KeyCode.Joystick2Button4,
                                                          KeyCode.Joystick2Button5,
                                                          KeyCode.Joystick2Button6,
                                                          KeyCode.Joystick2Button7,
                                                          KeyCode.Joystick2Button8,
                                                          KeyCode.Joystick2Button9,
                                                          KeyCode.Joystick2Button10,
                                                          KeyCode.Joystick2Button11,
                                                          KeyCode.Joystick2Button12,
                                                          KeyCode.Joystick2Button13,
                                                          KeyCode.Joystick2Button14,
                                                          KeyCode.Joystick2Button15,
                                                          KeyCode.Joystick2Button16,
                                                          KeyCode.Joystick2Button17,
                                                          KeyCode.Joystick2Button19,
                                                          KeyCode.Joystick3Button1,
                                                          KeyCode.Joystick3Button2,
                                                          KeyCode.Joystick3Button3,
                                                          KeyCode.Joystick3Button4,
                                                          KeyCode.Joystick3Button5,
                                                          KeyCode.Joystick3Button6,
                                                          KeyCode.Joystick3Button7,
                                                          KeyCode.Joystick3Button8,
                                                          KeyCode.Joystick3Button9,
                                                          KeyCode.Joystick3Button10,
                                                          KeyCode.Joystick3Button11,
                                                          KeyCode.Joystick3Button12,
                                                          KeyCode.Joystick3Button13,
                                                          KeyCode.Joystick3Button14,
                                                          KeyCode.Joystick3Button15,
                                                          KeyCode.Joystick3Button16,
                                                          KeyCode.Joystick3Button17,
                                                          KeyCode.Joystick3Button19,
                                                          KeyCode.Joystick4Button1,
                                                          KeyCode.Joystick4Button2,
                                                          KeyCode.Joystick4Button3,
                                                          KeyCode.Joystick4Button4,
                                                          KeyCode.Joystick4Button5,
                                                          KeyCode.Joystick4Button6,
                                                          KeyCode.Joystick4Button7,
                                                          KeyCode.Joystick4Button8,
                                                          KeyCode.Joystick4Button9,
                                                          KeyCode.Joystick4Button10,
                                                          KeyCode.Joystick4Button11,
                                                          KeyCode.Joystick4Button12,
                                                          KeyCode.Joystick4Button13,
                                                          KeyCode.Joystick4Button14,
                                                          KeyCode.Joystick4Button15,
                                                          KeyCode.Joystick4Button16,
                                                          KeyCode.Joystick4Button17,
                                                          KeyCode.Joystick4Button19 };

        /// <summary>
        /// Assignable buttons for any gamepad.
        /// </summary>
        static readonly KeyCode[] ASSIGNABLE_GENERIC_GAMEPAD_BUTTONS = { KeyCode.JoystickButton1,
                                                                         KeyCode.JoystickButton2,
                                                                         KeyCode.JoystickButton3,
                                                                         KeyCode.JoystickButton4,
                                                                         KeyCode.JoystickButton5,
                                                                         KeyCode.JoystickButton6,
                                                                         KeyCode.JoystickButton7,
                                                                         KeyCode.JoystickButton8,
                                                                         KeyCode.JoystickButton9,
                                                                         KeyCode.JoystickButton10,
                                                                         KeyCode.JoystickButton11,
                                                                         KeyCode.JoystickButton12,
                                                                         KeyCode.JoystickButton13,
                                                                         KeyCode.JoystickButton14,
                                                                         KeyCode.JoystickButton15,
                                                                         KeyCode.JoystickButton16,
                                                                         KeyCode.JoystickButton17,
                                                                         KeyCode.JoystickButton19 };

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

        #region Internal vars
        [SerializeField, HideInInspector]
        List<InputMapData> _inputMaps;
        #endregion

        #region Public vars
        [Header("General settings:")]
#if UNITY_CONSOLE
        [HideInInspector]
#endif
        public bool HideMouseCursorInGamepadMode = true;

        [Tooltip("Interval between input type identification checks.")]
#if UNITY_CONSOLE
        [HideInInspector] 
#endif
        public float CheckInputTypeInterval = 0.02f;

#if UNITY_CONSOLE
        [HideInInspector] 
#endif
        public GenericGamepadInputLayoutAsset GenericGamepadSetup;
        public bool EnableGamepadVibration = true;

        [Header("UI settings:")]
        /// <summary>
        /// Time for double click detection on UI controls.
        /// </summary>
#if UNITY_CONSOLE
        [HideInInspector] 
#endif
        public float DoubleClickTime = 0.25f;

#if UNITY_CONSOLE
        [HideInInspector] 
#endif
        [Header("Nintendo Switch Pro controller (PC only):")]
        [Tooltip("Use the Nintendo Switch Pro controller button layout or XBox button layout.\n\nWhen the Nintendo Switch Pro controller is active, this setting allow to use the Nintendo button layout. If this setting is false, uses the XBox button layout (swtich A B buttons, and X Y buttons to match the XBox controller button layout).\n\nThis setting not affect to XBox360/One, PS4 or generic controllers.")]
        public bool UseNintendoButtonLayout = true;

        /// <summary>
        /// Event raised when the input type is changed.
        /// </summary>
        public Action<InputType> OnInputTypeChange;
        #endregion

        #region Properties
        /// <summary>
        /// Return the current input type is received.
        /// </summary>
        public InputType CurrentInputType { get; private set; }

        /// <summary>
        /// Return true if the mouse was move on the current frame.
        /// </summary>
        public bool HasMotionFromMouse { get; private set; }

        /// <summary>
        /// Return true if any key from keyboard or button from mouse is pressed down on current frame.
        /// </summary>
        public bool IsAnyKeyDown { get; private set; }

        public bool IsGamepadVibrationEnable
        {
            get
            {
                return this.EnableGamepadVibration && this.CurrentInputType != InputType.KeyboardAndMouse;
            }

            set
            {
                this.EnableGamepadVibration = value;
            }
        }
        #endregion

        #region Initializers
        private void Awake()
        {
            Gamepad.Instance.TryToIndentifyGamepad();

#if UNITY_DESKTOP
            // Check in defined intervals the current active input:
            StartCoroutine(this.CheckCurrentInputTypeCoroutine()); 

            // Check for a generic joystick and initialize it for support Force Feedback:
            ForceFeedback.CheckForAvailableJoystick();
#endif

            InputManager.Instance = this;

            DontDestroyOnLoad(this);
        }
        #endregion

        #region Update logic
        private void Update()
        {
#if UNITY_DESKTOP
            if (this.CheckInputTypeInterval < 0.01f)
            {
                this.CheckInputTypeInterval = 0.01f;
            }

            if (Gamepad.Instance.UseNintendoButtonLayout != this.UseNintendoButtonLayout)
            {
                Gamepad.Instance.UseNintendoButtonLayout = this.UseNintendoButtonLayout;
            }

            this.HasMotionFromMouse = Mathf.Abs(UnityEngine.Input.GetAxisRaw(InputManager.MOUSE_X_NAME)) > InputManager.MIN_MOUSE_X_DELTA ||
                                      Mathf.Abs(UnityEngine.Input.GetAxisRaw(InputManager.MOUSE_Y_NAME)) > InputManager.MIN_MOUSE_Y_DELTA ||
                                      Mathf.Abs(UnityEngine.Input.GetAxisRaw(InputManager.MOUSE_Z_NAME)) > InputManager.MIN_MOUSE_Z_DELTA;
#endif

            this.IsAnyKeyDown = UnityEngine.Input.anyKeyDown;

            // Process the Gamepad states:
            Gamepad.Instance.Update();

            // Update the input maps:
            for (int i = 0; i < this._inputMaps.Count; i++)
            {
                this._inputMaps[i].Data.Update();
            }
        }
        #endregion

        #region Events
#if UNITY_DESKTOP
        private void OnApplicationQuit()
        {
            this.SetGamepadVibration(Vector2.zero);

            // Release a generic joystick and stop active Force Feedback effect:
            ForceFeedback.ReleaseJoystick();
        } 
#endif
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Get the input map with the desired name.
        /// </summary>
        /// <param name="name">Input map name.</param>
        /// <returns>Return the first ocurrence.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public InputMapAsset GetInputMap(string name)
        {
            for (int i = 0; i < this._inputMaps.Count; i++)
            {
                if (this._inputMaps[i].Name == name)
                {
                    return this._inputMaps[i].Data;
                }
            }

            throw new KeyNotFoundException($"The input map '{name}' not exists.");
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
                        case InputType.KeyboardAndMouse:

                            return InputManager.NOT_ASSIGNABLE_KEYS.Contains(keyCode) ? KeyCode.None : keyCode; 

                        case InputType.XBoxController:

                            return InputManager.ASSIGNABLE_XBOX_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;

                        case InputType.PS4Controller:

                            return InputManager.ASSIGNABLE_PS4_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;

                        case InputType.NintendoSwitchProController:

                            return InputManager.ASSIGNABLE_NINTENDO_SWITCH_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;

                        case InputType.GenericGamepad:

                            return InputManager.ASSIGNABLE_GENERIC_GAMEPAD_BUTTONS.Contains(keyCode) ? keyCode : KeyCode.None;

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
            var wait = new WaitForSecondsRealtime(this.CheckInputTypeInterval);
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

                        Cursor.visible = !this.HideMouseCursorInGamepadMode;
                        Cursor.lockState = this.HideMouseCursorInGamepadMode ? CursorLockMode.Locked : CursorLockMode.None;
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
                        print($"Current Input type: {this.CurrentInputType}");
                    }
                }

                yield return wait;
            }
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(InputManager))]
    public class InputManagerEditor : Editor
    {
        #region Constants
        const string HEADER_NAME = "Input Maps";
        const string PROPERTY_NAME = "_inputMaps";
        const string PREFIX_NAME = "Input Map";

        const string HELPBOX_MESSAGE = "For the right behaviour of the Argos Input Manager, the Unity input settings must be setup first with the Argos input axes predefined values.";
        const string BUTTON_LABEL = "Setup Unity input settings";

        const string DIALOG_TITLE = "Warning!";
        const string DIALOG_MESSAGE = "This action will delete the current Unity input settings axes values. Are you sure?";
        const string DIALOG_OK = "Yes, proceed";
        const string DIALOG_CANCEL = "No, cancel";
        #endregion

        #region Internal vars
        private ReorderableList _inputMapList;
        #endregion

        #region Events
        private void OnEnable()
        {
            this._inputMapList = EditorHelper.CreateNamedList(this, this._inputMapList, InputManagerEditor.HEADER_NAME, InputManagerEditor.PROPERTY_NAME, InputManagerEditor.PREFIX_NAME);
        }

        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(InputManagerEditor.HELPBOX_MESSAGE, MessageType.Info);
            GUI.enabled = !Application.isPlaying;
            if (GUILayout.Button(InputManagerEditor.BUTTON_LABEL))
            {
                if (EditorUtility.DisplayDialog(InputManagerEditor.DIALOG_TITLE, InputManagerEditor.DIALOG_MESSAGE, InputManagerEditor.DIALOG_OK, InputManagerEditor.DIALOG_CANCEL))
                {
                    UnityInputManagerAsset.SetupInputAxes();
                }
            }

            EditorGUILayout.Space();

            this.serializedObject.Update();
            this._inputMapList.DoLayoutList();
            this.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
        }
        #endregion
    } 
#endif
}
