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
        const int ERROR_SUCCESS = 0;
        const ushort MAX_VIBRATION_LEVEL = 65000;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        struct GamepadVibration
        {
            public ushort LeftMotor;
            public ushort RightMotor;

            public GamepadVibration(ushort left, ushort right)
            {
                LeftMotor = left;
                RightMotor = right;
            }
        }
        #endregion

        #region DLL Imports
        [DllImport("xinput9_1_0.dll", EntryPoint = "XInputSetState", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        static extern int SetState(int dwUserIndex, ref GamepadVibration pVibration);
        #endregion

        #region Vars
        static GamepadVibration _vibratorsSetup = new GamepadVibration();
        #endregion

#elif ENABLE_WINMD_SUPPORT

        #region Vars
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
                XInput._vibratorsSetup.LeftMotor = (ushort)(XInput.MAX_VIBRATION_LEVEL * Mathf.Abs(engines.x));
            }
            if (engines.y >= 0f)
            {
                XInput._vibratorsSetup.RightMotor = (ushort)(XInput.MAX_VIBRATION_LEVEL * Mathf.Abs(engines.y));
            }

            return XInput.SetState(0, ref XInput._vibratorsSetup) == XInput.ERROR_SUCCESS;

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