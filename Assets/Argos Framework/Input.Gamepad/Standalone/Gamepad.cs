using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Argos.Framework;
using Argos.Framework.Utils;
using SDL2;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Gamepad desktop implementation based on SDL2.
    /// </summary>
    public class Gamepad : GamepadBase, IDisposable
    {
        #region Constants
        const int MAX_INDEX = 4;  // TODO: Expose as inspector field in Input Manager.
        const float AXIS_DELTA = 0.5f; // TODO: Expose as inspector field in Input Manager.
        #endregion

        #region Internal vars
        bool _disposed;
        IntPtr _gamepad;
        Timer _timer;
        #endregion

        #region Properties
        public string Name { get; private set; } // TODO: Used to get the gamepad type (XBox, PS4, Switch, Steam Controlller...) using the SDL comunity database https://github.com/gabomdq/SDL_GameControllerDB/blob/master/gamecontrollerdb.txt
        public bool IsAttached { get; private set; }
        #endregion

        #region Initializers
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Initialize()
        {
            SDL.SDL_Init(SDL.SDL_INIT_GAMECONTROLLER);
            Application.onBeforeRender += () => { SDL.SDL_GameControllerUpdate(); };
            Application.quitting += () => { SDL.SDL_Quit(); };
        }
        #endregion

        #region Constructor & Destructor
        public Gamepad(int index)
        {
            if (MathUtility.IsClamped(index, 0, MAX_INDEX - 1))
            {
                this.Index = index;
                this._timer = new Timer();
            }
            else
            {
                throw new IndexOutOfRangeException($"Gamepad: The index {index} is out of range [0 - {MAX_INDEX - 1}]");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this.SetVibration(Vector2.zero);
                SDL.SDL_GameControllerClose(this._gamepad);

                this._disposed = true;
            }
        }
        #endregion

        #region Update logic
        public override void Update()
        {
            this.CheckGamepad();

            if (this.IsAttached)
            {
                this.LeftStick = new Vector2(this.ReadAxis(SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX), this.ReadAxis(SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY));
                this.RightStick = new Vector2(this.ReadAxis(SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX), this.ReadAxis(SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY));

                this.LeftTrigger = this.ReadAxis(SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT);
                this.RightTrigger = this.ReadAxis(SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT);

                this.A.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A));
                this.B.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B));
                this.X.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X));
                this.Y.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y));

                this.LeftStickButton.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSTICK));
                this.RightStickButton.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSTICK));

                this.Start.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START));
                this.Back.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK));

                this.LeftBumper.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER));
                this.RightBumper.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER));

                this.LeftTriggerButton.SetState(this.LeftTrigger >= AXIS_DELTA);
                this.RightTriggerButton.SetState(this.RightTrigger >= AXIS_DELTA);

                this.DPadLeft.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_LEFT));
                this.DPadUp.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP));
                this.DPadRight.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT));
                this.DPadDown.SetState(this.ReadButton(SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_DOWN));

                if (this.DPadLeft.IsPressed || this.DPadLeft.IsDown)
                {
                    this.DPad = Vector2.left;
                }
                else if (this.DPadUp.IsPressed || this.DPadUp.IsDown)
                {
                    this.DPad = Vector2.up;
                }
                else if (this.DPadRight.IsPressed || this.DPadRight.IsDown)
                {
                    this.DPad = Vector2.right;
                }
                else if (this.DPadDown.IsPressed || this.DPadDown.IsDown)
                {
                    this.DPad = Vector2.down;
                }
                else
                {
                    this.DPad = Vector2.zero;
                }
            }
        }
        #endregion

        #region Methods & Functions
        float ReadAxis(SDL.SDL_GameControllerAxis axis)
        {
            return SDL.SDL_GameControllerGetAxis(this._gamepad, axis) / ushort.MaxValue;
        }

        bool ReadButton(SDL.SDL_GameControllerButton button)
        {
            return (SDL.SDL_GameControllerGetButton(this._gamepad, SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A) == 1);
        }

        public override void SetVibration(Vector2 force)
        {
            if (this.IsAttached && !MathUtility.CompareVector(force, this.Vibration))
            {
                this.Vibration = force;
                SDL.SDL_GameControllerRumble(this._gamepad, (ushort)(ushort.MaxValue * Mathf.Clamp01(force.x)), (ushort)(ushort.MaxValue * Mathf.Clamp01(force.y)), 0);
            }
        }

        void CheckGamepad()
        {
            if (this._timer.Value > 0.002f)  // TODO: Expose as inspector field in Input Manager.
            {
                this._timer.Reset();

                if (!this.IsAttached || this._gamepad == IntPtr.Zero)
                {
                    if (SDL.SDL_IsGameController(this.Index) == SDL.SDL_bool.SDL_TRUE)
                    {
                        this._gamepad = SDL.SDL_GameControllerOpen(this.Index);
                        this.IsAttached = (this._gamepad != IntPtr.Zero);
                        this.Name = this.IsAttached ? SDL.SDL_GameControllerName(this._gamepad) : string.Empty;
                    }
                }
                else
                {
                    this.IsAttached = (SDL.SDL_GameControllerGetAttached(this._gamepad) == SDL.SDL_bool.SDL_TRUE);
                }
            }
        }
        #endregion
    }
}