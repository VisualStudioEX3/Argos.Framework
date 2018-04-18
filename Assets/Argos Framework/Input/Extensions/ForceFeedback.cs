#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#define ENABLE_FORCE_FEEDBACK_SUPPORT
#endif

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
#if ENABLE_FORCE_FEEDBACK_SUPPORT
using System.Runtime.InteropServices;
using SharpDX.DirectInput;
#endif

namespace Argos.Framework.Input.Extensions
{
    /// <summary>
    /// DirectInput8 Force Feedback support via SharpDX wrapper.
    /// </summary>
    /// <remarks>SharpDX: http://sharpdx.org/
    /// WARNING: You need to install the joystick driver to enable the force feedback support.</remarks>
    public static class ForceFeedback
    {
#if ENABLE_FORCE_FEEDBACK_SUPPORT
        #region Internal vars
        static DirectInput _directInput;
        static IList<DeviceInstance> _devices;
        static Joystick _joystick;
        static Effect _effect;
        static EffectParameters _effectParams;
        #endregion

        #region DLL Imports
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        #endregion

        #region Constructors
        static ForceFeedback()
        {
            ForceFeedback._directInput = new DirectInput();
            ForceFeedback._effectParams = new EffectParameters
            {
                Parameters = new SharpDX.DirectInput.ConstantForce(),
                Duration = -1,
                Gain = 10000,
                TriggerButton = -1,
                Flags = EffectFlags.ObjectOffsets | EffectFlags.Cartesian,
                Axes = new int[] { (int)JoystickOffset.X, (int)JoystickOffset.Y },
                Directions = new int[2]
            };
        }
        #endregion
#endif

        #region Methods & Functions
        /// <summary>
        /// Check for available joysticks and setup the first joystick found if is needed.
        /// </summary>
        /// <returns>Return true if a joystick is found and is initialized.</returns>
        public static bool CheckForAvailableJoystick()
        {
#if ENABLE_FORCE_FEEDBACK_SUPPORT
            ForceFeedback._devices = ForceFeedback._directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly | DeviceEnumerationFlags.ForceFeedback);

            if (ForceFeedback._devices.Count > 0)
            {
                ForceFeedback.ReleaseJoystick();

                ForceFeedback._joystick = new Joystick(ForceFeedback._directInput, ForceFeedback._devices[0].InstanceGuid);
                ForceFeedback._joystick.SetCooperativeLevel(ForceFeedback.GetActiveWindow(), CooperativeLevel.Exclusive | CooperativeLevel.Background);
                ForceFeedback._joystick.Acquire();

                ForceFeedback._effect = new Effect(ForceFeedback._joystick, EffectGuid.ConstantForce, ForceFeedback._effectParams);
                ForceFeedback._effect.Start();

                return true;
            }

            return false;
#endif
        }

        /// <summary>
        /// Release the joystick if already is setup.
        /// </summary>
        /// <remarks>Call this method at the end of the program.</remarks>
        public static void ReleaseJoystick()
        {
#if ENABLE_FORCE_FEEDBACK_SUPPORT
            ForceFeedback._effect?.Stop();
            ForceFeedback._effect?.Dispose();
            ForceFeedback._effect = null;

            ForceFeedback._joystick?.Unacquire();
            ForceFeedback._joystick?.Dispose();
            ForceFeedback._joystick = null; 
#endif
        }

        /// <summary>
        /// Set the vibrator forces values.
        /// </summary>
        /// <param name="axes">Force Feedback axis forces. Values per axis go from 0 to 1.</param>
        /// <remarks>Return true if the joystick has conected.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool SetVibration(Vector2 axes)
        {
#if ENABLE_FORCE_FEEDBACK_SUPPORT
            if (ForceFeedback._joystick != null)
            {
                int x = (int)(axes.x * ForceFeedback._effectParams.Gain);
                int y = (int)(axes.y * ForceFeedback._effectParams.Gain);

                (ForceFeedback._effectParams.Parameters as SharpDX.DirectInput.ConstantForce).Magnitude = (int)Mathf.Sqrt(x * x + y * y);
                ForceFeedback._effectParams.Directions = new int[] { Mathf.CeilToInt(axes.x), Mathf.CeilToInt(axes.y) };

                ForceFeedback._effect.SetParameters(ForceFeedback._effectParams, EffectParameterFlags.Direction | EffectParameterFlags.TypeSpecificParameters | EffectParameterFlags.Start);

                return true;
            } 
#endif
            return false;
        } 
        #endregion
    }
}