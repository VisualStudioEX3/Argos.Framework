using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Argos.Framework;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Virtual Input Axis.
    /// </summary>
    /// <remarks>Simulated a input axis with the capacity of setup input actions for each axis direction, and support predefined axes for mouse input and gamepad axes.</remarks>
    [Serializable]
    public class InputAxis
    {
        #region Constants
		const string MOUSE_X_NAME = "Mouse X";
		const string MOUSE_Y_NAME = "Mouse Y";
        const float MIN_SENSITIVITY = 0.5f;
        const float MAX_SENSITIVITY = 30f;
        const float DEFAULT_SENSITIVITY = 10f;
        #endregion

        #region Enums
        /// <summary>
        /// Input axis type.
        /// </summary>
        /// <remarks>Reference to custom setup, mouse or standard gamepad axis.</remarks>
        public enum InputAxisType
        {
            /// <summary>
            /// None. Uses only the custom keys.
            /// </summary>
            None,
            /// <summary>
            /// Mouse axis.
            /// </summary>
            MouseAxis,
            /// <summary>
            /// Gamepad left stick.
            /// </summary>
            GamepadLeftStick,
            /// <summary>
            /// Gamepad right stick.
            /// </summary>
            GamepadRightStick,
            /// <summary>
            /// Gamepad directional pad.
            /// </summary>
            GamepadDPad
        }

        public enum MouseInputMode
        {
            None,
            MousePosition,
            MouseAxis
        }
        #endregion

        #region Internal vars
        Vector2 _axis;
        Vector2 _target;
        #endregion

        #region Public vars
        /// <summary>
        /// Axis type.
        /// </summary>
        /// <remarks>Uses prebuild axis setup as alternate input (read after the custom input setup).</remarks>
        public InputAxisType AxisType;

        /// <summary>
        /// Invert Y axis.
        /// </summary>
        public bool InvertYAxis;

        /// <summary>
        /// Axis sensitivity factor.
        /// </summary>
        /// <remarks>Not apply on Xbox/PS4/Nintendo Switch controllers axes.</remarks>
        [Range(InputAxis.MIN_SENSITIVITY, InputAxis.MAX_SENSITIVITY)]
        public float Sensitivity = InputAxis.DEFAULT_SENSITIVITY;

        /// <summary>
        /// Indicate if this axis uses the mouse input (axis or position) when the gamepad is not is the active input.
        /// </summary>
        /// <remarks>When this property is not setup as None, this axis ignore all Keys fields (InputActions) configured.</remarks>
        [HelpBox("Use mouse input when the gamepad is not the active input:"), Space]
        public MouseInputMode AlternativeMouseInput = MouseInputMode.None;

        /// <summary>
        /// Indicate if this input is using in UI.
        /// </summary>
        /// <remarks>This uses the absolute value of the axis, without applying sensivity.</remarks>
        [HelpBox("Uses the absolute value of the axis, without applying sensivity:"), Space]
        public bool IsUIInput;
        
        /// <summary>
        /// Normalize axis.
        /// </summary>
        /// <remarks>Use this property to fix the diagonal ranges on 360º/free movements.</remarks>
        [HelpBox("Normalize the diagonal ranges:"), Space]
        public bool Normalize;

        /// <summary>
        /// Left input action (-1 to 0 in X axis).
        /// </summary>
        [Header("Keys:")]
        public InputAction Left;

        /// <summary>
        /// Right input action (0 to 1 in X axis).
        /// </summary>
        public InputAction Right;

        /// <summary>
        /// Up input action (-1 to 0 in Y axis).
        /// </summary>
        public InputAction Up;

        /// <summary>
        /// Down input action (0 to 1 in Y axis).
        /// </summary>
        public InputAction Down;
        
        [Header("Print debug info:")]
        public bool Debug;
        #endregion

        #region Properties
        /// <summary>
        /// Horizontal axis.
        /// </summary>
        public float X { get { return this._axis.x; } }

        /// <summary>
        /// Vertical axis.
        /// </summary>
        public float Y { get { return this._axis.y; } }

        /// <summary>
        /// Return the axis where keydown event, from keyboard only, is raised.
        /// </summary>
        /// <remarks>Used by UI navigation system.</remarks>
        public Vector2 AxisKeyDown { get; private set; }
        #endregion

        #region Operators
        /// <summary>
        /// Cast to Vector2.
        /// </summary>
        public static explicit operator Vector2(InputAxis value)
        {
            return new Vector2(value.X, value.Y);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="leftKey">Left input action (-1 to 0 in X axis).</param>
        /// <param name="rightKey">Right input action (0 to 1 in X axis).</param>
        /// <param name="downKey">Down input action (-1 to 0 in Y axis).</param>
        /// <param name="upKey">Up input action (0 to 1 in Y axis).</param>
        /// <param name="axisType">Predefined gamepad axis.</param>
        /// <param name="alternativeMouseInput">Alternative axis setup using the mouse. Uses when the axis type is any gamepad axis and want to uses the mouse when the active input is keyboard and mouse.</param>
        /// <param name="isUIInput">Indicates if this axis is for UI only.</param>
        /// <param name="sensitivity">Axis sensitivity (from 0.5 to 30, default is 10).</param>
        /// <param name="invertY">Invert Y axis.</param>
        /// <param name="normalize">Normalize the axis on 360º/free movements.</param>
        /// <param name="debug"></param>
        public InputAxis(InputAction leftKey, InputAction rightKey, InputAction downKey, InputAction upKey, InputAxisType axisType = InputAxisType.GamepadLeftStick, MouseInputMode alternativeMouseInput = MouseInputMode.None, bool isUIInput = false, float sensitivity = InputAxis.DEFAULT_SENSITIVITY, bool invertY = false, bool normalize = false, bool debug = false)
        {
            this._axis = this._target = this.AxisKeyDown = Vector2.zero;

            this.Left = leftKey;
            this.Right = rightKey;
            this.Down = downKey;
            this.Up = upKey;
            this.AlternativeMouseInput = alternativeMouseInput;
            this.IsUIInput = isUIInput;
            this.Sensitivity = sensitivity;
            this.AxisType = axisType;
            this.InvertYAxis = invertY;
            this.Normalize = normalize;
            this.Debug = debug;
        }

        /// <summary>
        /// Constructor copy.
        /// </summary>
        /// <param name="instance">Previous instance of an InputAxis.</param>
        /// <remarks>Use this to fast clone instance.</remarks>
        public InputAxis(InputAxis instance) : this(new InputAction(instance.Left), new InputAction(instance.Right), new InputAction(instance.Down), new InputAction(instance.Up), instance.AxisType, instance.AlternativeMouseInput, instance.IsUIInput, instance.Sensitivity, instance.InvertYAxis, instance.Normalize, instance.Debug)
        {

        }
        #endregion

        #region Update logic
        /// <summary>
        /// Update logic for the virtual axis.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Update()
        {
            this.AxisKeyDown = Vector2.zero;

            switch (this.AxisType)
            {
                case InputAxisType.MouseAxis:

                    this._target = this.GetMouseAxis();
                    break;

                case InputAxisType.GamepadLeftStick:

                    this._target = Gamepad.Instance.LeftStick;
                    break;

                case InputAxisType.GamepadRightStick:

                    this._target = Gamepad.Instance.RightStick;
                    break;

                case InputAxisType.GamepadDPad:

                    this._target = Gamepad.Instance.DPad;
                    break;
            }

            if (this._target == Vector2.zero)
            {
                if (this.AlternativeMouseInput != MouseInputMode.None)
                {
                    if (this.AlternativeMouseInput == MouseInputMode.MousePosition)
                    {
                        this._axis = UnityEngine.Input.mousePosition;
                    }
                    else
                    {
                        this._axis = this.GetMouseAxis();
                    }
                }
                else
                {
                    this.Left.Update();
                    this.Right.Update();
                    this.Up.Update();
                    this.Down.Update();

                    this._target.x = this.Left.State ? -1f : this.Right.State ? 1f : 0f;
                    this._target.y = this.Down.State ? -1f : this.Up.State ? 1f : 0f;

                    // For the right behaviour, the ActionInputs KeyEvent property must be setted as Down:
                    this.AxisKeyDown = new Vector2()
                    {
                        x = this.Left.State || this.Right.State ? 1f : 0f,
                        y = this.Up.State || this.Down.State ? 1f : 0f
                    };
                }
            }

            if (!this.IsUIInput)
            {
                float time = Time.unscaledDeltaTime * this.Sensitivity;
                this._axis.x = Mathf.Lerp(this._axis.x, this._target.x, time);
                this._axis.y = Mathf.Lerp(this._axis.y, this._target.y, time);

                if (Helper.CompareVector(this._axis, Vector2.zero, 0.001f))
                {
                    this._axis = Vector2.zero;
                }
            }
            else
            {
                this._axis = this._target;
            }

            if (this.InvertYAxis)
            {
                this._axis.y *= -1;
            }

            if (this.Normalize)
            {
                if (this._axis.sqrMagnitude > 1f)
                {
                    this._axis.Normalize();
                }
            }

            if (this.Debug)
            {
                UnityEngine.Debug.Log(this.ToString());
            }
        }
        #endregion

        #region Methods & Functions
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        Vector2 GetMouseAxis()
        {
            return new Vector2(UnityEngine.Input.GetAxisRaw(InputAxis.MOUSE_X_NAME), UnityEngine.Input.GetAxisRaw(InputAxis.MOUSE_Y_NAME));
        }

        public override string ToString()
        {
            return $"{this._axis.ToString()} (Axis KeyDown: {this.AxisKeyDown.ToString()}) - Type: {this.AxisType} Left key: {this.Left.Main}/{this.Left.Alternative}/{this.Left.GamepadButton}, Right key: {this.Right.Main}/{this.Right.Alternative}/{this.Right.GamepadButton}, Up key: {this.Up.Main}/{this.Up.Alternative}/{this.Up.GamepadButton}, Down key: {this.Down.Main}/{this.Down.Alternative}/{this.Down.GamepadButton}, Is UI Input: {this.IsUIInput}, Sensitivity: {this.Sensitivity}, Invert Y: {this.InvertYAxis}, Normalize: {this.Normalize}";
        }
        #endregion
    }
}