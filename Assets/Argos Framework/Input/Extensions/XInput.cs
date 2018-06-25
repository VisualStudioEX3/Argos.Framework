#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#define ENABLE_XINPUT_SUPPORT
#endif

using System.Runtime.CompilerServices;
using UnityEngine;
#if ENABLE_XINPUT_SUPPORT
using System.Runtime.InteropServices;
#endif

namespace Argos.Framework.Input.Extensions
{
    /// <summary>
    /// Microsoft XInput interface used for manage the XBox360/XBox One controller vibrators.
    /// </summary>
    /// <remarks>This class implemented code for desktop and UWP versions. The UWP code bring support for trigger vibrators (only for builds, not from editor).</remarks>
    public static class XInput
    {
#if ENABLE_XINPUT_SUPPORT

        #region Constants
        const ushort MAX_VIBRATION_LEVEL = 65000;
        #endregion

        #region Variables
        static SharpDX.XInput.Controller _gamepad;
        static SharpDX.XInput.Vibration _vibratorsSetup; 
        #endregion

#elif ENABLE_WINMD_SUPPORT

        #region Variables
        static Windows.Gaming.Input.GamepadVibration _vibrationSetup = new Windows.Gaming.Input.GamepadVibration(); 
        #endregion

#endif

        #region Methods & Functions
        /// <summary>
        /// Set vibrators force values.
        /// </summary>
        /// <param name="engines">Axis force for engines (x = left engine, y = right engine). Axis values from 0 to 1. -1 keeps the current force value.</param>
        /// <returns>Return true if a Xbox360/XBox One controller or compatible is conected and available as first player.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool SetVibration(Vector2 engines)
        {
#if ENABLE_XINPUT_SUPPORT // Desktop:

            if (engines.x >= 0f)
            {
                XInput._vibratorsSetup.LeftMotorSpeed = (ushort)(XInput.MAX_VIBRATION_LEVEL * Mathf.Abs(engines.x));
            }
            if (engines.y >= 0f)
            {
                XInput._vibratorsSetup.RightMotorSpeed = (ushort)(XInput.MAX_VIBRATION_LEVEL * Mathf.Abs(engines.y));
            }

            if (XInput._gamepad == null)
            {
                XInput._gamepad = new SharpDX.XInput.Controller(SharpDX.XInput.UserIndex.One);
            }
            else if (XInput._gamepad.IsConnected)
            {
                return (XInput._gamepad.SetVibration(XInput._vibratorsSetup) == SharpDX.Result.Ok);
            }

            return false;

#elif ENABLE_WINMD_SUPPORT // UWP:

            if (Windows.Gaming.Input.Gamepad.Gamepads.Count > 0)
            {
                if (engines.x >= 0f)
                {
                    XInput._vibrationSetup.LeftMotor = Mathf.Abs(engines.x);
                }
                if (engines.y >= 0f)
                {
                    XInput._vibrationSetup.RightMotor = Mathf.Abs(engines.y);
                }

                Windows.Gaming.Input.Gamepad.Gamepads[0].Vibration = XInput._vibrationSetup;

                return true;
            }
            else
            {
                return false;
            }

#else
            return false;
#endif
        }

        /// <summary>
        /// Set impulse for triggers.
        /// </summary>
        /// <param name="impulse">Impulse values for left (x) and right (y) triggers. Axis values from 0 to 1. -1 keeps the current impulse value.</param>
        /// <returns>Return true if a Xbox360/XBox One controller or compatible is conected and available as first player.</returns>
        /// <remarks>Triggers vibrators are only available for XBox One controllers and only in UWP builds. Not available from editor.</remarks>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool SetTriggersImpulse(Vector2 impulse)
        {
#if ENABLE_WINMD_SUPPORT // Only UWP:

            if (Windows.Gaming.Input.Gamepad.Gamepads.Count > 0)
            {
                if (impulse.x >= 0f)
                {
                    XInput._vibrationSetup.LeftTrigger = Mathf.Abs(impulse.x);
                }
                if (impulse.y >= 0f)
                {
                    XInput._vibrationSetup.RightTrigger = Mathf.Abs(impulse.y);
                }

                Windows.Gaming.Input.Gamepad.Gamepads[0].Vibration = XInput._vibrationSetup;

                return true;
            }

#endif
            return false;
        }
        #endregion
    }
}