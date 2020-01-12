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
    /// Gamepad buttons (used XBox360 button names).
    /// </summary>
    public enum GamepadButtons
    {
        None = -1,
        A,
        B,
        X,
        Y,
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

    #region Structs
    /// <summary>
    /// Button states.
    /// </summary>
    /// <remarks>Uses to ease get the button states and to virtualize axis states (triggers, DPad) as button states.</remarks>
    public struct ButtonStates
    {
        #region protected vars
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

            internal set
            {
                this._isDown = this._isUp = this._isPressed;
                this._isPressed = value;
            }
        }
        public bool IsDown => !this._isDown && this._isPressed;
        public bool IsUp => this._isUp && !this._isPressed;
        #endregion
    }
    #endregion

    #region Classes
    public abstract class GamepadBase : IDisposable
    {
        #region Properties
        public int Index { get; protected set; }

        public Vector2 LeftStick { get; protected set; }
        public Vector2 RightStick { get; protected set; }
        public Vector2 LeftTrigger { get; protected set; }
        public Vector2 RightTrigger { get; protected set; }
        public Vector2 DPad { get; protected set; }

        public ButtonStates A { get; protected set; }
        public ButtonStates B { get; protected set; }
        public ButtonStates X { get; protected set; }
        public ButtonStates Y { get; protected set; }

        public ButtonStates Start { get; protected set; }
        public ButtonStates Select { get; protected set; }

        public ButtonStates LeftBumper { get; protected set; }
        public ButtonStates RightBumper { get; protected set; }

        public ButtonStates LeftTriggerButton { get; protected set; }
        public ButtonStates RightTriggerButton { get; protected set; }

        public ButtonStates LeftStickButton { get; protected set; }
        public ButtonStates RightStickButton { get; protected set; }

        public ButtonStates DPadLeft { get; protected set; }
        public ButtonStates DPadRight { get; protected set; }
        public ButtonStates DPadUp { get; protected set; }
        public ButtonStates DPadDown { get; protected set; }

        public bool IsAnyButtonPressed =>   this.A.IsPressed ||
                                            this.B.IsPressed ||
                                            this.X.IsPressed ||
                                            this.Y.IsPressed ||
                                            this.Start.IsPressed ||
                                            this.Select.IsPressed ||
                                            this.LeftBumper.IsPressed ||
                                            this.RightBumper.IsPressed ||
                                            this.LeftTriggerButton.IsPressed ||
                                            this.RightTriggerButton.IsPressed ||
                                            this.LeftStickButton.IsPressed ||
                                            this.RightStickButton.IsPressed ||
                                            this.DPadLeft.IsPressed ||
                                            this.DPadRight.IsPressed ||
                                            this.DPadUp.IsPressed ||
                                            this.DPadDown.IsPressed;

        public bool IsAnyButtonDown =>      this.A.IsDown ||
                                            this.B.IsDown ||
                                            this.X.IsDown ||
                                            this.Y.IsDown ||
                                            this.Start.IsDown ||
                                            this.Select.IsDown ||
                                            this.LeftBumper.IsDown ||
                                            this.RightBumper.IsDown ||
                                            this.LeftTriggerButton.IsDown ||
                                            this.RightTriggerButton.IsDown ||
                                            this.LeftStickButton.IsDown ||
                                            this.RightStickButton.IsDown ||
                                            this.DPadLeft.IsDown ||
                                            this.DPadRight.IsDown ||
                                            this.DPadUp.IsDown ||
                                            this.DPadDown.IsDown;

        public bool IsAnyButtonUp =>        this.A.IsUp ||
                                            this.B.IsUp ||
                                            this.X.IsUp ||
                                            this.Y.IsUp ||
                                            this.Start.IsUp ||
                                            this.Select.IsUp ||
                                            this.LeftBumper.IsUp ||
                                            this.RightBumper.IsUp ||
                                            this.LeftTriggerButton.IsUp ||
                                            this.RightTriggerButton.IsUp ||
                                            this.LeftStickButton.IsUp ||
                                            this.RightStickButton.IsUp ||
                                            this.DPadLeft.IsUp ||
                                            this.DPadRight.IsUp ||
                                            this.DPadUp.IsUp ||
                                            this.DPadDown.IsUp;

        public bool HasMotionFromAnyAxis => (this.LeftStick + this.RightStick + this.DPad) != Vector2.zero;

        public Vector2 Vibration { get; protected set; }
        #endregion

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        #region Update logic
        public abstract void Update(); 
        #endregion

        #region Methods & Functions
        public abstract void SetVibration(Vector2 force);

        public void SetVibration(float left, float right)
        {
            this.SetVibration(new Vector2(left, right));
        } 
        #endregion
    }
    #endregion

    /// <summary>
    /// Gamepad manager base implementation.
    /// </summary>
    /// <remarks>Use this class to define custom platform implementation.</remarks>
    public abstract class GamepadManagerBase
    {
        #region Properties
        public GamepadBase[] Gamepads { get; private set; } 
        #endregion

        #region Update logic
        /// <summary>
        /// Update gamepad values.
        /// </summary>
        public virtual void Update()
        {
            this.ReadLeftStick();
            this.ReadRightStick();
            this.ReadDPad();
            this.ReadTriggers();
            this.ReadButtons();
        }
        #endregion

        #region Methods & Functions
        public abstract void ReadButtons();

        public abstract void ReadLeftStick();

        public abstract void ReadRightStick();

        public abstract void ReadDPad();

        public abstract void ReadTriggers();


        #endregion
    }
}