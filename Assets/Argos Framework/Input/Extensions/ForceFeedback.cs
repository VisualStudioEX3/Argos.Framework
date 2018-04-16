#if ENABLE_FORCE_FEEDBACK_SUPPORT && (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UnityEngine;
using SharpDX.DirectInput;

namespace Argos.Framework.Input
{
    /// <summary>
    /// DirectInput Force Feedback support via SharpDX wrapper.
    /// </summary>
    /// <remarks>WARNING: For use this feature you must add SharpDX DLLs to Unity project.
    /// Also need to install the gamepad drivers to enable force feedback support in hardware.</remarks>
    public static class ForceFeedback
    {
        #region Internal vars
        static DirectInput _directInput;
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

        #region Methods & Functions
        public static void SetupJoystick()
        {
            var devices = ForceFeedback._directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly | DeviceEnumerationFlags.ForceFeedback);

            if (devices.Count > 0)
            {
                ForceFeedback._joystick = new Joystick(ForceFeedback._directInput, devices[0].InstanceGuid);
                ForceFeedback._joystick.SetCooperativeLevel(ForceFeedback.GetActiveWindow(), CooperativeLevel.Exclusive | CooperativeLevel.Background);
                ForceFeedback._joystick.Acquire();
                ForceFeedback._effect = new Effect(ForceFeedback._joystick, EffectGuid.ConstantForce, ForceFeedback._effectParams);
            }
        }

        public static void Release()
        {
            if (ForceFeedback._joystick != null)
            {
                ForceFeedback._effect.Stop();
                ForceFeedback._joystick.Unacquire();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axes"></param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void SetVibration(Vector2 axes)
        {
            if (ForceFeedback._joystick != null) // Find way to check if joysticks in disconected.
            {
                if (axes == Vector2.zero)
                {
                    ForceFeedback._effect.Stop();
                }
                else
                {
                    int x = (int)(axes.x * ForceFeedback._effectParams.Gain);
                    int y = (int)(axes.y * ForceFeedback._effectParams.Gain);

                    (ForceFeedback._effectParams.Parameters as SharpDX.DirectInput.ConstantForce).Magnitude = (int)Mathf.Sqrt(x * x + y * y);
                    ForceFeedback._effectParams.Directions = new int[] { Mathf.CeilToInt(axes.x), Mathf.CeilToInt(axes.y) };

                    ForceFeedback._effect.SetParameters(ForceFeedback._effectParams, EffectParameterFlags.Direction | EffectParameterFlags.TypeSpecificParameters | EffectParameterFlags.Start);

                    if (ForceFeedback._joystick.GetForceFeedbackState() == ForceFeedbackState.Stopped)
                    {
                        ForceFeedback._effect.Start();
                    }
                }
            }
        } 
        #endregion
    }
}
#endif