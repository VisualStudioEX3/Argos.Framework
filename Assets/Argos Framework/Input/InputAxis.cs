using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Utils;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Virtual Input Axis.
    /// </summary>
    /// <remarks>Simulated an input axis with the capacity of setup input actions for each axis direction, and support predefined axes for mouse input and gamepad axes.</remarks>
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
        /// <remarks>Uses built-in axis setup as alternate input (read after the custom input setup).</remarks>
        public InputAxisType axisType;

        /// <summary>
        /// Invert Y axis.
        /// </summary>
        public bool invertYAxis;

        /// <summary>
        /// Axis sensitivity factor.
        /// </summary>
        /// <remarks>Not apply on Xbox/PS4/Nintendo Switch controllers axes.</remarks>
        [Range(InputAxis.MIN_SENSITIVITY, InputAxis.MAX_SENSITIVITY)]
        public float sensitivity = InputAxis.DEFAULT_SENSITIVITY;

        /// <summary>
        /// Indicate if this axis uses the mouse input (axis or position) when the gamepad is not is the active input.
        /// </summary>
        /// <remarks>When this property is not setup as None, this axis ignore all Keys fields (InputActions) configured.</remarks>
        [HelpBox("Use mouse input when the gamepad is not the active input:"), Space]
        public MouseInputMode alternativeMouseInput = MouseInputMode.None;

        /// <summary>
        /// Indicate if this input is using in UI.
        /// </summary>
        /// <remarks>This uses the absolute value of the axis, without applying sensivity.</remarks>
        [HelpBox("Uses the absolute value of the axis, without applying sensivity:"), Space]
        public bool isUIInput;
        
        /// <summary>
        /// Normalize axis.
        /// </summary>
        /// <remarks>Use this property to fix the diagonal ranges on 360º/free movements.</remarks>
        [HelpBox("Normalize the diagonal ranges:"), Space]
        public bool normalize;

        /// <summary>
        /// Left input action (-1 to 0 in X axis).
        /// </summary>
        [Header("Keys:")]
        public InputAction left;

        /// <summary>
        /// Right input action (0 to 1 in X axis).
        /// </summary>
        public InputAction right;

        /// <summary>
        /// Up input action (-1 to 0 in Y axis).
        /// </summary>
        public InputAction up;

        /// <summary>
        /// Down input action (0 to 1 in Y axis).
        /// </summary>
        public InputAction down;
        
        [Header("Print debug info:")]
        public bool debug;
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
        public static implicit operator Vector2(InputAxis value)
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

            this.left = leftKey;
            this.right = rightKey;
            this.down = downKey;
            this.up = upKey;
            this.alternativeMouseInput = alternativeMouseInput;
            this.isUIInput = isUIInput;
            this.sensitivity = sensitivity;
            this.axisType = axisType;
            this.invertYAxis = invertY;
            this.normalize = normalize;
            this.debug = debug;
        }

        /// <summary>
        /// Constructor copy.
        /// </summary>
        /// <param name="instance">Previous instance of an InputAxis.</param>
        /// <remarks>Use this to fast clone instance.</remarks>
        public InputAxis(InputAxis instance) : this(new InputAction(instance.left), new InputAction(instance.right), new InputAction(instance.down), new InputAction(instance.up), instance.axisType, instance.alternativeMouseInput, instance.isUIInput, instance.sensitivity, instance.invertYAxis, instance.normalize, instance.debug)
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

            switch (this.axisType)
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
                if (this.alternativeMouseInput != MouseInputMode.None)
                {
                    if (this.alternativeMouseInput == MouseInputMode.MousePosition)
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
                    this.left.Update();
                    this.right.Update();
                    this.up.Update();
                    this.down.Update();

                    this._target.x = this.left.State ? -1f : this.right.State ? 1f : 0f;
                    this._target.y = this.down.State ? -1f : this.up.State ? 1f : 0f;

                    // For the right behaviour, the ActionInputs KeyEvent property must be setted as Down:
                    this.AxisKeyDown = new Vector2()
                    {
                        x = this.left.State || this.right.State ? 1f : 0f,
                        y = this.up.State || this.down.State ? 1f : 0f
                    };
                }
            }

            if (!this.isUIInput)
            {
                float time = Time.unscaledDeltaTime * this.sensitivity;
                this._axis.x = Mathf.Lerp(this._axis.x, this._target.x, time);
                this._axis.y = Mathf.Lerp(this._axis.y, this._target.y, time);

                if (Utils.MathUtility.CompareVector(this._axis, Vector2.zero, 0.001f))
                {
                    this._axis = Vector2.zero;
                }
            }
            else
            {
                this._axis = this._target;
            }

            if (this.invertYAxis)
            {
                this._axis.y *= -1;
            }

            if (this.normalize)
            {
                if (this._axis.sqrMagnitude > 1f)
                {
                    this._axis.Normalize();
                }
            }

            if (this.debug)
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
            return $"{this._axis.ToString()} (Axis KeyDown: {this.AxisKeyDown.ToString()}) - Type: {this.axisType} Left key: {this.left.main}/{this.left.alternative}/{this.left.gamepadButton}, Right key: {this.right.main}/{this.right.alternative}/{this.right.gamepadButton}, Up key: {this.up.main}/{this.up.alternative}/{this.up.gamepadButton}, Down key: {this.down.main}/{this.down.alternative}/{this.down.gamepadButton}, Is UI Input: {this.isUIInput}, Sensitivity: {this.sensitivity}, Invert Y: {this.invertYAxis}, Normalize: {this.normalize}";
        }
        #endregion
    }
}