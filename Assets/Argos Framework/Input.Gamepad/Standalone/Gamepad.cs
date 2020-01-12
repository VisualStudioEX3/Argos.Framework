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
        const int MAX_INDEX = 4;
        #endregion

        #region Internal vars
        bool _disposed;
        IntPtr _gamepad;
        Timer _timer;
        #endregion

        #region Properties
        public bool IsAttached { get; private set; }
        #endregion

        #region Initializers
        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            SDL.SDL_Init(SDL.SDL_INIT_GAMECONTROLLER);
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

        ~Gamepad()
        {
            this.Dispose(false);
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
                SDL.SDL_GameControllerUpdate(); // TODO: Try to find a way to call this method only one time for all gamepads.

                // TODO: Read values for all axes and buttons.
            }
        }
        #endregion

        #region Methods & Functions
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
            if (this._timer.Value > 0.002f) // TODO: Setup field on InputManager to define this value.
            {
                this._timer.Reset();

                if (!this.IsAttached || this._gamepad == IntPtr.Zero)
                {
                    if (SDL.SDL_IsGameController(this.Index) == SDL.SDL_bool.SDL_TRUE)
                    {
                        this._gamepad = SDL.SDL_GameControllerOpen(this.Index);
                        this.IsAttached = (this._gamepad != IntPtr.Zero);
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