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
        Back,
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

    ///// <summary>
    ///// Gamepad controller type.
    ///// </summary>
    //public enum GamepadType // TODO: Maybe add specific versions: Xbox360, Xbox One, PS3, PS4, Nintendo Switch Pro, Nintendo Switch Joycon, etc...
    //{
    //    /// <summary>
    //    /// Xbox360 / Xbox One game controller.
    //    /// </summary>
    //    XBoxController,
    //    /// <summary>
    //    /// PS3/PS4 game controller.
    //    /// </summary>
    //    PlayStationController,
    //    /// <summary>
    //    /// Nintendo Switch Pro controller.
    //    /// </summary>
    //    NintendoSwitchProController,
    //    /// <summary>
    //    /// Steam Controller.
    //    /// </summary>
    //    SteamController
    //}
    #endregion

    #region Structs
    /// <summary>
    /// Button states.
    /// </summary>
    /// <remarks>Uses to ease get the button states and to virtualize axis states (triggers, DPad) as button states.</remarks>
    public struct ButtonStates
    {
        #region Internal vars
        bool _isDown;
        bool _isUp;
        #endregion

        #region Properties
        public bool IsPressed { get; private set; }
        public bool IsDown => !this._isDown && this.IsPressed;
        public bool IsUp => this._isUp && !this.IsPressed;
        #endregion

        #region Methods & Functions
        public void SetState(bool state)
        {
            this._isDown = this._isUp = this.IsPressed;
            this.IsPressed = state;
        } 
        #endregion
    }
    #endregion

    #region Classes
    public abstract class GamepadBase : IDisposable
    {
        #region Properties
        public int Index { get; protected set; }
        public GamepadType Type { get; protected set; }

        public Vector2 LeftStick { get; protected set; }
        public Vector2 RightStick { get; protected set; }
        public float LeftTrigger { get; protected set; }
        public float RightTrigger { get; protected set; }
        public Vector2 DPad { get; protected set; }

        public ButtonStates A { get; protected set; }
        public ButtonStates B { get; protected set; }
        public ButtonStates X { get; protected set; }
        public ButtonStates Y { get; protected set; }

        public ButtonStates Start { get; protected set; }
        public ButtonStates Back { get; protected set; }

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

        public bool IsAnyButtonPressed => this.A.IsPressed ||
                                            this.B.IsPressed ||
                                            this.X.IsPressed ||
                                            this.Y.IsPressed ||
                                            this.Start.IsPressed ||
                                            this.Back.IsPressed ||
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

        public bool IsAnyButtonDown => this.A.IsDown ||
                                            this.B.IsDown ||
                                            this.X.IsDown ||
                                            this.Y.IsDown ||
                                            this.Start.IsDown ||
                                            this.Back.IsDown ||
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

        public bool IsAnyButtonUp => this.A.IsUp ||
                                            this.B.IsUp ||
                                            this.X.IsUp ||
                                            this.Y.IsUp ||
                                            this.Start.IsUp ||
                                            this.Back.IsUp ||
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

        #region Destructor
        ~GamepadBase()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
        #endregion

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
}