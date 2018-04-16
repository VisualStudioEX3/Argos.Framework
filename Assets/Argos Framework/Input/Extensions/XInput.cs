#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#define ENABLE_XINPUT_SUPPORT
#endif

#if ENABLE_XINPUT_SUPPORT || ENABLE_WINMD_SUPPORT
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
#if ENABLE_XINPUT_SUPPORT
using System.Runtime.InteropServices;
#endif

namespace Argos.Framework.Input
{
    /// <summary>
    /// Microsoft XInput interface used for manage the XBox360/XBoxn One controller vibrators.
    /// </summary>
    /// <remarks>This class implemented code for desktop and UWP versions. The UWP code bring support for trigger vibrators (only for builds, not from editor).</remarks>
    public static class XInput
    {
#if ENABLE_XINPUT_SUPPORT // Desktop version:
        #region Constants
        const int ERROR_SUCCESS = 0;
        const ushort MAX_VIBRATION_LEVEL = 65000;
        const int PLAYER_ONE = 0;
        #endregion

        #region Structs
        [StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct XInputVibration
        {
            public ushort LeftMotorSpeed;
            public ushort RightMotorSpeed;

            public XInputVibration(ushort left, ushort right)
            {
                LeftMotorSpeed = left;
                RightMotorSpeed = right;
            }
        }
        #endregion

        #region DLL Imports
        [DllImport("xinput9_1_0.dll", EntryPoint = "XInputSetState", CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.Winapi)]
        static extern int SetState(int dwUserIndex, ref XInputVibration pVibration);
        #endregion

        #region Vars
        static XInputVibration _vibratorsSetup = new XInputVibration();
        #endregion

        #region Methods & Functions
        /// <summary>
        /// Set vibrators force values.
        /// </summary>
        /// <param name="left">Left engine.</param>
        /// <param name="right">Right engine.</param>
        /// <returns>Return true if a Xbox360/XBox One controller or compatible is conected and available as first player.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool SetVibration(Vector2 engines)
        {
            XInput._vibratorsSetup.LeftMotorSpeed = (ushort)(XInput.MAX_VIBRATION_LEVEL * Mathf.Abs(engines.x));
            XInput._vibratorsSetup.RightMotorSpeed = (ushort)(XInput.MAX_VIBRATION_LEVEL * Mathf.Abs(engines.y));

            return XInput.SetState(XInput.PLAYER_ONE, ref XInput._vibratorsSetup) == XInput.ERROR_SUCCESS;
        }
        #endregion
#elif ENABLE_WINMD_SUPPORT // UWP version:
        #region Methods & Functions
        /// <summary>
        /// Set vibrators force values.
        /// </summary>
        /// <param name="engines">Axis force for engines (x = left engine, y = right engine).</param>
        /// <returns>Return true if a Xbox360/XBox One controller or compatible is conected and available as first player.</returns>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool SetVibration(Vector2 engines)
        {
            return XInput.SetVibration(engines, Vector2.zero);
        }

        /// <summary>
        /// Set vibrators force values.
        /// </summary>
        /// <param name="engines">Axis force for engines (x = left engine, y = right engine).</param>
        /// <param name="triggers">Axis force for triggers (x = left trigger, y = right trigger).</param>
        /// <returns>Return true if a Xbox360/XBox One controller or compatible is conected and available as first player.</returns>
        public static bool SetVibration(Vector2 engines, Vector2 triggers)
        {
            if (Windows.Gaming.Input.Gamepad.Gamepads.Count > 0)
            {
                Windows.Gaming.Input.Gamepad.Gamepads[0].Vibration = new Windows.Gaming.Input.GamepadVibration()
                {
                    LeftMotor = Mathf.Abs(engines.x),
                    RightMotor = Mathf.Abs(engines.y),
                    LeftTrigger = Mathf.Abs(triggers.x),
                    RightTrigger = Mathf.Abs(triggers.y)
                };

                return true;
            }

            return false;
        } 
        #endregion
#endif
    }
} 
#endif